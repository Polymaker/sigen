using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.Utilities
{
    static class ControlHelper
    {
        private static System.Reflection.MethodInfo GetTextBaselineMethod;

        public static int GetTextBaseline(Control ctrl, ContentAlignment alignment)
        {
            if (GetTextBaselineMethod == null)
            {
                var designerClass = Type.GetType("System.Windows.Forms.Design.DesignerUtils, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                if (designerClass != null)
                    GetTextBaselineMethod = designerClass.GetMethod("GetTextBaseline");
            }

            if (GetTextBaselineMethod != null)
                return (int)GetTextBaselineMethod.Invoke(null, new object[] { ctrl, alignment });
            return 0;
        }
    }
}
