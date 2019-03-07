using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using Svg;
using Svg.Pathing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        private static readonly Vector FlipY = new Vector(1, -1);

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
            Document.Width = new SvgUnit(SvgUnitType.Centimeter, (float)LayoutBounds.Width.NormalizedValue);
            Document.Height = new SvgUnit(SvgUnitType.Centimeter, (float)LayoutBounds.Height.NormalizedValue);

            if (Options.InkscapeCompatible)
            {
                Document.CustomAttributes.Add("xmlns:inkscape", "http://www.inkscape.org/namespaces/inkscape");
                //The Svg library serializes the viewbox with comma and space, but this combination is not handled by the Inkscape DXF exporter
                Document.CustomAttributes.Add("viewBox", string.Format(NumberFormatInfo.InvariantInfo, "0,0,{0},{1}", LayoutBounds.Width.NormalizedValue, LayoutBounds.Height.NormalizedValue));
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

        private SvgPath CreatePath(SvgElement owner, LayoutPolyLine line, SvgUnit stroke, Color color)
        {
            var path = new SvgPath() { StrokeWidth = stroke, Stroke = new SvgColourServer(color), Fill = SvgPaintServer.None };
            if(line.Spline == null)
            {
                path.PathData.Add(new SvgMoveToSegment(WorldToDisplay(line.Points[0])));
                for(int i = 0; i < line.Points.Count - 1; i++)
                {
                    path.PathData.Add(new SvgLineSegment(
                        WorldToDisplay(line.Points[i]), 
                        WorldToDisplay(line.Points[i + 1])));
                }
            }
            else
            {
                var offsetVec = OriginOffset.ToVector();
                path.PathData.Add(new SvgMoveToSegment((PointF)(offsetVec + line.Spline.Curves[0].StartPoint * FlipY)));

                for (int i = 0; i < line.Spline.Curves.Length; i ++)
                {
                    var curve = line.Spline.Curves[i];
                    //path.PathData.Add(new SvgMoveToSegment((PointF)(offsetVec + curve.StartPoint * FlipY)));
                    path.PathData.Add(new SvgCubicCurveSegment(
                        (PointF)(offsetVec + curve.StartPoint * FlipY),
                        (PointF)(offsetVec + curve.ControlPoints[0] * FlipY),
                        (PointF)(offsetVec + curve.ControlPoints[1] * FlipY),
                        (PointF)(offsetVec + curve.EndPoint * FlipY)
                        ));
                }
            }
            if (owner != null)
                owner.Children.Add(path);
            return path;
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

        private PointF WorldToDisplay(PointM pos)
        {
            return (PointF)((pos.ToVector() * FlipY) + OriginOffset.ToVector());
        }

        #endregion

        private void GenerateDocument()
        {
            GenerateLayoutElements();

            GenerateFingerboardElements();

            GenerateFretElements();

            GenerateStringElements();
        }

        private void GenerateLayoutElements()
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
        }

        private void GenerateFingerboardElements()
        {
            //Fingerboard
            var fingerboardGroup = CreateLayer("Fingerboard");
            Document.Children.Add(fingerboardGroup);

            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardSideEdge>())
                CreateLine(fingerboardGroup, fingerboardEdge.P1, fingerboardEdge.P2, GetScaledUnit(1, SvgUnitType.Point), Color.Blue);

            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardEdge>())
                CreatePath(fingerboardGroup, fingerboardEdge, GetScaledUnit(1, SvgUnitType.Point), Color.Blue);

            if (!Options.ExportStrings && Options.ExportFingerboardMargin)//export first & last string to show fingerboard margin
            {
                var firstString = Layout.FirstString.LayoutLine;
                var lastString = Layout.LastString.LayoutLine;
                var trebleEdge = Layout.GetStringBoundaryLine(Layout.FirstString, FingerboardSide.Treble);
                var bassEdge = Layout.GetStringBoundaryLine(Layout.LastString, FingerboardSide.Bass);

                CreateLine(fingerboardGroup, firstString.P1, firstString.SnapToLine(trebleEdge.P2, true), GetScaledUnit(1, SvgUnitType.Point), Color.Gray);
                if (firstString != lastString)
                    CreateLine(fingerboardGroup, lastString.P1, lastString.SnapToLine(bassEdge.P2, true), GetScaledUnit(1, SvgUnitType.Point), Color.Gray);
            }
        }

        private void GenerateFretElements()
        {
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
                    if (Options.ExtendFretSlots && fretLine.Length > Options.FretSlotsExtensionAmount * 2)
                    {
                        layoutLine.P2 = layoutLine.P2 + (layoutLine.Direction * Options.FretSlotsExtensionAmount);
                        layoutLine.P1 = layoutLine.P1 + (layoutLine.Direction * (Options.FretSlotsExtensionAmount * -1));
                    }
                    var svgLine = CreateLine(fretsGroup, layoutLine, fretStroke, Options.FretColor);
					if (fretLine.IsNut)
						svgLine.CustomAttributes.Add("Fret", "Nut");
					else
						svgLine.CustomAttributes.Add("Fret", fretLine.FretIndex.ToString());

				}
                else
                {
                    fretLine.RebuildSpline();
                    SvgPath fretPath = null;

                    if (Options.ExtendFretSlots && fretLine.Length > Options.FretSlotsExtensionAmount * 2)
                    {
                        var tmpLine = new LayoutPolyLine(fretLine.Points);
                        var bounds = fretLine.GetFretBoundaries(false);
                        var offset1 = LayoutLine.Offset(bounds.Item1, Options.FretSlotsExtensionAmount * -1);
                        var offset2 = LayoutLine.Offset(bounds.Item2, Options.FretSlotsExtensionAmount);
                        tmpLine.TrimBetween(offset1, offset2, true);
                        if (fretLine.Spline != null)
                            tmpLine.InterpolateSpline(0.5);
                        fretPath = CreatePath(fretsGroup, tmpLine, fretStroke, Options.FretColor);
                    }
                    else
                    {
                        fretPath = CreatePath(fretsGroup, fretLine, fretStroke, Options.FretColor);
                    }

                    fretPath.CustomAttributes.Add("Index", fretLine.FretIndex.ToString());
                }
            }
        }

        private void GenerateStringElements()
        {
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
                        Options.StringColor);
                    svgLine.CustomAttributes.Add("Index", stringLine.Index.ToString());
                }
            }
        }

        public static void ExportLayout(string filename, SILayout layout, LayoutSvgExportOptions options)
        {
            var exporter = new SvgLayoutExporter(layout, options);
            exporter.GenerateDocument();
            exporter.Document.Write(filename);
        }
    }
}
