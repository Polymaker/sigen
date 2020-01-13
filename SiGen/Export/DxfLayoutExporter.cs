using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{
    public class DxfLayoutExporter : LayoutExporterBase
    {

        //private PointM OriginOffset;
        //private RectangleM LayoutBounds;
        private DxfDocument Document;
        private Dictionary<string, Layer> Layers;

        private Dictionary<string, AciColor> ColorDictionary;
        

        private DxfLayoutExporter(SILayout layout, LayoutExportOptions options)
            : base(layout, options)
        {
            Layers = new Dictionary<string, Layer>();
            ColorDictionary = new Dictionary<string, AciColor>();
        }

        protected override void InitializeDocument()
        {
            Document = new DxfDocument();

            if (Options.ExportUnit == UnitOfMeasure.Millimeters)
            {
                Document.DrawingVariables.Measurement = 1;
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Millimeters;
            }
            else if (Options.ExportUnit == UnitOfMeasure.Centimeters)
            {
                Document.DrawingVariables.Measurement = 1;
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Centimeters;
            }
            else if (Options.ExportUnit == UnitOfMeasure.Inches)
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Inches;
            else if (Options.ExportUnit == UnitOfMeasure.Feets)
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Feet;

            //var rootLayer = Document.Layers.First();
            //rootLayer.Name = "Layout";
            //Layers.Add(rootLayer.Name, rootLayer);
        }


        protected override void AddLayoutLine(LayoutLine line, VisualElementType elementType, LayoutLineExportConfig lineConfig)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    AddDxfLine("Frets", line, lineConfig);
                    break;
                case VisualElementType.String:
                    AddDxfLine("Strings", line, lineConfig);
                    break;
                case VisualElementType.FingerboardEdge:
                case VisualElementType.FingerboardMargin:
                case VisualElementType.FingerboardContinuation:
                    AddDxfLine("Fingerboard", line, lineConfig);
                    break;
                case VisualElementType.CenterLine:
                case VisualElementType.GuideLine:
                case VisualElementType.StringCenter:
                    AddDxfLine("Layout", line, lineConfig);
                    break;
            }
        }

        protected override void AddLayoutSpline(LayoutPolyLine line, VisualElementType elementType, LayoutLineExportConfig lineConfig)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    AddDxfSpline("Frets", line, lineConfig);
                    break;
                case VisualElementType.FingerboardEdge:
                case VisualElementType.FingerboardMargin:
                case VisualElementType.FingerboardContinuation:
                    AddDxfSpline("Fingerboard", line, lineConfig);
                    break;
                //case VisualElementType.GuideLine:
                default:
                    AddDxfSpline("Layout", line, lineConfig);
                    break;
            }
        }

        protected override void AddLayoutPolyLine(LayoutPolyLine line, VisualElementType elementType, LayoutLineExportConfig lineConfig)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    AddDxfPolyLine("Frets", line, lineConfig);
                    break;
                case VisualElementType.FingerboardEdge:
                case VisualElementType.FingerboardMargin:
                case VisualElementType.FingerboardContinuation:
                    AddDxfPolyLine("Fingerboard", line, lineConfig);
                    break;
                //case VisualElementType.GuideLine:
                default:
                    AddDxfPolyLine("Layout", line, lineConfig);
                    break;
            }
        }

        private Line AddDxfLine(string layerName, LayoutLine line, LayoutLineExportConfig lineConfig)
        {
            var dxfLine = new Line(PointToVector(line.P1), PointToVector(line.P2))
            {
                Layer = GetLayer(layerName),
                Color = GetColor(lineConfig.Color)
            };
            if (lineConfig.IsDashed)
                dxfLine.Linetype = Linetype.Dashed;
            Document.AddEntity(dxfLine);
            return dxfLine;
        }

        private Spline AddDxfSpline(string layerName, LayoutPolyLine line, LayoutLineExportConfig lineConfig)
        {
            var splinePoints = new List<Vector2>();
            foreach (var pt in line.Points)
                splinePoints.Add(PointToVector(pt));

            var splineLine = new Spline(splinePoints.Select(x => new SplineVertex(x)).ToList())
            {
                Layer = GetLayer(layerName),
                Color = GetColor(lineConfig.Color)
            };

            if (lineConfig.IsDashed)
                splineLine.Linetype = Linetype.Dashed;

            Document.AddEntity(splineLine);
            return splineLine;
        }

        private LwPolyline AddDxfPolyLine(string layerName, LayoutPolyLine line, LayoutLineExportConfig lineConfig)
        {
            var splinePoints = new List<Vector2>();
            foreach (var pt in line.Points)
                splinePoints.Add(PointToVector(pt));

            var polyLine = new LwPolyline(splinePoints)
            {
                Layer = GetLayer(layerName),
                Color = GetColor(lineConfig.Color),
            };
            //foreach (var vec in splinePoints)
            //    polyLine.Vertexes.Add(new LwPolylineVertex(vec));

            if (lineConfig.IsDashed)
                polyLine.Linetype = Linetype.Dashed;

            Document.AddEntity(polyLine);
            return polyLine;
        }

        private Layer GetLayer(string name)
        {
            if (!Layers.ContainsKey(name))
                Layers.Add(name, new Layer(name));

            return Layers[name];
        }

        private AciColor GetColor(Color color)
        {
            string argb = $"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            if (!ColorDictionary.ContainsKey(argb))
                ColorDictionary.Add(argb, new AciColor(color));
            return ColorDictionary[argb];
        }

        private Vector2 PointToVector(PointM point)
        {
            return new Vector2((double)point.X[Options.ExportUnit], (double)point.Y[Options.ExportUnit]);
        }

        public static void ExportLayout(string filename, SILayout layout, LayoutExportOptions options)
        {
            var exporter = new DxfLayoutExporter(layout, options);
            exporter.GenerateDocument();
            exporter.Document.Save(filename);
        }
    }
}
