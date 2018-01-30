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
            test.NumberOfStrings = 7;
            test.Strings.SetAll(v => v.NumberOfFrets, 24);

            //(test.CurrentScaleLength as ScaleLengthManager.SingleScale).Length = Measure.Inches(25.5);
            test.ScaleLengthMode = ScaleLengthType.Multiple;
            test.SingleScaleConfig.Length = Measure.Inches(25.5);
            test.MultiScaleConfig.Treble = Measure.Inches(25.5);
            test.MultiScaleConfig.Bass = Measure.Inches(27);

            test.Strings.MassAssign(s => s.PhysicalProperties,
                new StringProperties(Measure.Inches(0.009), Measure.Inches(0.009), 0.00001794, 29442660.75919),
                new StringProperties(Measure.Inches(0.011), Measure.Inches(0.011), 0.00002680, 29442660.75919),
                new StringProperties(Measure.Inches(0.016), Measure.Inches(0.016), 0.00005671, 29442660.75919),
                new StringProperties(Measure.Inches(0.014), Measure.Inches(0.024), 0.00010857, 29442660.75919),
                new StringProperties(Measure.Inches(0.015), Measure.Inches(0.032), 0.00019347, 29442660.75919),
                new StringProperties(Measure.Inches(0.018), Measure.Inches(0.042), 0.00032279, 29442660.75919),
                new StringProperties(Measure.Inches(0.021), Measure.Inches(0.054), 0.00053838, 29442660.75919)
                );

            test.Strings.MassAssign(s => s.ActionAtTwelfthFret,
                Measure.Inches(0.063),
                Measure.Inches(0.069),
                Measure.Inches(0.075),
                Measure.Inches(0.082),
                Measure.Inches(0.088),
                Measure.Inches(0.094),
                Measure.Inches(0.118)
                );

            test.Margins.Edges = Measure.Mm(3.5);
            
            test.StringSpacing.SetSpacing(0, Measure.Mm(7), true);
            test.StringSpacing.SetSpacing(0, Measure.Mm(10), false);
            LayoutBuilder.BuildLayout(test);
            layoutViewer1.CurrentLayout = test;
            
            //test.Save("template 7-strings multi-scale guitar.xml");

            //var test2 = SILayout.Load("template 6-strings guitar.xml");
        }
    }
}
