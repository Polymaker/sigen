using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{
    public class FingerboardExportConfig : LineExportConfig
    {
        private bool _ContinueLines;

        public bool ContinueLines
        {
            get => _ContinueLines;
            set => SetPropertyValue(ref _ContinueLines, value);
        }
    }
}
