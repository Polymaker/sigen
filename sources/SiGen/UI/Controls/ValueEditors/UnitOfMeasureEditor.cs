using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SiGen.UI.Designers
{
    public class UnitOfMeasureEditor : UITypeEditor
    {
        private IWindowsFormsEditorService editorService;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {
                    var editControl = new ListBox();
                    var units = new UnitOfMeasure[] { UnitOfMeasure.Mm, UnitOfMeasure.Cm, UnitOfMeasure.In, UnitOfMeasure.Ft };

                    editControl.DisplayMember = "Name";
                    editControl.BorderStyle = BorderStyle.None;
                    editControl.IntegralHeight = false;
                    editControl.SelectionMode = SelectionMode.One;
                    editControl.SelectedIndexChanged += EditControl_SelectedIndexChanged;

                    foreach (var unit in units)
                    {
                        int index = editControl.Items.Add(unit);
                        if (unit == value)
                            editControl.SelectedIndex = index;
                    }

                    editorService.DropDownControl(editControl);

                    if (editControl.SelectedItem == null) // no selection, return the passed-in value as is
                        return value;

                    value = editControl.SelectedItem;

                }
            }
            return value;
        }

        private void EditControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorService.CloseDropDown();
        }

    }
}
