using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Common
{
    public class LayoutFile
    {
        public bool HasChanged { get; set; }
        public SILayout Layout { get; private set; }
        public string FileName { get; set; }

        public LayoutFile(SILayout layout)
        {
            Layout = layout;
            FileName = string.Empty;
            Layout.LayoutChanged += Layout_LayoutChanged;
        }

        private void Layout_LayoutChanged(object sender, EventArgs e)
        {
            HasChanged = true;
        }

        public static LayoutFile Open(string filename, bool asTemplate = false)
        {
            var layout = SILayout.Load(filename);
            var file = new LayoutFile(layout);
            if (!asTemplate)
                file.FileName = filename;
            return file;
        }

        public static LayoutFile OpenTemplate(string filename)
        {
            var layout = SILayout.Load(filename);
            return new LayoutFile(layout) { FileName = string.Empty };
        }
    }
}
