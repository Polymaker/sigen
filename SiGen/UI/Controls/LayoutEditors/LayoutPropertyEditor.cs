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
using System.Collections;

namespace SiGen.UI.Controls
{
    [ToolboxItem(false)]
    public partial class LayoutPropertyEditor : UserControl
    {
        private SILayout _CurrentLayout;
        /// <summary>
        /// Used to store display state information
        /// </summary>
        protected Dictionary<SILayout, Hashtable> CachedLayoutData;

        protected FlagList FlagManager;
        private bool _IsLoading;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SILayout CurrentLayout
        {
            get { return _CurrentLayout; }
            set { BindLayout(value); }
        }

        protected bool IsLoading { get { return _IsLoading; /* FlagManager["ReloadPropertyValues"] */} }

        public LayoutPropertyEditor()
        {
            InitializeComponent();
            FlagManager = new FlagList();
            CachedLayoutData = new Dictionary<SILayout, Hashtable>();
        }

        private void BindLayout(SILayout layout)
        {
            if(_CurrentLayout != layout)
            {
                if (_CurrentLayout != null)
                    CacheCurrentLayoutValues();

                if (_CurrentLayout != null)
                {
                    _CurrentLayout.NumberOfStringsChanged -= CurrentLayout_NumberOfStringsChanged;
                    _CurrentLayout.LayoutUpdated -= CurrentLayout_LayoutUpdated;
                }

                _CurrentLayout = layout;
                OnCurrentLayoutChanged();
                ReloadPropertyValues();

                if(_CurrentLayout != null)
                {
                    _CurrentLayout.NumberOfStringsChanged += CurrentLayout_NumberOfStringsChanged;
                    _CurrentLayout.LayoutUpdated += CurrentLayout_LayoutUpdated;
                }
                
                if (layout == null)
                    ClearLayoutCache(null);
                else if (CachedLayoutData.ContainsKey(layout))
                    RestoreCachedLayoutValues();
            }
        }

        private void CurrentLayout_NumberOfStringsChanged(object sender, EventArgs e)
        {
            OnNumberOfStringsChanged();
        }

        private void CurrentLayout_LayoutUpdated(object sender, EventArgs e)
        {
            OnLayoutUpdated();
        }

        protected virtual void OnNumberOfStringsChanged()
        {

        }

        protected virtual void OnLayoutUpdated()
        {

        }

        protected virtual void OnCurrentLayoutChanged()
        {

        }
        
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

        protected virtual void CacheCurrentLayoutValues()
        {
            if (!CachedLayoutData.ContainsKey(CurrentLayout))
                CachedLayoutData.Add(CurrentLayout, new Hashtable());
        }

        protected virtual void RestoreCachedLayoutValues()
        {

        }

        public virtual void ClearLayoutCache(SILayout layout)
        {
            if (layout == null)
                CachedLayoutData.Clear();
            else if (CachedLayoutData.ContainsKey(layout))
                CachedLayoutData.Remove(layout);
        }

        protected LayoutViewer GetCurrentViewer()
        {
            if(Parent != null && Parent is WeifenLuo.WinFormsUI.Docking.DockContent)
            {
                var activeDoc = (Parent as WeifenLuo.WinFormsUI.Docking.DockContent).DockPanel.ActiveDocument;
                if(activeDoc != null)
                {
                    return (activeDoc as WeifenLuo.WinFormsUI.Docking.DockContent).Controls.OfType<LayoutViewer>().FirstOrDefault();
                }
            }
            return null;
        }

        protected void SetControlsVisibility(bool condition, params Control[] controls)
        {
            foreach (var ctrl in controls)
                ctrl.Visible = condition;
        }
    }
}
