using SiGen.Common;
using SiGen.Configuration;
using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class LayoutViewerPanel : DockContent
    {
        private double ScreenDPI;
        private LayoutDocument _CurrentFile;

        public LayoutViewer Viewer { get { return layoutViewer1; } }

        public LayoutDocument CurrentDocument
        {
            get => _CurrentFile;
            set => SetCurrentLayout(value);
        }

        public SILayout CurrentLayout => CurrentDocument?.Layout;

        public LayoutEditorWindow Window { get; }

        public LayoutViewerPanel(LayoutEditorWindow window)
        {
            InitializeComponent();
            Window = window;
            DisplayOptionsDropDown.DropDown.Closing += DropDown_Closing;
        }

        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ScreenDPI = 109;
            Viewer.EnableMeasureTool = true;
            Viewer.SetDisplayConfig(AppConfig.Current.DisplayConfig);
            //Viewer.DisplayConfig.Strings.RenderMode = Configuration.Display.LineRenderMode.RealisticLook;
            //Viewer.DisplayConfig.Frets.RenderMode = Configuration.Display.LineRenderMode.RealisticLook;
        }

        private void SetCurrentLayout(LayoutDocument value)
        {
            if (value != _CurrentFile)
            {
                if (_CurrentFile != null)
                {
                    _CurrentFile.Layout.NumberOfStringsChanged -= Layout_NumberOfStringsChanged;
                }

                _CurrentFile = value;

                if (_CurrentFile != null)
                {
                    _CurrentFile.Layout.NumberOfStringsChanged += Layout_NumberOfStringsChanged;
                }

                Viewer.CurrentLayout = value?.Layout;
            }

        }

        private void Layout_NumberOfStringsChanged(object sender, EventArgs e)
        {
            //if (CurrentDocument.IsNew)
            //{
            //    Text = $"New {CurrentLayout.NumberOfStrings} Strings Layout";
            //    CurrentLayout.LayoutName = LayoutDocument.GenerateLayoutName(CurrentLayout);
            //}
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Viewer.ResetCamera();
        }

        private void layoutViewer1_ZoomChanged(object sender, EventArgs e)
        {
            var currentZoom = Viewer.Zoom;
            var dpi = ScreenDPI == 0 ? 96 : ScreenDPI;
            
            ZoomToolstripLabel.Text = string.Format("Zoom: {0:0.##}%", (currentZoom / (dpi / 2.54)) * 100);
        }

        private void cmsDocumentTab_Opening(object sender, CancelEventArgs e)
        {
            tsmiCloseOthers.Enabled = DockPanel.DocumentsCount > 1;
            tsmiCloseRight.Enabled = GetTabIndex() < DockPanel.DocumentsCount - 1;
            tsmiOpenFileDirectory.Enabled = !string.IsNullOrEmpty(CurrentDocument.FileName);
        }

        private void tsmiCloseLayout_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int GetTabIndex()
        {
            return Pane.Contents.IndexOf(this);
        }

        private void tsmiCloseOthers_Click(object sender, EventArgs e)
        {
            var otherDocuments = DockPanel.Documents.OfType<DockContent>().ToArray();
            for(int i = 0; i < otherDocuments.Length; i++)
            {
                if (otherDocuments[i] != this)
                    otherDocuments[i].Close();
            }
        }

        private void tsmiCloseRight_Click(object sender, EventArgs e)
        {
            var otherDocuments = Pane.Contents.OfType<DockContent>().ToArray();
            for (int i = GetTabIndex() + 1; i < otherDocuments.Length; i++)
                otherDocuments[i].Close();
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            Window.SaveLayout(CurrentDocument);
        }

        private void tsmiSaveAS_Click(object sender, EventArgs e)
        {
            Window.SaveLayout(CurrentDocument, true);
        }

        private void tsmiOpenFileDirectory_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", $"/select, \"{CurrentDocument.FileName}\"");
        }
       

        private void ResetCameraButton_Click(object sender, EventArgs e)
        {
            Viewer.ResetCamera();
        }

        private bool UpdatingDisplayOptionsMenuItem;

        private void DisplayOptionsDropDown_DropDownOpening(object sender, EventArgs e)
        {
            UpdatingDisplayOptionsMenuItem = true;

            DisplayStringsMenuItem.Checked = Viewer.DisplayConfig.ShowStrings;
            DisplayStringCentersMenuItem.Checked = Viewer.DisplayConfig.ShowMidlines;
            DisplayFretsMenuItem.Checked = Viewer.DisplayConfig.ShowFrets;
            DisplayMarginsMenuItem.Checked = Viewer.DisplayConfig.ShowMargins;
            DisplayFingerboardMenuItem.Checked = Viewer.DisplayConfig.ShowFingerboard;
            DisplayCenterLineMenuItem.Checked = Viewer.DisplayConfig.ShowCenterLine;

            UpdatingDisplayOptionsMenuItem = false;
        }

        private void DisplayOptionsMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdatingDisplayOptionsMenuItem)
                return;

            if (sender == DisplayStringsMenuItem)
            {
                Viewer.DisplayConfig.ShowStrings = DisplayStringsMenuItem.Checked;
            }
            else if (sender == DisplayStringCentersMenuItem)
            {
                Viewer.DisplayConfig.ShowMidlines = DisplayStringCentersMenuItem.Checked;
            }
            else if (sender == DisplayFretsMenuItem)
            {
                Viewer.DisplayConfig.ShowFrets = DisplayFretsMenuItem.Checked;
            }
            else if (sender == DisplayMarginsMenuItem)
            {
                Viewer.DisplayConfig.ShowMargins = DisplayMarginsMenuItem.Checked;
            }
            else if (sender == DisplayFingerboardMenuItem)
            {
                Viewer.DisplayConfig.ShowFingerboard= DisplayFingerboardMenuItem.Checked;
            }
            else if (sender == DisplayCenterLineMenuItem)
            {
                Viewer.DisplayConfig.ShowCenterLine = DisplayCenterLineMenuItem.Checked;
            }
        }

        private void tsbMeasureTool_CheckedChanged(object sender, EventArgs e)
        {
            Viewer.EnableMeasureTool = tsbMeasureTool.Checked;
        }

        
    }
}
