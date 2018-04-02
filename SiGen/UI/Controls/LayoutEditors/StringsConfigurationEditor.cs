using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class StringsConfigurationEditor : LayoutPropertyEditor
    {
        public StringsConfigurationEditor()
        {
            InitializeComponent();
        }

        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            numericBox1.Enabled = (CurrentLayout != null);

            if (CurrentLayout != null)
            {
                numericBox1.Value = CurrentLayout.NumberOfStrings;
            }
            else
            {
                numericBox1.Value = 6;
            }
        }

        private void numericBox1_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.NumberOfStrings = (int)numericBox1.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentLayout.CompensateFretPositions = !CurrentLayout.CompensateFretPositions;
            if (CurrentLayout.CompensateFretPositions)
                CurrentLayout.FretsTemperament = Physics.Temperament.ThidellFormula;
            else
                CurrentLayout.FretsTemperament = Physics.Temperament.Equal;
            CurrentLayout.RebuildLayout();
        }
    }
}
