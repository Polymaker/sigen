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
        private MeasureTextbox editControl;

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
                        editControl = new MeasureTextbox();
                        editControl.AllowEmptyValue = true;
                    }

                    var previousValue = (Measure)value;
                    editControl.Value = previousValue;
                    windowsFormsEditorService.DropDownControl(editControl);

                    if (previousValue == editControl.Value && previousValue.Unit != editControl.Value.Unit)//the measure is equal but in another display unit
                        context.PropertyDescriptor.SetValue(context.Instance, editControl.Value);//force a ValueChange to keep the new display unit
                    value = editControl.Value;
                }
            }
            return value;
        }
    }
}
