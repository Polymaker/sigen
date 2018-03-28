using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Controls
{
    [ToolboxItem(true)]
    public partial class TextBoxEx : TextBox
    {
        private bool _ValidateOnEnter;
        /// <summary>
        ///  Gets or sets a value indicating whether pressing ENTER in a System.Windows.Forms.TextBox will cause validation.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidateOnEnter
        {
            get { return _ValidateOnEnter; }
            set
            {
                if(value != _ValidateOnEnter)
                {
                    if (AcceptsReturn)
                        AcceptsReturn = false;
                    _ValidateOnEnter = value;
                }
            }
        }

        public event KeyEventHandler CommandKeyPressed;

        public TextBoxEx()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var kea = new KeyEventArgs(keyData);
            OnCommandKeyPressed(kea);
            
            if(!kea.SuppressKeyPress)
            {
                if (keyData == Keys.Enter && ValidateOnEnter)
                {
                    var parent = GetContainerControl();
                    if (parent != null && parent is ContainerControl)
                    {
                        (GetContainerControl() as ContainerControl).ValidateChildren();
                        return true;
                    }
                }

                return kea.Handled || base.ProcessCmdKey(ref msg, keyData);
            }
            return kea.Handled;
        }

        protected void OnCommandKeyPressed(KeyEventArgs e)
        {
            var handler = CommandKeyPressed;
            if (handler != null)
                handler(this, e);
        }
    }
}
