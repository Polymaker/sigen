using SiGen.Maths;
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
            if (measurePos2.IsEmpty)
            {
                using (var pen = new Pen(Color.Black, 2))
                    g.DrawLine(pen, WorldToDisplay(measurePos1, _Zoom, true), PointToClient(Cursor.Position));
            }
            else
            {
                var pt1 = WorldToDisplay(measurePos1, _Zoom, true);
                var pt2 = WorldToDisplay(measurePos2, _Zoom, true);
                var pt3 = new PointF(Math.Min(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y));
                var pt4 = new PointF(Math.Max(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y));
                PointF pt5, pt6;

                if (pt3 == pt1 || pt3 == pt2)
                    pt5 = pt4;
                else
                    pt5 = pt3;

                if (pt3 == pt1 || pt4 == pt1)
                    pt6 = pt2;
                else
                    pt6 = pt1;

                bool lineLeft = (pt5.X == pt3.X);

                var measureLen = Measure.Cm((measurePos1 - measurePos2).Length);
                var measureWidth = Measure.Cm(IsHorizontal ? Math.Abs(measurePos2.Y - measurePos1.Y) : Math.Abs(measurePos2.X - measurePos1.X));
                var measureHeight = Measure.Cm(IsHorizontal ? Math.Abs(measurePos2.X - measurePos1.X) : Math.Abs(measurePos2.Y - measurePos1.Y));

                using (var pen = new Pen(Color.Black, 2))
                    g.DrawLine(pen, pt1, pt2);
                var lineCenter = new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);

                if (Math.Abs(pt1.X - pt2.X) > 1 && Math.Abs(pt1.Y - pt2.Y) > 1)
                {
                    using (var pen = new Pen(Color.Blue, 2))
                        g.DrawLine(pen, pt3, pt4);

                    using (var pen = new Pen(Color.Red, 2))
                        g.DrawLine(pen, pt5, pt6);

                    var horizontalCenter = new PointF((pt3.X + pt4.X) / 2f, pt3.Y);
                    var verticalCenter = new PointF(pt5.X, (pt5.Y + pt6.Y) / 2f);

                    DrawMeasureBox(g, measureWidth, Color.Blue, horizontalCenter, StringAlignment.Center, Orientation.Horizontal);

                    DrawMeasureBox(g, measureHeight, Color.Red, verticalCenter, lineLeft ? StringAlignment.Far : StringAlignment.Near, Orientation.Vertical);
                    lineCenter.Y -= Font.Height;
                    DrawMeasureBox(g, measureLen, Color.Black, lineCenter, lineLeft ? StringAlignment.Near : StringAlignment.Far, Orientation.Vertical);
                }
                else
                {
                    if (Math.Abs(pt1.X - pt2.X) <= 1)
                        DrawMeasureBox(g, measureLen, Color.Black, lineCenter, StringAlignment.Near, Orientation.Vertical);
                    else
                        DrawMeasureBox(g, measureLen, Color.Black, lineCenter, StringAlignment.Center, Orientation.Horizontal);
                }
            }
        }

        private void DrawMeasureBox(Graphics g, Measure value, Color color, PointF center, StringAlignment alignment, Orientation offsetDir)
        {
            SizeF textSize = g.MeasureString(value.ToString(DisplayUnit), Font);
            var txtBounds = new RectangleF(center.X, center.Y - (textSize.Height / 2), textSize.Width, textSize.Height);

            if (alignment == StringAlignment.Center)
                txtBounds.X -= (textSize.Width / 2);
            else if (alignment == StringAlignment.Far)
                txtBounds.X -= textSize.Width;

            if (offsetDir == Orientation.Horizontal)
                txtBounds.Y += (textSize.Height / 2) + 6;
            else
            {
                if (alignment == StringAlignment.Near)
                    txtBounds.X += 6;
                else if (alignment == StringAlignment.Far)
                    txtBounds.X -= 6;
            }

            txtBounds.Inflate(2, 2);
            g.FillRectangle(Brushes.White, txtBounds);
            using (var pen = new Pen(color, 1.5f))
                g.DrawRectangle(pen, txtBounds.X, txtBounds.Y, txtBounds.Width, txtBounds.Height);
            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(value.ToString(DisplayUnit), Font, Brushes.Black, txtBounds, sf);
        }

    }
}
