using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.StringedInstruments.Layout;
using SiGen.Utilities;

namespace SiGen.UI.Controls
{
    [ToolboxItem(false)]
    public partial class LayoutPropertyEditor : UserControl
    {
        private SILayout _CurrentLayout;
        protected FlagList FlagManager;
        private bool _IsLoading;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SILayout CurrentLayout
        {
            get { return _CurrentLayout; }
            set { BindLayout(value); }
        }

        protected bool IsLoading { get { return _IsLoading; } }

        public LayoutPropertyEditor()
        {
            InitializeComponent();
            FlagManager = new FlagList();
        }

        private void BindLayout(SILayout layout)
        {
            if(_CurrentLayout != layout)
            {
                //if(_CurrentLayout != null)
                //    _CurrentLayout.LayoutUpdated -= LayoutUpdated;
                _CurrentLayout = layout;
                //if (_CurrentLayout != null)
                //    _CurrentLayout.LayoutUpdated += LayoutUpdated;
                OnLayoutChanged();
                ReloadPropertyValues();
            }
        }

        protected virtual void OnLayoutChanged()
        {

        }

        //private void LayoutUpdated(object sender, EventArgs e)
        //{

        //}

        public void ReloadPropertyValues()
        {
            _IsLoading = true;

            using (FlagManager.UseFlag("ReloadPropertyValues"))
                ReadLayoutProperties();

            _IsLoading = false;
        }

        protected virtual void ReadLayoutProperties()
        {

        }
    }
}
