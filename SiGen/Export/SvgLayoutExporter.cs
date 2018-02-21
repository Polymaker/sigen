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

            if (options.ExportCenterLine)
            {
                bool centerExist = false;

                if(options.ExportStringCenters && layout.NumberOfStrings % 2 == 0)//even number of strings
                {
                    var stringCenter = layout.GetStringsCenter(layout.Strings[(layout.NumberOfStrings / 2) - 1], layout.Strings[layout.NumberOfStrings / 2]);
                    if (stringCenter.Equation.IsVertical && stringCenter.Equation.X == 0)
                        centerExist = true;
                }
                else if(options.ExportStrings && layout.NumberOfStrings % 2 == 1)//odd number of strings
                {
                    var middleString = layout.Strings[(layout.NumberOfStrings - 1) / 2];
                    if (middleString.LayoutLine.Equation.IsVertical && middleString.LayoutLine.Equation.X == 0)
                        centerExist = true;
                }

                if (!centerExist)
                {
                    CreateLine(guideLinesGroup, 
                        new PointM(Measure.Zero, layoutBounds.Top), 
                        new PointM(Measure.Zero, layoutBounds.Bottom), centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Black);
                }
            }

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
            var fretStroke = GetScaledUnit(1, SvgUnitType.Point);
            if (!options.FretLineThickness.IsEmpty)
                fretStroke = GetScaledUnit(options.FretLineThickness);

            foreach (var fretLine in layout.VisualElements.OfType<FretLine>())
            {
                if (fretLine.IsStraight)
                {
                    var layoutLine = new LayoutLine(fretLine.Points.First(), fretLine.Points.Last());
                    if(!options.ExtendFretSlots.IsEmpty && options.ExtendFretSlots != Measure.Zero)
                    {
                        layoutLine.P2 = layoutLine.P2 + (layoutLine.Direction * options.ExtendFretSlots);
                        layoutLine.P1 = layoutLine.P1 + (layoutLine.Direction * (options.ExtendFretSlots * -1));
                    }
                    CreateLine(fretsGroup, layoutLine, centerOffset, fretStroke, Color.Red);
                }
                else
                {
                    var fretPath = new SvgPath() { StrokeWidth = fretStroke, Stroke = new SvgColourServer(Color.Red), Fill = SvgPaintServer.None };
                    var fretPoints = fretLine.Points.ToList();
                    if (!options.ExtendFretSlots.IsEmpty && options.ExtendFretSlots != Measure.Zero && fretPoints.Count >= 2)
                    {
                        var trebleLine = new LayoutLine(fretLine.Points[1], fretLine.Points[0]);
                        fretPoints[0] = fretPoints[0] + (trebleLine.Direction * options.ExtendFretSlots);
                        var bassLine = new LayoutLine(fretLine.Points[fretLine.Points.Count - 2], fretLine.Points[fretLine.Points.Count - 1]);
                        fretPoints[fretLine.Points.Count - 1] = fretPoints[fretLine.Points.Count - 1] + (bassLine.Direction * options.ExtendFretSlots);
                    }
                    fretPath.PathData.Add(new SvgPolylineSegment(fretPoints.Select(pt => new PointM(pt.X, pt.Y*-1) + centerOffset)));
                    fretsGroup.Children.Add(fretPath);
                }
            }

            //Strings

            var stringsGroup = new SvgGroup() { ID = "Strings" };
            svgDoc.Children.Add(stringsGroup);

            if (options.ExportStrings)
            {
                foreach(var stringLine in layout.VisualElements.OfType<StringLine>().OrderBy(sl=>sl.Index))
                {
                    var svgLine = CreateLine(stringsGroup, stringLine, centerOffset, 
                        options.UseStringGauge && !stringLine.String.Gauge.IsEmpty ? 
                        GetScaledUnit(stringLine.String.Gauge) : GetScaledUnit(1, SvgUnitType.Point), 
                        Color.Black);
                    svgLine.CustomAttributes.Add("Index", stringLine.Index.ToString());
                }
            }
            else
            {
                //export first & last string to show fingerboard margin
                var firstString = layout.FirstString.LayoutLine;
                var lastString = layout.LastString.LayoutLine;
                var trebleEdge = layout.GetStringBoundaryLine(layout.FirstString, FingerboardSide.Treble);
                var bassEdge = layout.GetStringBoundaryLine(layout.LastString, FingerboardSide.Bass);

                CreateLine(fingerboardGroup, firstString.P1, firstString.SnapToLine(trebleEdge.P2, true), centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Gray);
                if(firstString != lastString)
                    CreateLine(fingerboardGroup, lastString.P1, lastString.SnapToLine(bassEdge.P2, true), centerOffset, GetScaledUnit(1, SvgUnitType.Point), Color.Gray);
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
