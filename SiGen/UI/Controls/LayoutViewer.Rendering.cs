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
            if (measurePos2.IsEmpty)
            {
                using (var pen = new Pen(Color.Black, 2))
                    g.DrawLine(pen, WorldToDisplay(measurePos1, _Zoom, true), PointToClient(Cursor.Position));
            }
            else
            {
                if(CurrentMeasure != null)
                {
                    bool showLengthOnly = false;
                    for (int i = 0; i < 3; i++)
                    {
                        var box = MeasureBoxes[i];
                        if (box == null || (box.Type != LengthType.Length && box.Value.NormalizedValue * _Zoom < 2))
                        {
                            showLengthOnly = true;
                            break;
                        }
                        box.Visible = true;
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        var box = MeasureBoxes[i];
                        if (box.Suppressed || (showLengthOnly && box.Type != LengthType.Length))
                        {
                            box.Visible = false;
                            continue;
                        }
                        
                        var p1UI = WorldToDisplay(box.P1, _Zoom, true);
                        var p2UI = WorldToDisplay(box.P2, _Zoom, true);
                        using (var pen = new Pen(GetMeasureTypeColor(box.Type), 2))
                            g.DrawLine(pen, p1UI, p2UI);
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        var box = MeasureBoxes[i];
                        if (box.Suppressed || (showLengthOnly && box.Type != LengthType.Length))
                            continue;

                        var targetUI = WorldToDisplay(box.TargetPos, _Zoom, true);
                        var centerUI = new PointF(targetUI.X + (float)box.LocalOffset.X, targetUI.Y + (float)box.LocalOffset.Y*-1f);
                        g.DrawLine(Pens.Black, targetUI, centerUI);
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        var box = MeasureBoxes[i];
                        if (box.Suppressed || (showLengthOnly && box.Type != LengthType.Length))
                            continue;

                        var textBounds = GetMeasureBoxBounds(box);
  
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                        g.FillRectangle(Brushes.White, textBounds);

                        using (var pen = new Pen(GetMeasureTypeColor(box.Type), 1.5f))
                            g.DrawRectangle(pen, textBounds.X, textBounds.Y, textBounds.Width, textBounds.Height);

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far })
                            g.DrawString(box.Value.ToString(box.DisplayUnit, 
                                box.ShowExactValue ? Measure.MeasureFormatFlag.HighPrecision | Measure.MeasureFormatFlag.ForceDecimal : Measure.MeasureFormatFlag.Default), 
                                Font, Brushes.Black, textBounds, sf);
                    }
                }
            }
        }

        private RectangleF GetMeasureBoxBounds(LengthValueBox box)
        {
            var boxCorner = LocalToDisplay(WorldToLocal(box.TargetPos, _Zoom, true) + box.LocalOffset);
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
