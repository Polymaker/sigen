using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout.Visual;

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class LayoutProperties : LayoutPropertyEditor
    {

        public LayoutProperties()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Parent.Padding = Padding.Empty;
        }
        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            FillLayoutInfo();
        }

        protected override void OnLayoutUpdated()
        {
            base.OnLayoutUpdated();
            FillLayoutInfo();
        }

        private void FillLayoutInfo()
        {
            var viewer = GetCurrentViewer();
            if (CurrentLayout != null)
            {
                var layoutBounds = CurrentLayout.GetLayoutBounds();

                if (viewer != null && viewer.DisplayConfig.FretboardOrientation == Orientation.Horizontal)
                {
                    mtbLayoutHeight.Value = layoutBounds.Width;
                    mtbLayoutWidth.Value = layoutBounds.Height;
                }
                else
                {
                    mtbLayoutWidth.Value = layoutBounds.Width;
                    mtbLayoutHeight.Value = layoutBounds.Height;
                }

                Measure minSize = Measure.Empty;
                Measure maxSize = Measure.Empty;
                Measure totalLen = Measure.Zero;
                foreach(var fretLine in CurrentLayout.VisualElements.OfType<FretLine>())
                {
                    Measure curSize = Measure.Zero;
                    for(int i = 0; i < fretLine.Points.Count - 1; i++)
                        curSize += PointM.Distance(fretLine.Points[i], fretLine.Points[i + 1]);

                    totalLen += curSize;

                    if (minSize.IsEmpty || maxSize.IsEmpty)
                    {
                        minSize = curSize;
                        maxSize = curSize;
                    }
                    else
                    {
                        if (curSize > maxSize)
                            maxSize = curSize;
                        if (curSize < minSize)
                            minSize = curSize;
                    }

                }
                measureTextbox1.Value = totalLen;
                measureTextbox1.ChangeDisplayedUnit(UnitOfMeasure.Inches);
                mtbShortFret.Value = minSize;
                mtbLongFret.Value = maxSize;

                if (viewer != null)
                {
                    mtbLayoutWidth.ChangeDisplayedUnit(viewer.DisplayConfig.DefaultDisplayUnit);
                    mtbLayoutHeight.ChangeDisplayedUnit(viewer.DisplayConfig.DefaultDisplayUnit);
                }
            }
            else
            {
                mtbLayoutWidth.Value = Measure.Empty;
                mtbLayoutHeight.Value = Measure.Empty;
                measureTextbox1.Value = Measure.Empty;
                mtbShortFret.Value = Measure.Empty;
                mtbLongFret.Value = Measure.Empty;
            }
        }
    }
}
