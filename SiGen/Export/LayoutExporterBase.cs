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
    public abstract class LayoutExporterBase
    {
        public SILayout Layout { get; protected set; }

        public LayoutExportConfig Options { get; protected set; }

        protected LayoutExporterBase(SILayout layout, LayoutExportConfig options)
        {
            Layout = layout;
            Options = options;
        }

        protected virtual void InitializeDocument()
        {

        }

        protected virtual void FinalizeDocument()
        {

        }

        public void GenerateDocument()
        {
            InitializeDocument();

            if (Options.ExportFingerboardEdges)
                GenerateFingerboard();

            GenerateLayoutLines();

            if (Options.ExportFrets)
                GenerateFrets();

            if (Options.ExportStrings)
                GenerateStrings();

            FinalizeDocument();
        }

        private void GenerateFrets()
        {
            foreach (var fretLine in Layout.VisualElements.OfType<FretLine>())
            {
                if (Options.ExtendFretSlots)
                {
                    var extendedFretLine = fretLine.GetExtendedFretLine(Options.Frets.ExtensionAmount);
                    if (extendedFretLine is LayoutLine line)
                    {
                        line.Tag = fretLine;
                        AddLayoutLine(line, VisualElementType.Fret, Options.Frets);
                        continue;
                    }
                    else if (extendedFretLine is LayoutPolyLine polyline)
                    {
                        polyline.Tag = fretLine;
                        if (polyline.Spline != null)
                            AddLayoutSpline(polyline, VisualElementType.Fret, Options.Frets);
                        else
                            AddLayoutPolyLine(polyline, VisualElementType.Fret, Options.Frets);
                        continue;
                    }
                }

                if (fretLine.IsStraight)
                {
                    var layoutLine = new LayoutLine(fretLine.Points.First(), fretLine.Points.Last());
                    layoutLine.Tag = fretLine;
                    AddLayoutLine(layoutLine, VisualElementType.Fret, Options.Frets);
                }
                else
                {
                    fretLine.RebuildSpline();

                    if (fretLine.Spline != null)
                        AddLayoutSpline(fretLine, VisualElementType.Fret, Options.Frets);
                    else
                        AddLayoutPolyLine(fretLine, VisualElementType.Fret, Options.Frets);
                }
            }

            if (Options.ExportBridgeLine)
            {
                var bridgeLine = Layout.GetElement<LayoutPolyLine>(x => x.ElementType == VisualElementType.BridgeLine);
                if (bridgeLine != null)
                {
                    var tmpConfig = new LineExportConfig()
                    {
                        Color = System.Drawing.Color.Black,
                        LineThickness = 1d,
                        LineUnit = LineUnit.Points
                    };
                    AddLayoutPolyLine(bridgeLine, VisualElementType.BridgeLine, tmpConfig);
                }
            }
        }

        private void GenerateFingerboard()
        {
            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardSideEdge>())
                AddLayoutLine(fingerboardEdge, VisualElementType.FingerboardEdge, Options.FingerboardEdges);

            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardEdge>())
            {
                if (fingerboardEdge.Points.Count == 2)
                {
                    var layoutLine = new LayoutLine(fingerboardEdge.Points.First(), fingerboardEdge.Points.Last());
                    AddLayoutLine(layoutLine, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
                }
                else if (fingerboardEdge.Spline != null)
                    AddLayoutSpline(fingerboardEdge, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
                else
                    AddLayoutPolyLine(fingerboardEdge, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
            }

            if (!Options.ExportFrets)
            {
                var nutLines = Layout.GetElements<FretLine>(x => x.IsNut);

                foreach (var line in nutLines)
                {
                    if (line.IsStraight)
                    {
                        var layoutLine = new LayoutLine(line.Points.First(), line.Points.Last());
                        AddLayoutLine(layoutLine, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
                    }
                    else if (line.Spline != null)
                        AddLayoutSpline(line, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
                    else
                        AddLayoutPolyLine(line, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
                }
            }
        }

        private void GenerateLayoutLines()
        {
            if (Options.ExportCenterLine)
            {
                bool centerExist = false;
                var centerLine = Layout.VisualElements.OfType<LayoutLine>().First(x => x.ElementType == VisualElementType.CenterLine);

                if (Options.ExportMidlines && Layout.NumberOfStrings % 2 == 0)//even number of strings
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
                    AddLayoutLine(centerLine, VisualElementType.CenterLine, Options.CenterLine);
            }

            if (Options.ExportMidlines)
            {
                foreach (var stringCenter in Layout.GetElements<StringCenter>())
                    AddLayoutLine(stringCenter, VisualElementType.StringMidline, Options.Midlines);
            }

            if (!Options.ExportStrings && Options.ExportFingerboardMargins)//export first & last string to show fingerboard margin
            {
                var margins = Layout.GetElements<LayoutLine>(x => x.ElementType == VisualElementType.FingerboardMargin);

                if (margins.Any())
                {
                    foreach(var marginLine in margins)
                        AddLayoutLine(marginLine, VisualElementType.FingerboardMargin, Options.FingerboardMargins);
                }
                //else
                //{
                //    var firstString = Layout.FirstString.LayoutLine;
                //    var lastString = Layout.LastString.LayoutLine;
                //    var trebleEdge = Layout.GetStringBoundaryLine(Layout.FirstString, FingerboardSide.Treble);
                //    var bassEdge = Layout.GetStringBoundaryLine(Layout.LastString, FingerboardSide.Bass);

                //    var trebleMargin = new LayoutLine(firstString.P1, firstString.SnapToLine(trebleEdge.P2, LineSnapDirection.Horizontal));
                //    AddLayoutLine(trebleMargin, VisualElementType.FingerboardMargin, Options.FingerboardMargins);

                //    if (firstString != lastString)
                //    {
                //        var bassMargin = new LayoutLine(lastString.P1, lastString.SnapToLine(bassEdge.P2, LineSnapDirection.Horizontal));
                //        AddLayoutLine(bassMargin, VisualElementType.FingerboardMargin, Options.FingerboardMargins);
                //    }
                //}
            }

            if (Options.ExportFingerboardEdges && Options.FingerboardEdges.ContinueLines)
            {
                var endPoints = new List<PointM>();

                foreach (var guideLine in Layout.GetElements<LayoutLine>(l =>
                    l.ElementType == VisualElementType.FingerboardContinuation))
                {
                    endPoints.Add(guideLine.P2);
                    AddLayoutLine(guideLine, VisualElementType.FingerboardContinuation, Options.GuideLines);
                }

                var pt1 = endPoints.OrderBy(pt => pt.X).First();
                var pt2 = endPoints.OrderByDescending(pt => pt.X).First();
                var bridgeLine = new LayoutLine(pt1, pt2);
                AddLayoutLine(bridgeLine, VisualElementType.FingerboardContinuation, Options.GuideLines);
            }
        }

        private void GenerateStrings()
        {
            foreach (var stringLine in Layout.VisualElements.OfType<StringLine>().OrderBy(sl => sl.Index))
                AddLayoutLine(stringLine, VisualElementType.String, Options.Strings);
        }

        protected virtual void AddLayoutLine(LayoutLine line, VisualElementType elementType, LineExportConfig lineConfig)
        {

        }

        protected virtual void AddLayoutPolyLine(LayoutPolyLine line, VisualElementType elementType, LineExportConfig lineConfig)
        {

        }

        protected virtual void AddLayoutSpline(LayoutPolyLine line, VisualElementType elementType, LineExportConfig lineConfig)
        {

        }
    }
}
