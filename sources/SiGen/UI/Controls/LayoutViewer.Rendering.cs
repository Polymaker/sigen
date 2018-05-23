using SiGen.Maths;
using SiGen.Measuring;
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

        private void RenderFingerboard(Graphics g)
        {
            if (DisplayConfig.ShowCenterLine)
            {
                var layoutBounds = CurrentLayout.GetLayoutBounds();
                using (var guidePen = GetPen(Color.Black, 1))
                {
                    DrawLine(g, guidePen, new PointM(Measure.Zero, layoutBounds.Top), new PointM(Measure.Zero, layoutBounds.Bottom));
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

            using (var edgePen = GetPen(Color.Blue, 1))
            {
                foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardEdge>())
                    DrawLines(g, edgePen, edge.Points);

                foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardSideEdge>())
                    DrawLine(g, edgePen, edge.P1, edge.P2);
            }
        }

        private void RenderGuideLines(Graphics g)
        {
            using (var guidePen = GetPen(Color.Gainsboro, 1))
            {
                guidePen.DashPattern = new float[] { 6, 4, 2, 4 };
                foreach (var line in CurrentLayout.VisualElements.OfType<LayoutLine>().Where(l => l.ElementType == VisualElementType.GuideLine))
                    DrawLine(g, guidePen, line.P1, line.P2);
            }
        }

        private void RenderFrets(Graphics g)
        {
            Pen fretPen = null;
            Pen nutPen = GetPen(Color.Red, 1);

            if (!DisplayConfig.RenderRealFrets)
                fretPen = GetPen(Color.Red, 1);
            else
                fretPen = GetPen(Color.DarkGray, DisplayConfig.FretWidth);

            foreach (var fretLine in CurrentLayout.VisualElements.OfType<FretLine>())
            {
                if (DisplayConfig.ShowFrets)
                {
                    var penToUse = fretLine.IsNut ? nutPen : fretPen;
                    var fretPoints = fretLine.Points.Select(p => PointToDisplay(p)).ToArray();
                    if (fretLine.IsStraight || CurrentLayout.FretInterpolation == StringedInstruments.Layout.FretInterpolationMethod.Linear)
                        g.DrawLines(penToUse, fretPoints);
                    else
                        g.DrawCurve(penToUse, fretPoints, 0.5f);
                }

                if(DisplayConfig.ShowTheoreticalFrets && fretLine.Strings.Count() > 1)
                {
                    g.DrawLines(nutPen, fretLine.Segments.Where(s => !s.IsVirtual).Select(s => PointToDisplay(s.PointOnString)).ToArray());
                }
                
                //g.DrawLines(nutPen, fretLine.Points.Select(p => PointToDisplay(p)).ToArray());
            }
           
            nutPen.Dispose();
            fretPen.Dispose();
        }

        private void RenderFretMarkers(Graphics g)
        {
            var bassLine = CurrentLayout.GetStringBoundaryLine(CurrentLayout.LastString, StringedInstruments.Layout.FingerboardSide.Bass);
            var trebleLine = CurrentLayout.GetStringBoundaryLine(CurrentLayout.FirstString, StringedInstruments.Layout.FingerboardSide.Treble);
            var bassLineVector = FixOrientation(bassLine.Equation.Vector);
            var bassOffsetVector = bassLine.Equation.GetPerpendicular(Vector.Zero).Vector * -1d;
            bassOffsetVector = FixOrientation(bassOffsetVector);

            var trebleOffsetVector = trebleLine.Equation.GetPerpendicular(Vector.Zero).Vector;
            trebleOffsetVector = FixOrientation(trebleOffsetVector);

            using (var sf = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center })
            {
                for (int i = CurrentLayout.MinimumFret; i <= CurrentLayout.MaximumFret; i++)
                {

                    var frets = CurrentLayout.VisualElements.OfType<FretLine>().Where(f => f.FretIndex == i).OrderBy(f => f.Strings.Min(s => s.Index));
                    Vector bassFretPos = Vector.Empty;
                    Vector trebleFretPos = Vector.Empty;

                    if (frets.Count() >= 1)
                    {
                        PointM inter;
                        if (frets.Last().Intersects(bassLine, out inter))
                            bassFretPos = WorldToLocal(inter.ToVector(), _Zoom, true);
                        if (frets.First().Intersects(trebleLine, out inter))
                            trebleFretPos = WorldToLocal(inter.ToVector(), _Zoom, true);
                    }
                    int fretPos = Math.Abs(i);

                    if((fretPos % 12 == 0 && (fretPos != 0 || CurrentLayout.MinimumFret < 0)) || (fretPos % 2 == 1 && fretPos % 12 > 2 && fretPos % 12 < 10))
                    {
                        if (!bassFretPos.IsEmpty)
                        {
                            if(i % 12 == 0)
                            {
                                var pos1 = LocalToDisplay(bassFretPos + (bassOffsetVector * 5) + (bassLineVector * 3));
                                var pos2 = LocalToDisplay(bassFretPos + (bassOffsetVector * 5) - (bassLineVector * 3));
                                DrawSideDot(g, pos1);
                                DrawSideDot(g, pos2);
                            }
                            else
                            {
                                var pos = LocalToDisplay(bassFretPos + (bassOffsetVector * 5));
                                DrawSideDot(g, pos);
                            }
                        }
                        if (!trebleFretPos.IsEmpty)
                        {
                            var pos = LocalToDisplay(trebleFretPos + (trebleOffsetVector * 5));
                            g.DrawString(i.ToString(), Font, Brushes.Black, new RectangleF(pos.X - 15, pos.Y, 30, Font.Height + 3), sf);
                        }
                    }
                }
            }
        }

        private void DrawSideDot(Graphics g, PointF pos)
        {
            g.DrawEllipse(Pens.DarkSlateGray, pos.X - 2f, pos.Y - 2f, 4f, 4f);
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
            if (MeasureLastSelection.IsEmpty)
            {
                using (var pen = new Pen(Color.Black, 2))
                    g.DrawLine(pen, WorldToDisplay(MeasureFirstSelection, _Zoom, true), PointToClient(Cursor.Position));
            }
            else
            {
                if(CurrentMeasure != null)
                {
                    var lengthMeasureBoxes = MeasureBoxes.OfType<LengthValueBox>();
                    bool showLengthOnly = lengthMeasureBoxes.Any(b=>b.Type != LengthType.Length && !b.IsMeasureVisible());

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
