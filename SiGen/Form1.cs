using SiGen.Measuring;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SiGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var test = new SILayout();
            test.NumberOfStrings = 6;
            
            (test.CurrentScaleLength as ScaleLengthManager.SingleScale).Length = Measure.Inches(25.5);
            test.StringSpacing.SetSpacing(0, Measure.Mm(7), true);
            test.StringSpacing.SetSpacing(0, Measure.Mm(10), false);
            LayoutBuilder.BuildLayout(test);
        }
    }
}
