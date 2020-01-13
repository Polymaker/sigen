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

        public LayoutExportOptions Options { get; protected set; }

        protected LayoutExporterBase(SILayout layout, LayoutExportOptions options)
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
                if (fretLine.IsStraight)
                {
                    var layoutLine = new LayoutLine(fretLine.Points.First(), fretLine.Points.Last());
                    if (Options.ExtendFretSlots && (fretLine.Length + (Options.Frets.ExtensionAmount * 2)).NormalizedValue > 0)
                    {
                        layoutLine.P2 += (layoutLine.Direction * Options.Frets.ExtensionAmount);
                        layoutLine.P1 += (layoutLine.Direction * (Options.Frets.ExtensionAmount * -1));
                    }
                    layoutLine.Tag = fretLine;
                    AddLayoutLine(layoutLine, VisualElementType.Fret, Options.Frets);
                }
                else
                {
                    fretLine.RebuildSpline();

                    if (Options.ExtendFretSlots && (fretLine.Length + (Options.Frets.ExtensionAmount * 2)).NormalizedValue > 0)
                    {
                        var tmpLine = new LayoutPolyLine(fretLine.Points);
                        var bounds = fretLine.GetFretBoundaries(false);
                        var offset1 = LayoutLine.Offset(bounds.Item1, Options.Frets.ExtensionAmount * -1);
                        var offset2 = LayoutLine.Offset(bounds.Item2, Options.Frets.ExtensionAmount);

                        tmpLine.TrimBetween(offset1, offset2, true);
                        if (fretLine.Spline != null)
                            tmpLine.InterpolateSpline(0.5);

                        tmpLine.Tag = fretLine;

                        if (tmpLine.Spline != null)
                            AddLayoutSpline(tmpLine, VisualElementType.Fret, Options.Frets);
                        else
                            AddLayoutPolyLine(tmpLine, VisualElementType.Fret, Options.Frets);
                    }
                    else
                    {
                        if (fretLine.Spline != null)
                            AddLayoutSpline(fretLine, VisualElementType.Fret, Options.Frets);
                        else
                            AddLayoutPolyLine(fretLine, VisualElementType.Fret, Options.Frets);
                    }
                }
            }
        }

        private void GenerateFingerboard()
        {
            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardSideEdge>())
                AddLayoutLine(fingerboardEdge, VisualElementType.FingerboardEdge, Options.FingerboardEdges);

            foreach (var fingerboardEdge in Layout.VisualElements.OfType<FingerboardEdge>())
            {
                if (fingerboardEdge.Spline != null)
                    AddLayoutSpline(fingerboardEdge, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
                else
                    AddLayoutPolyLine(fingerboardEdge, VisualElementType.FingerboardEdge, Options.FingerboardEdges);
            }
        }

        private void GenerateLayoutLines()
        {
            if (Options.ExportCenterLine)
            {
                bool centerExist = false;
                var centerLine = Layout.VisualElements.OfType<LayoutLine>().First(x => x.ElementType == VisualElementType.CenterLine);

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
                    AddLayoutLine(centerLine, VisualElementType.CenterLine, Options.CenterLine);
            }

            if (Options.ExportStringCenters)
            {
                foreach (var stringCenter in Layout.VisualElements.OfType<StringCenter>())
                    AddLayoutLine(stringCenter, VisualElementType.StringCenter, Options.StringCenters);
            }

            if (!Options.ExportStrings && Options.ExportFingerboardMargins)//export first & last string to show fingerboard margin
            {
                var margins = Layout.VisualElements.OfType<LayoutLine>().Where(x => x.ElementType == VisualElementType.FingerboardMargin);

                if (margins.Any())
                {
                    foreach(var marginLine in margins)
                        AddLayoutLine(marginLine, VisualElementType.FingerboardMargin, Options.FingerboardMargins);
                }
                else
                {
                    var firstString = Layout.FirstString.LayoutLine;
                    var lastString = Layout.LastString.LayoutLine;
                    var trebleEdge = Layout.GetStringBoundaryLine(Layout.FirstString, FingerboardSide.Treble);
                    var bassEdge = Layout.GetStringBoundaryLine(Layout.LastString, FingerboardSide.Bass);

                    var trebleMargin = new LayoutLine(firstString.P1, firstString.SnapToLine(trebleEdge.P2, LineSnapDirection.Horizontal));
                    AddLayoutLine(trebleMargin, VisualElementType.FingerboardMargin, Options.FingerboardMargins);

                    if (firstString != lastString)
                    {
                        var bassMargin = new LayoutLine(lastString.P1, lastString.SnapToLine(bassEdge.P2, LineSnapDirection.Horizontal));
                        AddLayoutLine(bassMargin, VisualElementType.FingerboardMargin, Options.FingerboardMargins);
                    }
                }
            }

            //foreach (var guideLine in Layout.VisualElements.OfType<LayoutLine>().Where(l => l.ElementType == VisualElementType.GuideLine))
            //{
            //    AddLayoutLine(guideLine, VisualElementType.GuideLine);
            //}

            if (Options.ExportFingerboardEdges)
            {
                foreach (var guideLine in Layout.VisualElements.OfType<LayoutLine>().Where(l => l.ElementType == VisualElementType.FingerboardContinuation))
                    AddLayoutLine(guideLine, VisualElementType.GuideLine, Options.GuideLines);
            }
        }

        private void GenerateStrings()
        {
            foreach (var stringLine in Layout.VisualElements.OfType<StringLine>().OrderBy(sl => sl.Index))
                AddLayoutLine(stringLine, VisualElementType.String, Options.Strings);
        }

        protected virtual void AddLayoutLine(LayoutLine line, VisualElementType elementType, LayoutLineExportConfig lineConfig)
        {

        }

        protected virtual void AddLayoutPolyLine(LayoutPolyLine line, VisualElementType elementType, LayoutLineExportConfig lineConfig)
        {

        }

        protected virtual void AddLayoutSpline(LayoutPolyLine line, VisualElementType elementType, LayoutLineExportConfig lineConfig)
        {

        }
    }
}
