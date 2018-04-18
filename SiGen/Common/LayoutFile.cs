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
        
        public SILayout Layout { get; set; }
        public string FileName { get; set; }

        public LayoutFile(SILayout layout)
        {
            Layout = layout;
            FileName = string.Empty;
        }

        public LayoutFile(string filename)
        {
            FileName = filename;
            Layout = SILayout.Load(filename);
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
