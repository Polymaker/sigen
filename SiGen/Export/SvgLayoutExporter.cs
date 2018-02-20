using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using Svg;
using Svg.Pathing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{
    public static class SvgLayoutExporter
    {

        public static void ExportLayout(string filename, SILayout layout, LayoutExportOptions options)
        {
            var svgDoc = new SvgDocument();
            var layoutBounds = layout.GetBounds();
            svgDoc.ViewBox = new SvgViewBox(0, 0, (float)layoutBounds.Width.NormalizedValue, (float)layoutBounds.Height.NormalizedValue);

            svgDoc.Width = new SvgUnit(SvgUnitType.Centimeter, svgDoc.ViewBox.Width);
            svgDoc.Height = new SvgUnit(SvgUnitType.Centimeter, svgDoc.ViewBox.Height);
            var ppi = svgDoc.Ppi;
            var centerOffset = new PointM(layoutBounds.Location.X * -1, layoutBounds.Location.Y);


            var guideLinesGroup = new SvgGroup() { ID = "Layout" };
            svgDoc.Children.Add(guideLinesGroup);

            var dashPattern = new SvgUnitCollection();
            dashPattern.Add(GetScaledUnit(6, SvgUnitType.Point));
            dashPattern.Add(GetScaledUnit(4, SvgUnitType.Point));
            dashPattern.Add(GetScaledUnit(2, SvgUnitType.Point));
            dashPattern.Add(GetScaledUnit(4, SvgUnitType.Point));

            if (options.ExportStringCenters)
            {
                foreach (var stringCenter in layout.VisualElements.OfType<StringCenter>())
                {
                    var line = CreateLine(guideLinesGroup, stringCenter, centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.LightGray);
                    line.StrokeDashArray = dashPattern;
                }
            }

            //Fingerboard

            var fingerboardGroup = new SvgGroup() { ID = "Fingerboard" };
            svgDoc.Children.Add(fingerboardGroup);

            foreach(var fingerboardEdge in layout.VisualElements.OfType<FingerboardEdge>())
                CreateLine(fingerboardGroup, fingerboardEdge.P1, fingerboardEdge.P2, centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Blue);

            //Frets

            var fretsGroup = new SvgGroup() { ID = "Frets" };
            svgDoc.Children.Add(fretsGroup);
            
            foreach (var fretLine in layout.VisualElements.OfType<FretLine>())
            {
                if (fretLine.IsStraight)
                {
                    CreateLine(fretsGroup, fretLine.Points.First(), fretLine.Points.Last(), centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Red);
                }
                else
                {
                    var fretPath = new SvgPath() { StrokeWidth = GetScaledUnit(new SvgUnit(SvgUnitType.Point, 1)), Stroke = new SvgColourServer(Color.Red), Fill = SvgPaintServer.None };
                    fretPath.PathData.Add(new SvgPolylineSegment(fretLine.Points.Select(pt => new PointM(pt.X, pt.Y*-1) + centerOffset)));
                    fretsGroup.Children.Add(fretPath);
                }
            }

            //Strings

            var stringsGroup = new SvgGroup() { ID = "Strings" };
            svgDoc.Children.Add(stringsGroup);

            if (options.ExportStrings)
            {
                foreach(var stringLine in layout.VisualElements.OfType<StringLine>())
                {
                    CreateLine(stringsGroup, stringLine, centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Black);
                }
            }
            else
            {
                var firstString = layout.Strings.First().LayoutLine;
                var lastString = layout.Strings.Last().LayoutLine;
                CreateLine(fingerboardGroup, firstString, centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Black);
                if(firstString != lastString)
                    CreateLine(fingerboardGroup, lastString, centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Black);
            }

            svgDoc.Write(filename);
        }

        private static SvgLine CreateLine(SvgElement owner, PointM p1, PointM p2, PointM offset, SvgUnit stroke, Color color)
        {
            var svgLine = new SvgLine()
            {
                StartX = new SvgUnit((float)(p1.X + offset.X).NormalizedValue),
                StartY = new SvgUnit((float)(p1.Y * -1 + offset.Y).NormalizedValue),
                EndX = new SvgUnit((float)(p2.X + offset.X).NormalizedValue),
                EndY = new SvgUnit((float)(p2.Y * -1 + offset.Y).NormalizedValue),
                StrokeWidth = stroke,
                Stroke = new SvgColourServer(color)
            };

            owner.Children.Add(svgLine);
            return svgLine;
        }

        private static SvgLine CreateLine(SvgElement owner, LayoutLine line, PointM offset, SvgUnit stroke, Color color)
        {
            return CreateLine(owner, line.P1, line.P2, offset, stroke, color);
        }

        private static SvgUnit GetScaledUnit(SvgUnit value)
        {
            if (value.Type == SvgUnitType.Pixel)
                return new SvgUnit(value.Value / 35.43307365614753f);
            else if (value.Type == SvgUnitType.Point)
                return new SvgUnit(value.Value / 28.34645490730993f);
            return value;
        }

        private static SvgUnit GetScaledUnit(double value, SvgUnitType type)
        {
            if (type == SvgUnitType.Pixel)
                return new SvgUnit((float)value / 35.43307365614753f);
            else if (type == SvgUnitType.Point)
                return new SvgUnit((float)value / 28.34645490730993f);
            return new SvgUnit((float)value);
        }

        private static SvgUnit GetScaledUnit(Measure value)
        {
            return new SvgUnit((float)value.NormalizedValue);
        }

        private class SvgPolylineSegment : SvgPathSegment
        {
            private List<PointM> _Points;

            public SvgPolylineSegment(IEnumerable<PointM> points)
            {
                _Points = points.ToList();
                Start = (PointF)_Points.First().ToVector();
                End = (PointF)_Points.Last().ToVector();
            }

            public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
            {
                graphicsPath.AddLines(_Points.Select(p => (PointF)p.ToVector()).ToArray());
            }

            public override string ToString()
            {
                return "M " + _Points.Select(p => string.Format("{0},{1}", p.X.NormalizedValue, p.Y.NormalizedValue)).Aggregate((i, j) => i + " " + j);
            }
        }

    }
}
