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
    public sealed class SvgLayoutExporter
    {

        private double CmToPixel = 35.43307086614173d;// dpi 90
        //dpi 90 = 35.43307086614173 (90 / 2.54)
        //dpi 92 = 36.22047244094488 (92 / 2.54)
        //dpi 96 = 37.79527559055118 (96 / 2.54)

        private SILayout Layout { get; set; }
        private LayoutSvgExportOptions Options { get; set; }
        private PointM OriginOffset;
        private RectangleM LayoutBounds;
        private SvgDocument Document;
        
        private SvgLayoutExporter(SILayout layout, LayoutSvgExportOptions options)
        {
            Layout = layout;
            Options = options;
            CmToPixel = Options.TargetDPI / 2.54d;
            InitializeDocument();
        }

        private void InitializeDocument()
        {
            Document = new SvgDocument();

            if (!string.IsNullOrEmpty(Layout.LayoutName))
                Document.CustomAttributes.Add("LayoutName", Layout.LayoutName);

            LayoutBounds = Layout.GetLayoutBounds();

            Document.X = new SvgUnit(0);
            Document.Y = new SvgUnit(0);

            if (Options.InkscapeCompatible)
            {
                //The Svg library serializes the viewbox with comma and space, but this combination is not handled by the Inkscape DXF exporter
                Document.CustomAttributes.Add("viewBox", string.Format("0,0,{0},{1}", LayoutBounds.Width.NormalizedValue, LayoutBounds.Height.NormalizedValue));
            }
            else
                Document.ViewBox = new SvgViewBox(0, 0, (float)LayoutBounds.Width.NormalizedValue, (float)LayoutBounds.Height.NormalizedValue);

            OriginOffset = new PointM(LayoutBounds.Location.X * -1, LayoutBounds.Location.Y);
        }

        #region SVG Entities

        private SvgLine CreateLine(PointM p1, PointM p2, SvgUnit stroke, Color color)
        {
            return CreateLine(null, p1, p1, stroke, color);
        }

        private SvgLine CreateLine(SvgElement owner, PointM p1, PointM p2, SvgUnit stroke, Color color)
        {
            var svgLine = new SvgLine()
            {
                StartX = GetScaledUnit(p1.X + OriginOffset.X),
                StartY = GetScaledUnit(p1.Y * -1 + OriginOffset.Y),
                EndX = GetScaledUnit(p2.X + OriginOffset.X),
                EndY = GetScaledUnit(p2.Y * -1 + OriginOffset.Y),
                StrokeWidth = stroke,
                Stroke = new SvgColourServer(color)
            };
            if (owner != null)
                owner.Children.Add(svgLine);
            return svgLine;
        }

        private SvgLine CreateLine(LayoutLine line, SvgUnit stroke, Color color)
        {
            return CreateLine(line.P1, line.P2, stroke, color);
        }

        private SvgLine CreateLine(SvgElement owner, LayoutLine line, SvgUnit stroke, Color color)
        {
            return CreateLine(owner, line.P1, line.P2, stroke, color);
        }

        private SvgGroup CreateLayer(string id)
        {
            return CreateLayer(id, id);
        }

        private SvgGroup CreateLayer(string id, string name)
        {
            var layerGroup = new SvgGroup() { ID = id };

            if (Options.InkscapeCompatible)
            {
                layerGroup.CustomAttributes.Add("inkscape:label", name);
                layerGroup.CustomAttributes.Add("inkscape:groupmode", "layer");
            }

            return layerGroup;
        }

        private SvgUnit GetScaledUnit(double value, SvgUnitType type)
        {
            if (type == SvgUnitType.Pixel)
                return new SvgUnit((float)(value / CmToPixel));
            else if (type == SvgUnitType.Point)
                return new SvgUnit((float)(value / (CmToPixel / 1.25d)));
            return new SvgUnit(type, (float)value);
        }

        private SvgUnit GetScaledUnit(Measure value)
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

        #endregion

        private void Generate()
        {
            var guideLinesGroup = CreateLayer("Layout");

            var dashPattern = new SvgUnitCollection();
            dashPattern.Add(GetScaledUnit(6, SvgUnitType.Point));
            dashPattern.Add(GetScaledUnit(4, SvgUnitType.Point));
            dashPattern.Add(GetScaledUnit(2, SvgUnitType.Point));
            dashPattern.Add(GetScaledUnit(4, SvgUnitType.Point));

            if (Options.ExportCenterLine)
            {
                bool centerExist = false;

                if (Options.ExportStringCenters && Layout.NumberOfStrings % 2 == 0)//even number of strings
                {
                    var stringCenter = Layout.GetStringsCenter(Layout.Strings[(Layout.NumberOfStrings / 2) - 1], Layout.Strings[Layout.NumberOfStrings / 2]);
                    if (stringCenter.Equation.IsVertical && stringCenter.Equation.X == 0)
                        centerExist = true;
                }
                else if (Options.ExportStrings && Layout.NumberOfStrings % 2 == 1)//odd number of strings
                {
                    var middleString = Layout.Strings[(Layout.NumberOfStrings - 1) / 2];
                    if (middleString.LayoutLine.Equation.IsVertical && middleString.LayoutLine.Equation.X == 0)
                        centerExist = true;
                }

                if (!centerExist)
                {
                    CreateLine(guideLinesGroup,
                        new PointM(Measure.Zero, LayoutBounds.Top),
                        new PointM(Measure.Zero, LayoutBounds.Bottom), GetScaledUnit(1, SvgUnitType.Point), Color.Black);
                }
            }

            if (Options.ExportStringCenters)
            {
                foreach (var stringCenter in Layout.VisualElements.OfType<StringCenter>())
                {
                    var line = CreateLine(guideLinesGroup, stringCenter, GetScaledUnit(1, SvgUnitType.Point), Color.LightGray);
                    line.StrokeDashArray = dashPattern;
                }
            }

            foreach (var guideLine in Layout.VisualElements.OfType<LayoutLine>().Where(l => l.ElementType == VisualElementType.GuideLine))
            {
                var line = CreateLine(guideLinesGroup, guideLine, GetScaledUnit(1, SvgUnitType.Point), Color.LightGray);
                line.StrokeDashArray = dashPattern;
            }

            if (guideLinesGroup.Children.Count > 0)
                Document.Children.Add(guideLinesGroup);

            //Fingerboard
            var fingerboardGroup = CreateLayer("Fingerboard");
            Document.Children.Add(fingerboardGroup);

            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardEdge>())
                CreateLine(fingerboardGroup, fingerboardEdge.P1, fingerboardEdge.P2, GetScaledUnit(1, SvgUnitType.Point), Color.Blue);

            //Frets

            var fretsGroup = CreateLayer("Frets");
            Document.Children.Add(fretsGroup);
            var fretStroke = GetScaledUnit(1, SvgUnitType.Point);
            if (!Options.FretLineThickness.IsEmpty)
                fretStroke = GetScaledUnit(Options.FretLineThickness);

            foreach (var fretLine in Layout.VisualElements.OfType<FretLine>())
            {
                if (fretLine.IsStraight)
                {
                    var layoutLine = new LayoutLine(fretLine.Points.First(), fretLine.Points.Last());
                    if (!Options.ExtendFretSlots.IsEmpty && Options.ExtendFretSlots != Measure.Zero)
                    {
                        layoutLine.P2 = layoutLine.P2 + (layoutLine.Direction * Options.ExtendFretSlots);
                        layoutLine.P1 = layoutLine.P1 + (layoutLine.Direction * (Options.ExtendFretSlots * -1));
                    }
                    CreateLine(fretsGroup, layoutLine, fretStroke, Color.Red);
                }
                else
                {
                    var fretPath = new SvgPath() { StrokeWidth = fretStroke, Stroke = new SvgColourServer(Color.Red), Fill = SvgPaintServer.None };
                    var fretPoints = fretLine.Points.ToList();
                    if (!Options.ExtendFretSlots.IsEmpty && Options.ExtendFretSlots != Measure.Zero && fretPoints.Count >= 2)
                    {
                        var trebleLine = new LayoutLine(fretLine.Points[1], fretLine.Points[0]);
                        fretPoints[0] = fretPoints[0] + (trebleLine.Direction * Options.ExtendFretSlots);
                        var bassLine = new LayoutLine(fretLine.Points[fretLine.Points.Count - 2], fretLine.Points[fretLine.Points.Count - 1]);
                        fretPoints[fretLine.Points.Count - 1] = fretPoints[fretLine.Points.Count - 1] + (bassLine.Direction * Options.ExtendFretSlots);
                    }
                    fretPath.PathData.Add(new SvgPolylineSegment(fretPoints.Select(pt => new PointM(pt.X, pt.Y * -1) + OriginOffset)));
                    fretsGroup.Children.Add(fretPath);
                }
            }

            //Strings
            if (Options.ExportStrings)
            {
                var stringsGroup = CreateLayer("Strings");
                Document.Children.Add(stringsGroup);

                foreach (var stringLine in Layout.VisualElements.OfType<StringLine>().OrderBy(sl => sl.Index))
                {
                    var svgLine = CreateLine(stringsGroup, stringLine,
                        Options.UseStringGauge && !stringLine.String.Gauge.IsEmpty ?
                        GetScaledUnit(stringLine.String.Gauge) : GetScaledUnit(1, SvgUnitType.Point),
                        Color.Black);
                    svgLine.CustomAttributes.Add("Index", stringLine.Index.ToString());
                }
            }
            else
            {
                //export first & last string to show fingerboard margin
                var firstString = Layout.FirstString.LayoutLine;
                var lastString = Layout.LastString.LayoutLine;
                var trebleEdge = Layout.GetStringBoundaryLine(Layout.FirstString, FingerboardSide.Treble);
                var bassEdge = Layout.GetStringBoundaryLine(Layout.LastString, FingerboardSide.Bass);

                CreateLine(fingerboardGroup, firstString.P1, firstString.SnapToLine(trebleEdge.P2, true), GetScaledUnit(1, SvgUnitType.Point), Color.Gray);
                if (firstString != lastString)
                    CreateLine(fingerboardGroup, lastString.P1, lastString.SnapToLine(bassEdge.P2, true), GetScaledUnit(1, SvgUnitType.Point), Color.Gray);
            }
        }

        public static void ExportLayout(string filename, SILayout layout, LayoutSvgExportOptions options)
        {
            var exporter = new SvgLayoutExporter(layout, options);
            exporter.Generate();
            exporter.Document.Write(filename);
        }
    }
}
