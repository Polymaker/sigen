﻿using SiGen.Maths;
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
    public sealed class SvgLayoutExporter : LayoutExporterBase
    {
        //dpi 90 = 35.43307086614173 (90 / 2.54)
        //dpi 92 = 36.22047244094488 (92 / 2.54)
        //dpi 96 = 37.79527559055118 (96 / 2.54)
        //private double CmToPixel = 35.43307086614173d;// dpi 90

        private PreciseDouble PixelToCm = 0;
        private PreciseDouble PointToCm = 0;


        private static readonly Vector FlipY = new Vector(1, -1);

        private PointM OriginOffset;
        private Vector OriginOffestVec;
        private RectangleM LayoutBounds;
        private SvgDocument Document;
        private Dictionary<string, SvgGroup> Layers;
        private SvgUnitCollection LineDashPattern;

        private SvgLayoutExporter(SILayout layout, LayoutSvgExportOptions options)
            : base(layout, options)
        {
            Layers = new Dictionary<string, SvgGroup>();
            PixelToCm = (PreciseDouble)2.54d / (PreciseDouble)Options.TargetDPI;
            PointToCm = PixelToCm * 1.25d;
        }

        protected override void InitializeDocument()
        {
            Document = new SvgDocument();

            if (!string.IsNullOrEmpty(Layout.LayoutName))
                Document.CustomAttributes.Add("LayoutName", Layout.LayoutName);

            LayoutBounds = Layout.GetLayoutBounds();

            Document.X = new SvgUnit(0);
            Document.Y = new SvgUnit(0);
            Document.Width = GetDocumentUnit(LayoutBounds.Width);
            Document.Height = GetDocumentUnit(LayoutBounds.Height);

            if (Options.InkscapeCompatible)
            {
                Document.CustomAttributes.Add("xmlns:inkscape", "http://www.inkscape.org/namespaces/inkscape");
                //The Svg library serializes the viewbox with comma and space, but this combination is not handled by the Inkscape DXF exporter
                Document.CustomAttributes.Add("viewBox", 
                    string.Format(NumberFormatInfo.InvariantInfo, "0,0,{0},{1}",
                    LayoutBounds.Width[Options.ExportUnit].DoubleValue, 
                    LayoutBounds.Height[Options.ExportUnit].DoubleValue));
            }
            else
            {
                Document.ViewBox = new SvgViewBox(0, 0, Document.Width, Document.Height);
            }

            OriginOffset = new PointM(LayoutBounds.Location.X * -1, LayoutBounds.Location.Y);
            OriginOffestVec = OriginOffset.ToVector(Options.ExportUnit);

            LineDashPattern = new SvgUnitCollection
            {
                GetRelativeUnit(6, SvgUnitType.Point),
                GetRelativeUnit(4, SvgUnitType.Point),
                GetRelativeUnit(2, SvgUnitType.Point),
                GetRelativeUnit(4, SvgUnitType.Point)
            };
        }

        protected override void FinalizeDocument()
        {
            foreach(var layer in Layers.Values)
            {
                if (layer.HasChildren())
                    Document.Children.Add(layer);
            }
        }

        protected override void AddLayoutLine(LayoutLine line, VisualElementType elementType, VisualElement extraInfo = null)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    {
                        var fretStroke = GetRelativeUnit(1, SvgUnitType.Point);

                        if (!Options.FretLineThickness.IsEmpty)
                            fretStroke = GetRelativeUnit(Options.FretLineThickness);

                        var svgLine = CreateSvgLine(GetLayer("Frets"), line, fretStroke, Options.FretColor);

                        if (extraInfo is FretLine fretLine)
                        {
                            if (fretLine.IsNut)
                                svgLine.CustomAttributes.Add("Fret", "Nut");
                            else
                                svgLine.CustomAttributes.Add("Fret", fretLine.FretIndex.ToString());
                        }
                        break;
                    }

                case VisualElementType.FingerboardEdge:
                    CreateSvgLine(GetLayer("Fingerboard"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.Blue);
                    break;

                case VisualElementType.FingerboardMargin:
                    CreateSvgLine(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.Gray);
                    break;

                case VisualElementType.FingerboardContinuation:
                case VisualElementType.StringCenter:
                case VisualElementType.GuideLine:
                    CreateSvgLine(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray, LineDashPattern);
                    break;

                case VisualElementType.String:
                    {
                        var stringInfo = line as StringLine;
                        var lineThickness = Options.UseStringGauge && !stringInfo.String.Gauge.IsEmpty ?
                            GetRelativeUnit(stringInfo.String.Gauge) : GetRelativeUnit(1, SvgUnitType.Point);

                        var svgLine = CreateSvgLine(GetLayer("Strings"), line, lineThickness, Options.StringColor);
                        svgLine.CustomAttributes.Add("Index", stringInfo.Index.ToString());
                        break;
                    }

                case VisualElementType.CenterLine:
                    CreateSvgLine(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.Black);
                    break;
            }
        }

        protected override void AddLayoutPolyLine(LayoutPolyLine line, VisualElementType elementType, VisualElement extraInfo = null)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    {
                        var fretStroke = GetRelativeUnit(1, SvgUnitType.Point);
                        if (!Options.FretLineThickness.IsEmpty)
                            fretStroke = GetRelativeUnit(Options.FretLineThickness);

                        var svgLine = CreateSvgPolyLine(GetLayer("Frets"), line, fretStroke, Options.FretColor);

                        var fretLine = (line as FretLine) ?? (extraInfo as FretLine);
                        if (fretLine != null)
                        {
                            if (fretLine.IsNut)
                                svgLine.CustomAttributes.Add("Fret", "Nut");
                            else
                                svgLine.CustomAttributes.Add("Fret", fretLine.FretIndex.ToString());
                        }
                        
                        break;
                    }
                case VisualElementType.FingerboardEdge:
                    CreateSvgPolyLine(GetLayer("Fingerboard"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.Blue);
                    break;

                case VisualElementType.GuideLine:
                    CreateSvgPolyLine(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray).StrokeDashArray = LineDashPattern;
                    break;
                    //case VisualElementType.FingerboardMargin:
                    //    CreateSvgPolyLine(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray);
                    //    break;
                    //case VisualElementType.FingerboardContinuation:
                    //    CreateSvgPolyLine(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray).StrokeDashArray = LineDashPattern;
                    //    break;
            }
        }

        protected override void AddLayoutSpline(LayoutPolyLine line, VisualElementType elementType, VisualElement extraInfo = null)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    {
                        var fretStroke = GetRelativeUnit(1, SvgUnitType.Point);

                        if (!Options.FretLineThickness.IsEmpty)
                            fretStroke = GetRelativeUnit(Options.FretLineThickness);

                        var svgSpline = CreateSvgSpline(GetLayer("Frets"), line, fretStroke, Options.FretColor);

                        FretLine fretLine = (line as FretLine) ?? (extraInfo as FretLine);
                        if (fretLine != null)
                        {
                            if (fretLine.IsNut)
                                svgSpline.CustomAttributes.Add("Fret", "Nut");
                            else
                                svgSpline.CustomAttributes.Add("Fret", fretLine.FretIndex.ToString());
                        }
                        break;
                    }
                case VisualElementType.FingerboardEdge:
                    CreateSvgSpline(GetLayer("Fingerboard"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.Blue);
                    break;

                case VisualElementType.GuideLine:
                    CreateSvgSpline(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray).StrokeDashArray = LineDashPattern;
                    break;
                    //case VisualElementType.FingerboardMargin:
                    //    CreateSvgSpline(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray);
                    //    break;
                    //case VisualElementType.FingerboardContinuation:
                    //    CreateSvgSpline(GetLayer("Layout"), line, GetRelativeUnit(1, SvgUnitType.Point), Color.LightGray).StrokeDashArray = LineDashPattern;
                    //    break;
            }
        }

        #region SVG Entities

        private SvgLine CreateSvgLine(SvgElement owner, PointM p1, PointM p2, SvgUnit stroke, Color color, SvgUnitCollection dashPattern)
        {
            var svgLine = new SvgLine()
            {
                StartX = GetRelativeUnit(p1.X + OriginOffset.X),
                StartY = GetRelativeUnit(p1.Y * -1 + OriginOffset.Y),
                EndX = GetRelativeUnit(p2.X + OriginOffset.X),
                EndY = GetRelativeUnit(p2.Y * -1 + OriginOffset.Y),
                StrokeWidth = stroke,
                Stroke = new SvgColourServer(color),
                StrokeDashArray = dashPattern
            };
            if (owner != null)
                owner.Children.Add(svgLine);
            return svgLine;
        }

        private SvgLine CreateSvgLine(SvgElement owner, PointM p1, PointM p2, SvgUnit stroke, Color color)
        {
            return CreateSvgLine(owner, p1, p2, stroke, color, null);
        }

        private SvgLine CreateSvgLine(SvgElement owner, LayoutLine line, SvgUnit stroke, Color color)
        {
            return CreateSvgLine(owner, line.P1, line.P2, stroke, color, null);
        }

        private SvgLine CreateSvgLine(SvgElement owner, LayoutLine line, SvgUnit stroke, Color color, SvgUnitCollection dashPattern)
        {
            return CreateSvgLine(owner, line.P1, line.P2, stroke, color, dashPattern);
        }

        private SvgPath CreateSvgPolyLine(SvgElement owner, LayoutPolyLine line, SvgUnit stroke, Color color)
        {
            var path = new SvgPath() { StrokeWidth = stroke, Stroke = new SvgColourServer(color), Fill = SvgPaintServer.None };

            path.PathData.Add(new SvgMoveToSegment(GetRelativePosition(line.Points[0])));

            for (int i = 0; i < line.Points.Count - 1; i++)
            {
                path.PathData.Add(new SvgLineSegment(
                    GetRelativePosition(line.Points[i]),
                    GetRelativePosition(line.Points[i + 1])
                    )
                );
            }
            if (owner != null)
                owner.Children.Add(path);
            return path;
        }

        private SvgPath CreateSvgSpline(SvgElement owner, LayoutPolyLine line, SvgUnit stroke, Color color)
        {
            var path = new SvgPath() { StrokeWidth = stroke, Stroke = new SvgColourServer(color), Fill = SvgPaintServer.None };

            path.PathData.Add(new SvgMoveToSegment(GetRelativePosition(line.Spline.Curves[0].StartPoint)));

            for (int i = 0; i < line.Spline.Curves.Length; i++)
            {
                var curve = line.Spline.Curves[i];
                path.PathData.Add(new SvgCubicCurveSegment(
                    GetRelativePosition(curve.StartPoint),
                    GetRelativePosition(curve.ControlPoints[0]),
                    GetRelativePosition(curve.ControlPoints[1]),
                    GetRelativePosition(curve.EndPoint)
                    )
                );
            }

            if (owner != null)
                owner.Children.Add(path);

            return path;
        }

        private SvgGroup GetLayer(string id)
        {
            return CreateLayer(id, id);
        }

        private SvgGroup CreateLayer(string id, string name)
        {
            if (!Layers.ContainsKey(id))
            {
                var layerGroup = new SvgGroup() { ID = id };

                if (Options.InkscapeCompatible)
                {
                    layerGroup.CustomAttributes.Add("inkscape:label", name);
                    layerGroup.CustomAttributes.Add("inkscape:groupmode", "layer");
                }

                Layers.Add(id, layerGroup);
            }

            return Layers[id];
        }

        private SvgUnit GetRelativeUnit(double value, SvgUnitType type)
        {
            if (type == SvgUnitType.Pixel)
            {
                var convertedValue = (PreciseDouble)value * PixelToCm;
                convertedValue = UnitOfMeasure.ConvertTo(convertedValue, UnitOfMeasure.Cm, Options.ExportUnit);
                return new SvgUnit((float)convertedValue);
            }
            else if (type == SvgUnitType.Point)
            {
                var convertedValue = (PreciseDouble)value * PointToCm;
                convertedValue = UnitOfMeasure.ConvertTo(convertedValue, UnitOfMeasure.Cm, Options.ExportUnit);
                return new SvgUnit((float)convertedValue);
            }

            return new SvgUnit(type, (float)value);
        }

        private SvgUnit GetRelativeUnit(Measure value)
        {
            return new SvgUnit((float)value[Options.ExportUnit]);
        }

        private SvgUnit GetDocumentUnit(Measure value)
        {
            var svgUnitType = UnitOfMeasureToSvg(Options.ExportUnit);
            var svgValue = value[Options.ExportUnit];

            if (Options.ExportUnit == UnitOfMeasure.Feets)
            {
                svgUnitType = SvgUnitType.Inch;
                svgValue *= 12;
            }

            return new SvgUnit(svgUnitType, (float)svgValue);
        }

        private SvgUnitType UnitOfMeasureToSvg(UnitOfMeasure unit)
        {
            if (unit == UnitOfMeasure.Millimeters)
                return SvgUnitType.Millimeter;
            else if (unit == UnitOfMeasure.Centimeters)
                return SvgUnitType.Centimeter;
            else if (unit == UnitOfMeasure.Inches)
                return SvgUnitType.Inch;
            //else if (unit == UnitOfMeasure.Feets)
            //    return SvgUnitType.f;

            return SvgUnitType.User;
        }

        private PointF GetRelativePosition(PointM pos)
        {
            var resultVec = (pos.ToVector(Options.ExportUnit) * FlipY) + OriginOffestVec;
            return (PointF)resultVec;
        }

        private PointF GetRelativePosition(Vector pos)
        {
            var resultVec = ((pos / Options.ExportUnit.ConversionFactor) * FlipY) + OriginOffestVec;
            return (PointF)resultVec;
        }

        #endregion

        public static void ExportLayout(string filename, SILayout layout, LayoutSvgExportOptions options)
        {
            var exporter = new SvgLayoutExporter(layout, options);
            exporter.GenerateDocument();
            exporter.Document.Write(filename);
        }
    }
}
