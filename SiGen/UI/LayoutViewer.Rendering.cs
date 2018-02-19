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
    }
}
