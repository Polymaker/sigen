﻿using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            if (IsHorizontal)
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

        private Pen GetPen(Color color, double size)
        {
            return new Pen(color, (float)(size / _Zoom));
        }

        private void DrawLine(Graphics g, Pen pen, PointM p1, PointM p2)
        {
            g.DrawLine(pen, PointToDisplay(p1), PointToDisplay(p2));
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
            using(var guidePen = GetPen(Color.Gainsboro, 1))
            {
                guidePen.DashPattern = new float[] { 6, 4, 2, 4 };
                foreach (var stringCenter in CurrentLayout.VisualElements.OfType<StringCenter>())
                    DrawLine(g, guidePen, stringCenter.P1, stringCenter.P2);
            }

            using (var edgePen = GetPen(Color.Blue, 1))
            {
                foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardEdge>())
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
                var penToUse = fretLine.IsNut ? nutPen : fretPen;
                var fretPoints = fretLine.Points.Select(p => PointToDisplay(p)).ToArray();
                if (fretLine.IsStraight)
                    g.DrawLines(penToUse, fretPoints);
                else
                    g.DrawCurve(penToUse, fretPoints, 0.3f);

            }
            nutPen.Dispose();
            fretPen.Dispose();
        }

        private void RenderStrings(Graphics g)
        {
            using(var stringPen = GetPen(Color.Black, 1))
            {
                foreach (var stringLine in CurrentLayout.VisualElements.OfType<StringLine>())
                {
                    if (DisplayConfig.RenderRealStrings && stringLine.String.Gauge != Measure.Empty && stringLine.String.Gauge > Measure.Zero)
                    {
                        using (var gaugePen = GetPen(Color.Black, stringLine.String.Gauge))
                            DrawLine(g, gaugePen, stringLine.P1, stringLine.P2);
                    }
                    else
                        DrawLine(g, stringPen, stringLine.P1, stringLine.P2);
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

                    //Draw Pointer Lines
                    foreach (var lengthBox in MeasureBoxes)
                    {
                        if (lengthBox.Suppressed || (lengthBox is LengthValueBox && (showLengthOnly && (lengthBox as LengthValueBox).Type != LengthType.Length)))
                            continue;

                        var targetUI = WorldToDisplay(lengthBox.TargetPosition, _Zoom, true);
                        var centerUI = new PointF(targetUI.X + (float)lengthBox.LocalOffset.X, targetUI.Y + (float)lengthBox.LocalOffset.Y*-1f);
                        g.DrawLine(Pens.Black, targetUI, centerUI);
                    }

                    //Draw Measure Boxes
                    foreach (var lengthBox in lengthMeasureBoxes)
                    {
                        if (lengthBox.Suppressed || (showLengthOnly && lengthBox.Type != LengthType.Length))
                            continue;

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                        g.FillRectangle(Brushes.White, lengthBox.DisplayBounds);

                        using (var pen = new Pen(GetMeasureTypeColor(lengthBox.Type), 1f))
                            g.DrawRectangle(pen, lengthBox.DisplayBounds);
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far })
                            g.DrawString(lengthBox.GetDisplayValue(), Font, Brushes.Black, lengthBox.DisplayBounds, sf);
                    }

                    foreach(var box in MeasureBoxes.OfType<AngleValueBox>())
                    {
                        using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far })
                            g.DrawString(box.GetDisplayValue(), Font, Brushes.Black, box.DisplayBounds, sf);
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
