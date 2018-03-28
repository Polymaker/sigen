using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SiGen.UI.Controls
{
    [Designer(typeof(NumericBoxDesigner)), DefaultEvent("ValueChanged")]
    public partial class NumericBox : TextBox
    {
        //public enum DecimalSeperatorMode
        //{
        //    UseCulture,
        //    ForceComma,
        //    ForceDot,
        //    CommaOrDot
        //}

        #region Fields

        //private bool _AllowDecimals;
        //private int _DecimalPlaces;
        private double _Value;
        private double _MinimumValue;
        private double _MaximumValue;
        private bool updatingText;
        private bool _IsEditing;

        #endregion

        #region Properties

        [DefaultValue(0d), Bindable(true)]
        public double Value
        {
            get { return _Value; }
            set
            {
                if(value != _Value)
                {
                    if (value < MinimumValue || value > MaximumValue)
                        throw new ArgumentOutOfRangeException("Value");

                    _Value = value;
                    if (IsHandleCreated)
                        UpdateTextboxValue();
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(0d)]
        public double MinimumValue
        {
            get { return _MinimumValue; }
            set
            {
                if (value != _MinimumValue)
                {
                    if (value > _MaximumValue)
                        _MaximumValue = value;
                    _MinimumValue = value;
                    Value = ConstraintValue(_Value);
                }
            }
        }

        [DefaultValue(100d)]
        public double MaximumValue
        {
            get { return _MaximumValue; }
            set
            {
                if (value != _MaximumValue)
                {
                    if (_MinimumValue > value)
                        _MinimumValue = value;
                    _MaximumValue = value;
                    Value = ConstraintValue(_Value);
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler BeginEdit;
        public event EventHandler EndEdit;
        public event EventHandler ValueChanged;

        #endregion

        public NumericBox()
        {
            InitializeComponent();
            _MaximumValue = 100;
            
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateTextboxValue();
        }

        private void UpdateTextboxValue()
        {
            updatingText = true;
            base.Text = Value.ToString();
            updatingText = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Enter && _IsEditing)
            {
                if (GetContainerControl() is ContainerControl)
                    (GetContainerControl() as ContainerControl).ValidateChildren();
                else
                    PerformEndEdit();
                return true;
            }
            else if(keyData == Keys.Escape && _IsEditing)
            {
                CancelEdit();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!updatingText)
            {
                if (!_IsEditing)
                {
                    OnBeginEdit();
                    _IsEditing = true;
                }
            }
            base.OnTextChanged(e);
        }

        private double ConstraintValue(double value)
        {
            if (value < MinimumValue)
                value = MinimumValue;
            if (value > MaximumValue)
                value = MaximumValue;
            return value;
        }

        public void PerformEndEdit()
        {
            if (_IsEditing)
            {
                _IsEditing = false;
                OnEndEdit();

                double value;
                if (double.TryParse(base.Text, out value))
                {
                    Value = ConstraintValue(value);
                    //success
                }
                else
                {
                    //fail
                }
            }
        }

        public void CancelEdit()
        {
            if (_IsEditing)
            {
                _IsEditing = false;
                UpdateTextboxValue();
                OnEndEdit();
            }
        }

        protected void OnBeginEdit()
        {
            var handler = BeginEdit;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected void OnEndEdit()
        {
            var handler = EndEdit;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected void OnValueChanged(EventArgs e)
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(this, e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            
            if (e.Cancel)
                CancelEdit();
            else
                PerformEndEdit();
        }

        //private const int WM_PASTE = 0x0302;

        //protected override void WndProc(ref Message m)
        //{
        //    if(m.Msg == WM_PASTE)
        //    {
        //        var inputText = Clipboard.GetText();
        //    }
        //    else
        //        base.WndProc(ref m);

        //}
    }

    internal class NumericBoxDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                object component = base.Component;
                selectionRules |= SelectionRules.AllSizeable;
                var autosizeProperty = TypeDescriptor.GetProperties(component)["AutoSize"];
                if (autosizeProperty != null)
                {
                    object value2 = autosizeProperty.GetValue(component);
                    if (value2 is bool && (bool)value2)
                    {
                        selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                    }
                }
                return selectionRules;
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            var hiddenProperties = new string[]
            {
                "Text", "AutoCompleteCustomSource", "AutoCompleteMode", "AutoCompleteSource",
                "Lines", "CharacterCasing", "Multiline", "PasswordChar", "WordWrap",
                "MaxLength", "ScrollBars", "UseSystemPasswordChar"
            };
            foreach (var propName in hiddenProperties)
            {
                if (properties.Contains(propName))
                    properties.Remove(propName);
            }
        }
    }
}
