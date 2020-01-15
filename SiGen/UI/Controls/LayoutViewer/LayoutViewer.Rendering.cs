﻿using SiGen.Maths;
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

        private void DrawLines(Graphics g, Pen pen, IEnumerable<PointM> points)
        {
            g.DrawLines(pen, points.Select(p => PointToDisplay(p)).ToArray());
        }

        private void DrawRotatedString(Graphics g, string text, Font font, Brush brush, PointF center, Angle angle)
        {
            SizeF textSize = g.MeasureString(text, font);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform((float)angle.Degrees);
            g.DrawString(text, font, brush, -(textSize.Width / 2), -(textSize.Height / 2));
            g.ResetTransform();
        }

        #endregion

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

                case VisualElementType.StringCenter:
                    return DisplayConfig.ShowMidlines;

                case VisualElementType.FingerboardContinuation:
                case VisualElementType.FingerboardEdge:
                    return DisplayConfig.ShowFingerboard;

                default:
                    {

                        break;
                    }
            }
            return false;
        }

        private void RenderFingerboard(Graphics g)
        {
            if (DisplayConfig.ShowCenterLine)
            {
                //var layoutBounds = CurrentLayout.GetLayoutBounds();
                var centerLine = CurrentLayout.VisualElements.OfType<LayoutLine>().FirstOrDefault(x => x.ElementType == VisualElementType.CenterLine);
                using (var guidePen = GetPen(Color.Black, 1))
                {
                    DrawLine(g, guidePen, centerLine.P1, centerLine.P2);
                }
            }

            if (DisplayConfig.ShowMidlines)
            {
                using (var guidePen = GetPen(Color.Gainsboro, 1))
                {
                    guidePen.DashPattern = new float[] { 6, 4, 2, 4 };
                    foreach (var stringCenter in CurrentLayout.VisualElements.OfType<StringCenter>())
                    {
                        if(!DisplayConfig.ShowCenterLine || !(stringCenter.Equation.IsVertical && stringCenter.Equation.X == 0))
                            DrawLine(g, guidePen, stringCenter.P1, stringCenter.P2);
                    }
                }
            }

            if (!DisplayConfig.ShowStrings && DisplayConfig.ShowMargins)
            {
                var margins = CurrentLayout.VisualElements.OfType<LayoutLine>().Where(x => x.ElementType == VisualElementType.FingerboardMargin);
                if (margins.Any())
                {
                    using (var guidePen = GetPen(Color.Gray, 1))
                    {
                        foreach (var marginLine in margins)
                            DrawLine(g, guidePen, marginLine.P1, marginLine.P2);
                    }
                }
                else
                {
                    //LEGACY
                    var firstString = CurrentLayout.FirstString.LayoutLine;
                    var lastString = CurrentLayout.LastString.LayoutLine;
                    var trebleEdge = CurrentLayout.GetStringBoundaryLine(CurrentLayout.FirstString, FingerboardSide.Treble);
                    var bassEdge = CurrentLayout.GetStringBoundaryLine(CurrentLayout.LastString, FingerboardSide.Bass);

                    using (var guidePen = GetPen(Color.Gray, 1))
                    {
                        DrawLine(g, guidePen, firstString.P1, firstString.SnapToLine(trebleEdge.P2, LineSnapDirection.Horizontal));
                        if (firstString != lastString)
                            DrawLine(g, guidePen, lastString.P1, lastString.SnapToLine(bassEdge.P2, LineSnapDirection.Horizontal));
                    }
                }
                
            }

            if (DisplayConfig.ShowFingerboard)
            {
                using (var edgePen = GetPen(Color.Blue, 1))
                {
                    foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardEdge>())
                        DrawLines(g, edgePen, edge.Points);

                    foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardSideEdge>())
                        DrawLine(g, edgePen, edge.P1, edge.P2);
                }
            }
        }

        private void RenderGuideLines(Graphics g)
        {
            using (var guidePen = GetPen(Color.Gainsboro, 1))
            {
                guidePen.DashPattern = new float[] { 6, 4, 2, 4 };

                if (DisplayConfig.ShowFingerboard)
                {
                    foreach (var line in CurrentLayout.VisualElements.OfType<LayoutLine>().Where(l => l.ElementType == VisualElementType.FingerboardContinuation))
                        DrawLine(g, guidePen, line.P1, line.P2);
                }

                foreach (var line in CurrentLayout.VisualElements.OfType<LayoutLine>().Where(l => l.ElementType == VisualElementType.GuideLine))
                    DrawLine(g, guidePen, line.P1, line.P2);
            }
        }

        private void RenderFrets(Graphics g)
        {
            Pen fretPen = null;
            Pen nutPen = GetPen(DisplayConfig.FretLineColor, 1);

            if (!DisplayConfig.RenderRealFrets)
                fretPen = GetPen(DisplayConfig.FretLineColor, 1);
            else
                fretPen = GetPen(Color.DarkGray, DisplayConfig.FretWidth);

            foreach (var fretLine in CurrentLayout.VisualElements.OfType<FretLine>())
            {
                if (DisplayConfig.ShowFrets)
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
                        g.DrawCurve(penToUse, fretPoints, 0.5f);
                }

                if (DisplayConfig.ShowTheoreticalFrets && fretLine.Strings.Count() > 1)
                {
                    g.DrawLines(nutPen, fretLine.Segments.Where(s => !s.IsVirtual).Select(s => PointToDisplay(s.PointOnString)).ToArray());
                }

                //g.DrawLines(nutPen, fretLine.Points.Select(p => PointToDisplay(p)).ToArray());
            }

            nutPen.Dispose();
            fretPen.Dispose();
        }

        private void RenderStrings(Graphics g)
        {
            using(var defaultPen = GetPen(Color.Black, 1))
            {
                foreach (var stringLine in CurrentLayout.VisualElements.OfType<StringLine>())
                {
                    if (DisplayConfig.RenderRealStrings && stringLine.String.Gauge != Measure.Empty && stringLine.String.Gauge > Measure.Zero)
                    {
                        var outlineColor = stringLine.String.Gauge.NormalizedValue >= 0.06090 ? Color.Gray : Color.DarkGray;
                        var stringColor = stringLine.String.Gauge.NormalizedValue >= 0.06090 ? Color.Silver : Color.Gainsboro;


                        using (var gaugePen = GetPen(outlineColor, stringLine.String.Gauge, 1.5))
                            DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);

                        using (var gaugePen = GetPen(stringColor, stringLine.String.Gauge))
                            DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);

                        //if(stringLine.String.Gauge.NormalizedValue >= 0.06090)
                        //{
                        //    using (var gaugePen = GetPen(Color.FromArgb(100, outlineColor), stringLine.String.Gauge))
                        //    {
                        //        float offset = (1f / gaugePen.Width) / 10f;
                        //        gaugePen.DashPattern = new float[] { .5f * offset, 1.0f * offset, .5f * offset, 1.0f * offset };
                        //        DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);
                        //    }
                        //}
                    }
                    else
                        DrawLine(g, defaultPen, stringLine.P1, stringLine.P2);
                }
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
