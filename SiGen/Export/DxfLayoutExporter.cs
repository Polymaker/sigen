using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
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

        private DxfLayoutExporter(SILayout layout, LayoutSvgExportOptions options)
            : base(layout, options)
        {
            Layers = new Dictionary<string, Layer>();
        }

        protected override void InitializeDocument()
        {
            Document = new DxfDocument();

            if (Options.ExportUnit == UnitOfMeasure.Millimeters)
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Millimeters;
            else if (Options.ExportUnit == UnitOfMeasure.Centimeters)
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Centimeters;
            else if (Options.ExportUnit == UnitOfMeasure.Inches)
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Inches;
            else if (Options.ExportUnit == UnitOfMeasure.Feets)
                Document.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Feet;

            //var rootLayer = Document.Layers.First();
            //rootLayer.Name = "Layout";
            //Layers.Add(rootLayer.Name, rootLayer);
        }


        protected override void AddLayoutLine(LayoutLine line, VisualElementType elementType)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    AddDxfLine("Frets", line, AciColor.Red);
                    break;
                case VisualElementType.FingerboardEdge:
                    AddDxfLine("Fingerboard", line, AciColor.Blue);
                    break;
                case VisualElementType.CenterLine:
                    AddDxfLine("Layout", line, AciColor.FromHsl(0,0,0));
                    break;
                case VisualElementType.GuideLine:
                case VisualElementType.StringCenter:
                    AddDxfLine("Layout", line, AciColor.LightGray).Linetype = Linetype.Dashed;
                    break;
                case VisualElementType.FingerboardMargin:
                    AddDxfLine("Layout", line, AciColor.LightGray);
                    break;
            }
        }

        protected override void AddLayoutSpline(LayoutPolyLine line, VisualElementType elementType)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    AddDxfSpline("Frets", line, AciColor.Red);
                    break;
                case VisualElementType.FingerboardEdge:
                    AddDxfSpline("Fingerboard", line, AciColor.Blue);
                    break;
                case VisualElementType.CenterLine:
                    AddDxfSpline("Layout", line, AciColor.FromHsl(0, 0, 0));
                    break;
                case VisualElementType.GuideLine:
                case VisualElementType.StringCenter:
                    AddDxfSpline("Layout", line, AciColor.LightGray).Linetype = Linetype.Dashed;
                    break;
            }
        }

        protected override void AddLayoutPolyLine(LayoutPolyLine line, VisualElementType elementType)
        {
            switch (elementType)
            {
                case VisualElementType.Fret:
                    AddDxfPolyLine("Frets", line, AciColor.Red);
                    break;
                case VisualElementType.FingerboardEdge:
                    AddDxfPolyLine("Fingerboard", line, AciColor.Blue);
                    break;
                case VisualElementType.CenterLine:
                    AddDxfPolyLine("Layout", line, AciColor.FromHsl(0, 0, 0));
                    break;
                case VisualElementType.GuideLine:
                case VisualElementType.StringCenter:
                    AddDxfPolyLine("Layout", line, AciColor.LightGray).Linetype = Linetype.Dashed;
                    break;
            }
        }

        private Line AddDxfLine(string layerName, LayoutLine line, AciColor color)
        {
            var dxfLine = new Line(PointToVector(line.P1), PointToVector(line.P2))
            {
                Layer = GetLayer(layerName),
                Color = color
            };
            Document.AddEntity(dxfLine);
            return dxfLine;
        }

        private Spline AddDxfSpline(string layerName, LayoutPolyLine line, AciColor color)
        {
            var splinePoints = new List<Vector2>();
            foreach (var pt in line.Points)
                splinePoints.Add(PointToVector(pt));

            var splineLine = new Spline(splinePoints.Select(x => new SplineVertex(x)).ToList())
            {
                Layer = GetLayer(layerName),
                Color = color,
            };

            Document.AddEntity(splineLine);
            return splineLine;
        }

        private LwPolyline AddDxfPolyLine(string layerName, LayoutPolyLine line, AciColor color)
        {
            var splinePoints = new List<Vector2>();
            foreach (var pt in line.Points)
                splinePoints.Add(PointToVector(pt));

            var polyLine = new LwPolyline(splinePoints)
            {
                Layer = GetLayer(layerName),
                Color = color,
            };
            //foreach (var vec in splinePoints)
            //    polyLine.Vertexes.Add(new LwPolylineVertex(vec));
            Document.AddEntity(polyLine);
            return polyLine;
        }

        private Layer GetLayer(string name)
        {
            if (!Layers.ContainsKey(name))
                Layers.Add(name, new Layer(name));

            return Layers[name];
        }

        private Vector2 PointToVector(PointM point)
        {
            return new Vector2((double)point.X[Options.ExportUnit], (double)point.Y[Options.ExportUnit]);
        }

        public static void ExportLayout(string filename, SILayout layout, LayoutSvgExportOptions options)
        {
            var exporter = new DxfLayoutExporter(layout, options);
            exporter.GenerateDocument();
            exporter.Document.Save(filename);
        }
    }
}
