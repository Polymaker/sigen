using SiGen.Common;
using SiGen.Configuration.Display;
using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public partial class LayoutViewer
    {
        #region Hardcoded Colors

        private static Color NylonFill = Color.FromArgb(180, 235, 235, 235);
        private static Color NylonOutline = Color.FromArgb(180, 200, 200, 200);

        private static Color PlainSteelFill = Color.Gainsboro;
        private static Color PlainSteelOutline = Color.DarkGray;

        private static Color SteelWoundFill = Color.Silver;
        private static Color SteelWoundOutline = Color.Gray;

        private static Color BronzeWoundFill = Color.FromArgb(194, 137, 33);
        private static Color BronzeWoundOutline = Color.FromArgb(169, 91, 14);

        #endregion

        #region Graphics Extentions

        private static readonly Vector FlipY = new Vector(1, -1);

        private Vector PointToVector(PointM pos)
        {
            //if (DisplayConfig.FretboardOrientation == Orientation.Horizontal)
            //    return new Vector(pos.Y.NormalizedValue, pos.X.NormalizedValue * -1);
            return new Vector(pos.X.NormalizedValue, pos.Y.NormalizedValue);
        }

        private PointF VectorToDisplay(Vector vec)
        {
            if(IsFlipHorizontal)
                vec = new Vector(vec.Y * -1, vec.X);
            else if (IsHorizontal)
                vec = new Vector(vec.Y, vec.X * -1);
            return (PointF)(vec * FlipY);
        }

        private PointF PointToDisplay(PointM pos)
        {
            return VectorToDisplay(PointToVector(pos));
        }

        private Pen GetPen(Color color, Measure size)
        {
            return new Pen(color, (float)size.NormalizedValue);
        }

        private Pen GetPen(Color color, Measure size, double offset)
        {
            return new Pen(color, (float)size.NormalizedValue + (float)(offset / _Zoom));
        }

        private Pen GetPen(Color color, double size)
        {
            return new Pen(color, (float)(size / _Zoom));
        }

        private void DrawLine(Graphics g, Pen pen, PointM p1, PointM p2)
        {
            g.DrawLine(pen, PointToDisplay(p1), PointToDisplay(p2));
        }

        private void DrawLine(Graphics g, LayoutLine line, Pen pen)
        {
            DrawLine(g, pen, line.P1, line.P2);
        }

        private void DrawLine(Graphics g, LayoutPolyLine line, Pen pen)
        {
            var linePoints = line.GetLinePoints()
                .Select(x => PointToDisplay(x)).ToArray();
            g.DrawCurve(pen, linePoints, 0.5f);
        }

        //private GraphicsPath GetLinePath(PointM p1, PointM p2, Measure thickness)
        //{
        //    var line = Line.FromPoints(p1.ToVector(), p2.ToVector());
        //    var perpVec = line.GetPerpendicular(p1.ToVector());
        //    var p1a = p1 + (perpVec.Vector * (thickness * -0.5d));
        //    var p1b = p1 + (perpVec.Vector * (thickness * 0.5d));
        //    var p2a = p2 + (perpVec.Vector * (thickness * -0.5d));
        //    var p2b = p2 + (perpVec.Vector * (thickness * 0.5d));
        //    var gp = new GraphicsPath();
        //    gp.StartFigure();
        //    gp.AddLine(PointToDisplay(p1a), PointToDisplay(p2a));
        //    gp.AddLine(PointToDisplay(p2b), PointToDisplay(p1b));
        //    gp.CloseFigure();

        //    return gp;
        //}

        //private GraphicsPath GetLinePath(PointM p1, PointM p2, Measure thickness, out PointF gradPt1, out PointF gradPt2)
        //{
        //    var line = Line.FromPoints(p1.ToVector(), p2.ToVector());
        //    var perpVec = line.GetPerpendicular(p1.ToVector());
        //    var p1a = p1 + (perpVec.Vector * (thickness * -0.5d));
        //    var p1b = p1 + (perpVec.Vector * (thickness * 0.5d));
        //    var p2a = p2 + (perpVec.Vector * (thickness * -0.5d));
        //    var p2b = p2 + (perpVec.Vector * (thickness * 0.5d));
        //    var gp = new GraphicsPath();
        //    gp.StartFigure();
        //    gp.AddLine(PointToDisplay(p1a), PointToDisplay(p2a));
        //    gp.AddLine(PointToDisplay(p2b), PointToDisplay(p1b));
        //    gp.CloseFigure();

        //    p1a = p1 + (perpVec.Vector * (thickness * -2d));
        //    p1b = p1 + (perpVec.Vector * (thickness * 2d));
        //    gradPt1 = PointToDisplay(p1a);
        //    gradPt2 = PointToDisplay(p1b);

        //    return gp;
        //}

        private void DrawLines(Graphics g, Pen pen, IEnumerable<PointM> points)
        {
            g.DrawLines(pen, points.Select(p => PointToDisplay(p)).ToArray());
        }

        #endregion

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            var center = new PointF(Width / 2f, Height / 2f);
            pe.Graphics.TranslateTransform(center.X, center.Y);
            pe.Graphics.ScaleTransform((float)_Zoom, (float)_Zoom);
            pe.Graphics.TranslateTransform((float)cameraPosition.X * -1, (float)cameraPosition.Y);


            //var topLeftPt = new Point(0, 0);
            //var botRightPt = new Point(Width, Height);

            //if (!pe.ClipRectangle.IsEmpty)
            //{
            //    topLeftPt = pe.ClipRectangle.Location;
            //    botRightPt = new Point(pe.ClipRectangle.Right, pe.ClipRectangle.Bottom);
            //}

            //var topLeftM = new PointM(DisplayToWorld(topLeftPt), UnitOfMeasure.Mm);
            //var botRightM = new PointM(DisplayToWorld(botRightPt), UnitOfMeasure.Mm);
            //var clipBounds = RectangleM.FromLTRB(topLeftM.X, botRightM.Y, botRightM.X, topLeftM.Y);

            if (CurrentLayout != null)
            {
                RenderFingerboard(pe.Graphics);
                RenderGuideLines(pe.Graphics);

                if (DisplayConfig.ShowFrets)
                    RenderFrets(pe.Graphics);

                if (DisplayConfig.ShowStrings)
                    RenderStrings(pe.Graphics);
            }

            pe.Graphics.ResetTransform();

            if (EnableMeasureTool)
                RenderMeasureTool(pe.Graphics);
        }

        private void InvalidateWorldRegion(int bleedSize, params Vector[] points)
        {
            Vector minPos = Vector.Empty;
            Vector maxPos = Vector.Empty;

            for (int i = 0; i < points.Length; i++)
            {
                if (!points[i].IsEmpty)
                {
                    if (minPos.IsEmpty)
                    {
                        minPos = points[i];
                        maxPos = points[i];
                    }
                    else
                    {
                        minPos = Vector.Min(minPos, points[i]);
                        maxPos = Vector.Max(maxPos, points[i]);
                    }
                }
            }

            if (!minPos.IsEmpty)
            {
                var minPt = WorldToDisplay(minPos, _Zoom, true);
                var maxPt = WorldToDisplay(maxPos, _Zoom, true);

                if (IsFlipHorizontal)
                {
                    var tmp1 = minPt;
                    var tmp2 = maxPt;
                    minPt = new PointF(Math.Min(tmp1.X, tmp2.X), Math.Min(tmp1.Y, tmp2.Y));
                    maxPt = new PointF(Math.Max(tmp1.X, tmp2.X), Math.Max(tmp1.Y, tmp2.Y));
                }

                var updateBounds = Rectangle.FromLTRB((int)minPt.X - bleedSize, (int)minPt.Y - bleedSize, (int)maxPt.X + bleedSize, (int)maxPt.Y + bleedSize);

                
                Invalidate(updateBounds);
            }
        }


        public bool IsElementVisible(VisualElement element)
        {
            switch (element.ElementType)
            {
                case VisualElementType.CenterLine:
                    return DisplayConfig.ShowCenterLine;

                case VisualElementType.Fret:
                    return DisplayConfig.ShowFrets;

                case VisualElementType.String:
                    return DisplayConfig.ShowStrings;

                case VisualElementType.FingerboardMargin:
                    return DisplayConfig.ShowMargins && !DisplayConfig.ShowStrings;

                case VisualElementType.StringMidline:
                    return DisplayConfig.ShowMidlines;

                case VisualElementType.FingerboardContinuation:
                case VisualElementType.FingerboardEdge:
                    return DisplayConfig.ShowFingerboard;

                default:
                    return false;
            }
            
        }

        private void RenderFingerboard(Graphics g)
        {
            if (DisplayConfig.ShowCenterLine)
            {
                var centerLine = CurrentLayout.VisualElements.OfType<LayoutLine>()
                    .FirstOrDefault(x => x.ElementType == VisualElementType.CenterLine);

                using (var guidePen = GetPen(DisplayConfig.CenterLine.Color, 1))
                    DrawLine(g, guidePen, centerLine.P1, centerLine.P2);
            }

            if (DisplayConfig.ShowMidlines)
            {
                using (var guidePen = GetPen(DisplayConfig.Midlines.Color, 1))
                {
                    guidePen.DashPattern = new float[] { 6, 4, 2, 4 };

                    foreach (var stringCenter in CurrentLayout.GetElements<StringCenter>())
                    {
                        bool isExactCenter = stringCenter.Equation.IsVertical && stringCenter.Equation.X == 0;
                        if (!DisplayConfig.ShowCenterLine || !isExactCenter)
                            DrawLine(g, guidePen, stringCenter.P1, stringCenter.P2);
                    }
                }
            }

            if (!DisplayConfig.ShowStrings && DisplayConfig.ShowMargins)
            {
                var margins = CurrentLayout.VisualElements.OfType<LayoutLine>()
                    .Where(x => x.ElementType == VisualElementType.FingerboardMargin);

                if (margins.Any())
                {
                    using (var guidePen = GetPen(DisplayConfig.Margins.Color, 1))
                    {
                        foreach (var marginLine in margins)
                            DrawLine(g, guidePen, marginLine.P1, marginLine.P2);
                    }
                }
            }

            if (DisplayConfig.ShowFingerboard)
            {
                using (var edgePen = GetPen(DisplayConfig.Fingerboard.Color, 1))
                {
                    foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardEdge>())
                        DrawLines(g, edgePen, edge.Points);

                    foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardSideEdge>())
                        DrawLine(g, edgePen, edge.P1, edge.P2);

                    if (!DisplayConfig.ShowFrets)
                    {
                        var nutLines = CurrentLayout.GetElements<FretLine>(x => x.IsNut);
                        foreach (var line in nutLines)
                            DrawLine(g, line, edgePen);
                    }
                }
            }
        }

        private void RenderGuideLines(Graphics g)
        {
            using (var guidePen = GetPen(Color.Gainsboro, 1))
            {
                guidePen.DashPattern = new float[] { 6, 4, 2, 4 };

                if (DisplayConfig.ShowFingerboard && DisplayConfig.Fingerboard.ContinueLines)
                {
                    var endPoints = new List<PointM>();

                    foreach (var line in CurrentLayout.GetElements<LayoutLine>(l => 
                        l.ElementType == VisualElementType.FingerboardContinuation))
                    {
                        DrawLine(g, line, guidePen);
                        endPoints.Add(line.P2);
                    }

                    var pt1 = endPoints.OrderBy(pt => pt.X).First();
                    var pt2 = endPoints.OrderByDescending(pt => pt.X).First();
                    DrawLine(g, guidePen, pt1, pt2);
                }

                foreach (var line in CurrentLayout.GetElements<LayoutLine>(l => 
                    l.ElementType == VisualElementType.GuideLine))
                {
                    DrawLine(g, line, guidePen);
                }
            }
        }

        private void RenderFrets(Graphics g)
        {
            Pen fretPen = null;
            Pen nutPen = GetPen(DisplayConfig.Frets.Color, 1);

            switch (DisplayConfig.Frets.RenderMode)
            {
                default:
                case LineRenderMode.PlainLine:
                    fretPen = GetPen(DisplayConfig.Frets.Color, 1);
                    break;
                case LineRenderMode.RealWidth:
                    fretPen = GetPen(DisplayConfig.Frets.Color, DisplayConfig.Frets.RenderWidth);
                    break;
                case LineRenderMode.RealisticLook:
                    fretPen = GetPen(Color.DarkGray, DisplayConfig.Frets.RenderWidth);
                    break;
            }

            foreach (var fretLine in CurrentLayout.VisualElements.OfType<FretLine>())
            {
                var penToUse = fretLine.IsNut ? nutPen : fretPen;
                var fretPoints = fretLine.Points.Select(p => PointToDisplay(p)).ToArray();

                if (DisplayConfig.ExtendFrets && fretLine.Tag is ILayoutLine layoutLine)
                {
                    fretPoints = layoutLine.GetLinePoints()
                        .Select(x => PointToDisplay(x)).ToArray();
                }

                if (fretLine.IsStraight || CurrentLayout.FretInterpolation == FretInterpolationMethod.Linear)
                    g.DrawLines(penToUse, fretPoints);
                else
                    g.DrawCurve(penToUse, fretPoints, 0.6f);

                if (DisplayConfig.Frets.DisplayAccuratePositions && fretLine.Strings.Count() > 1)
                {
                    g.DrawLines(nutPen, fretLine.Segments.Where(s => !s.IsVirtual)
                        .Select(s => PointToDisplay(s.PointOnString)).ToArray());
                }
            }

            if (DisplayConfig.Frets.DisplayBridgeLine)
            {
                var bridgeLine = CurrentLayout.GetElement<LayoutPolyLine>(x => x.ElementType == VisualElementType.BridgeLine);
                if (bridgeLine != null)
                {
                    DrawLine(g, bridgeLine, nutPen);
                }
            }

            nutPen.Dispose();
            fretPen.Dispose();
        }

        private void RenderStrings(Graphics g)
        {
            using (var defaultPen = GetPen(DisplayConfig.Strings.Color, 1))
            {
                foreach (var stringLine in CurrentLayout.VisualElements.OfType<StringLine>())
                {
                    var renderMode = DisplayConfig.Strings.RenderMode;
                    if (!(stringLine.String.Gauge > Measure.Zero))
                        renderMode = LineRenderMode.PlainLine;

                    switch (renderMode)
                    {
                        default:
                        case LineRenderMode.PlainLine:
                            DrawLine(g, defaultPen, stringLine.P1, stringLine.P2);
                            break;
                        case LineRenderMode.RealWidth:
                            using (var gaugePen = GetPen(defaultPen.Color, stringLine.String.Gauge))
                                DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);
                            break;
                        case LineRenderMode.RealisticLook:
                            {
                                var stringMaterial = StringMaterial.PlainSteel;

                                if (stringLine.String.Gauge.NormalizedValue >= 0.06090)
                                    stringMaterial = StringMaterial.SteelWound;

                                GetStringMaterialColors(stringMaterial, out Color stringColor, out Color outlineColor);

                                using (var gaugePen = GetPen(outlineColor, stringLine.String.Gauge, 1.8))
                                    DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);

                                using (var gaugePen = GetPen(stringColor, stringLine.String.Gauge))
                                    DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);


                                var highlightSize = (stringLine.String.Gauge * 0.5d).NormalizedValue * _Zoom;

                                if (highlightSize >= 0.5d)
                                {
                                    var testP1 = stringLine.P1;
                                    testP1.X -= (stringLine.String.Gauge * 0.1);
                                    var testP2 = stringLine.P2;
                                    testP2.X -= (stringLine.String.Gauge * 0.1);

                                    using (var gaugePen = GetPen(Color.FromArgb(80, 255, 255, 255), stringLine.String.Gauge * 0.50d))
                                        DrawLine(g, gaugePen, testP1, testP2);

                                    //using (var gaugePen = GetPen(Color.FromArgb(80, 255, 255, 255), stringLine.String.Gauge * 0.2d))
                                    //    DrawLine(g, gaugePen, testP1, testP2);
                                }
                                

                                break;
                            }
                    }
                }
            }
        }

        private void GetStringMaterialColors(StringMaterial material, out Color fillColor, out Color outlineColor)
        {
            switch (material)
            {
                default:
                case StringMaterial.PlainSteel:
                    outlineColor = PlainSteelOutline;
                    fillColor = PlainSteelFill;
                    break;
                case StringMaterial.SteelWound:
                    outlineColor = SteelWoundOutline;
                    fillColor = SteelWoundFill;
                    break;
                case StringMaterial.BronzeWound:
                    outlineColor = BronzeWoundOutline;
                    fillColor = BronzeWoundFill;
                    break;
                case StringMaterial.Nylon:
                    outlineColor = NylonOutline;
                    fillColor = NylonFill;
                    break;
            }
        }

        private void RenderMeasureTool(Graphics g)
        {
            if (!MeasureSnapPosition.IsEmpty && MeasureLastSelection.IsEmpty)
            {
                var snapPt = WorldToDisplay(MeasureSnapPosition, _Zoom, true);
                using (var brush = new SolidBrush(Color.FromArgb(140, 50, 50, 200)))
                    g.FillEllipse(brush, snapPt.X - 6, snapPt.Y - 6, 12, 12);
            }

            if (MeasureLastSelection.IsEmpty)
            {
                using (var pen = new Pen(Color.Black, 2))
                {
                    //g.DrawLine(pen, WorldToDisplay(MeasureFirstSelection, _Zoom, true), PointToClient(Cursor.Position));
                    g.DrawLine(pen, WorldToDisplay(MeasureFirstSelection, _Zoom, true), WorldToDisplay(MeasureSnapPosition, _Zoom, true));
                }
            }
            else if (!MeasureFirstSelection.IsEmpty)
            {
                if(CurrentMeasure != null)
                {
                    var lengthMeasureBoxes = MeasureBoxes.OfType<LengthValueBox>();
                    bool showLengthOnly = lengthMeasureBoxes.Any(b => b.Type != LengthType.Length && !b.IsMeasureVisible());

                    //Draw Measures Lines
                    foreach (var lengthBox in lengthMeasureBoxes)
                    {
                        if (lengthBox.Suppressed || (showLengthOnly && lengthBox.Type != LengthType.Length))
                            continue;

                        var p1UI = WorldToDisplay(lengthBox.P1, _Zoom, true);
                        var p2UI = WorldToDisplay(lengthBox.P2, _Zoom, true);
                        using (var pen = new Pen(GetMeasureTypeColor(lengthBox.Type), 2))
                            g.DrawLine(pen, p1UI, p2UI);
                    }

                    if (!IsMovingCamera)
                    {
                        //Draw Pointer Lines
                        foreach (var lengthBox in MeasureBoxes)
                        {
                            if (lengthBox.Suppressed || (lengthBox is LengthValueBox && (showLengthOnly && (lengthBox as LengthValueBox).Type != LengthType.Length)))
                                continue;

                            var targetUI = WorldToDisplay(lengthBox.TargetPosition, _Zoom, true);
                            var centerUI = new PointF(targetUI.X + (float)lengthBox.LocalOffset.X, targetUI.Y + (float)lengthBox.LocalOffset.Y * -1f);
                            g.DrawLine(Pens.Black, targetUI, centerUI);
                        }

                        //Draw Measure Boxes
                        foreach (var lengthBox in lengthMeasureBoxes)
                        {
                            if (lengthBox.Suppressed || (showLengthOnly && lengthBox.Type != LengthType.Length))
                                continue;

                            g.SmoothingMode = SmoothingMode.HighSpeed;
                            g.PixelOffsetMode = PixelOffsetMode.None;

                            g.FillRectangle(Brushes.White, lengthBox.DisplayBounds);

                            using (var pen = new Pen(GetMeasureTypeColor(lengthBox.Type), 1f))
                                g.DrawRectangle(pen, lengthBox.DisplayBounds);

                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.SmoothingMode = SmoothingMode.AntiAlias;

                            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far })
                                g.DrawString(lengthBox.GetDisplayValue(), Font, Brushes.Black, lengthBox.DisplayBounds, sf);
                        }

                        foreach (var box in MeasureBoxes.OfType<AngleValueBox>())
                        {
                            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far })
                                g.DrawString(box.GetDisplayValue(), Font, Brushes.Black , box.DisplayBounds, sf);
                        }
                    }
                }
            }
        }

        private RectangleF GetMeasureBoxBounds(MeasureValueBox box)
        {
            var boxCorner = LocalToDisplay(WorldToLocal(box.TargetPosition, _Zoom, true) + box.LocalOffset);
            boxCorner.X -= box.Size.Width / 2f;
            boxCorner.Y -= box.Size.Height / 2f;
            return new RectangleF(boxCorner, box.Size);
        }

        private static Color GetMeasureTypeColor(LengthType type)
        {
            switch (type)
            {
                default:
                case LengthType.Length:
                    return Color.Black;
                case LengthType.Width:
                    return Color.Blue;
                case LengthType.Height:
                    return Color.Red;
            }
        }

    }
}
