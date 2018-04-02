using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Windows
{
    public partial class DetectScreenDPI : Form
    {
        private bool isLoading;

        public DetectScreenDPI()
        {
            InitializeComponent();
        }

        private ScreenInfo CurrentScreen
        {
            get { return cboScreens.SelectedItem as ScreenInfo; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cboScreens.DisplayMember = "DisplayName";
            cboScreens.ValueMember = "ID";
            cboScreens.DataSource = GetDisplays();
            cboScreens.SelectedValue = Screen.PrimaryScreen.DeviceName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Size = measureTextbox1.Value;

                var rs = (double)CurrentScreen.Width / (double)CurrentScreen.Height;
                var hyp = Math.Sqrt(1d + (rs * rs));
                var screenHeight = measureTextbox1.Value / hyp;
                numDPI.Value = (CurrentScreen.Height / screenHeight[UnitOfMeasure.Inches]);
                
            }
        }

        private void pbxMeasureDPI_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(pbxMeasureDPI.BackColor);
            if (CurrentScreen != null)
            {
                var g = e.Graphics;
                var targetSize = CurrentScreen.DPI;
                if (rbCM.Checked)
                    targetSize /= 2.54;
                g.DrawRectangle(Pens.Black, 0, 0, (float)targetSize, (float)targetSize);
                //e.Graphics.ScaleTransform
            }
        }

        private List<ScreenInfo> GetDisplays()
        {
            var displayList = new List<ScreenInfo>();

            foreach (var screen in Screen.AllScreens)
                displayList.Add(new ScreenInfo(screen));

            return displayList;
        }

        private class ScreenInfo
        {
            public string ID { get; set; }
            public int Number { get; set; }
            public bool IsPrimary { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public Measure Size;

            public double DPI { get; set; }

            public string DisplayName
            {
                get
                {
                    return string.Format("Screen #{0} {1}x{2}", Number, Width, Height);
                }
            }

            public ScreenInfo()
            {
                DPI = 96;
                Size = Measure.Inches(0);
            }

            public ScreenInfo(Screen screen)
            {
                DPI = 96;
                ID = screen.DeviceName;
                Number = int.Parse(screen.DeviceName.Substring(11));
                Width = screen.Bounds.Width;
                Height = screen.Bounds.Height;
                IsPrimary = screen.Primary;
                Size = Measure.Inches(0);

                //var diag = Math.Sqrt((Height * Height) + (Width * Width));
                //Size = Measure.Inches(diag / DPI);
            }
        }

        private void cboScreens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentScreen != null)
            {
                isLoading = true;
                numDPI.Value = CurrentScreen.DPI;
                measureTextbox1.Value = CurrentScreen.Size;
                isLoading = false;
            }
        }

        private void numDPI_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading && CurrentScreen != null)
            {
                CurrentScreen.DPI = numDPI.Value;
            }

            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (CurrentScreen != null)
            {
                var targetSize = CurrentScreen.DPI;
                if (rbCM.Checked)
                    targetSize /= 2.54;
                var sizeRounded = (int)Math.Ceiling(targetSize);
                pbxMeasureDPI.Size = new Size(sizeRounded + 1, sizeRounded + 1);
                pbxMeasureDPI.Invalidate();
            }
        }

        private void rbPreviewSize_CheckChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                UpdatePreview();
            }
        }
    }
}
