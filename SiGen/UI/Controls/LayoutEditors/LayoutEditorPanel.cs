﻿using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace SiGen.UI.Controls.LayoutEditors
{
    public interface ILayoutEditorPanel
    {
        LayoutPropertyEditor Editor { get; }
        SILayout CurrentLayout { get; set; }
    }

    public class LayoutEditorPanel<T> : DockContent, ILayoutEditorPanel where T : LayoutPropertyEditor
    {
        private T _Editor;

        public T Editor { get { return _Editor; } }

        public SILayout CurrentLayout
        {
            get { return Editor.CurrentLayout; }
            set
            {
                Editor.CurrentLayout = value;
            }
        }

        LayoutPropertyEditor ILayoutEditorPanel.Editor
        {
            get { return _Editor; }
        }

        public LayoutEditorPanel()
        {
            Padding = new System.Windows.Forms.Padding(3);
            _Editor = Activator.CreateInstance<T>();
           
            //_Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            CloseButtonVisible = false;
            DockAreas ^= DockAreas.Document;
            MaximizeBox = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Controls.Add(_Editor);
            _Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            Text = _Editor.Title;
        }

        protected override void OnDockStateChanged(EventArgs e)
        {
            base.OnDockStateChanged(e);
            if (DockState == DockState.Float && Text.Contains("&&"))
                Text = Text.Replace("&&", "&");
            else if(DockState != DockState.Float && Text.Contains(" & "))
                Text = Text.Replace(" & ", " && ");
        }
    }
}
