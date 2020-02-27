using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace SiGen.UI.Controls
{
    internal class TextBoxDesigner : ControlDesigner
    {
        private ControlDesigner BaseDesigner;
        private static Type BaseDesignerType;

        static TextBoxDesigner()
        {
            BaseDesignerType = typeof(ControlDesigner).Assembly.GetType("System.Windows.Forms.Design.TextBoxDesigner");
        }

        public override IList SnapLines => BaseDesigner?.SnapLines ?? base.SnapLines;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            BaseDesigner = (ControlDesigner)Activator.CreateInstance(BaseDesignerType);
            BaseDesigner.Initialize(Control);
        }
    }
}
