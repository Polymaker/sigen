using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace SiGen.UI.Designers
{
    public class MeasureEditor : UITypeEditor
    {
        private MeasureEdit editControl;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                var windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (windowsFormsEditorService != null)
                {
                    if (editControl == null)
                    {
                        editControl = new MeasureEdit();
                    }
                    var currentValue = (Measure)value;
                    editControl.Value = currentValue;
                    windowsFormsEditorService.DropDownControl(editControl);
                    if (editControl.Value != Measure.Empty/* && editControl.Value != currentValue*/)
                    {
                        value = editControl.Value;
                    }
                }
            }

            return value;
        }
    }
}
