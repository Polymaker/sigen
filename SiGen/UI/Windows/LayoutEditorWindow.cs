using SiGen.Common;
using SiGen.Configuration;
using SiGen.Export;
using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout;
using SiGen.UI.Controls;
using SiGen.UI.Controls.LayoutEditors;
using SiGen.UI.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SiGen.UI
{
    public partial class LayoutEditorWindow : Form
    {
        private LayoutEditorPanel<StringSpacingEditor> stringSpacingPanel;
        private LayoutEditorPanel<StringsConfigurationEditor> stringConfigPanel;
        private LayoutEditorPanel<FingerboardMarginEditor> layoutMarginPanel;
        private LayoutEditorPanel<ScaleLengthEditor> scaleLengthPanel;
        //private LayoutEditorPanel<LayoutProperties> layoutInfoPanel;
        private LayoutViewerPanel PreviousDocument;
        private string[] FilesToOpen;

        public LayoutViewerPanel ActiveDocument
        {
            get
            {
                return dockPanel1.ActiveDocument as LayoutViewerPanel;
            }
        }

        public LayoutDocument CurrentLayoutDocument => ActiveDocument?.CurrentDocument;

        private IEnumerable<LayoutViewerPanel> OpenDocuments
        {
            get
            {
                return dockPanel1.Documents.OfType<LayoutViewerPanel>();
            }
        }

        public LayoutEditorWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.SigenIcon;
        }

        public LayoutEditorWindow(string[] args)
        {
            InitializeComponent();
            Icon = Properties.Resources.SigenIcon;
            if (args != null)
                FilesToOpen = args.Where(x => File.Exists(x)).ToArray();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeEditingPanels();

            if(FilesToOpen != null && FilesToOpen.Length > 0)
            {
                foreach (var path in FilesToOpen)
                    OpenLayoutFile(path, false);
            }
            else
            {
                OpenDefaultLayout();
            }
            AppConfigManager.ValidateRecentFiles();
            RebuildRecentFilesMenu();
        }

        #region Document Management

        private void InitializeEditingPanels()
        {
            dockPanel1.DockBottomPortion = 350d / (double)dockPanel1.Height;
            
            stringConfigPanel = new LayoutEditorPanel<StringsConfigurationEditor>();
            stringConfigPanel.Show(dockPanel1, DockState.DockBottom);
            stringConfigPanel.Text = "General Configuration";

            layoutMarginPanel = new LayoutEditorPanel<FingerboardMarginEditor>();
            layoutMarginPanel.Show(stringConfigPanel.Pane, DockAlignment.Right, .6);
            layoutMarginPanel.Text = "Fingerboard Margins";

            stringSpacingPanel = new LayoutEditorPanel<StringSpacingEditor>();
            stringSpacingPanel.Show(layoutMarginPanel.Pane, DockAlignment.Right, 0.5);
            stringSpacingPanel.Text = "String Spacing";

            //layoutInfoPanel = new LayoutEditorPanel<LayoutProperties>();
            //layoutInfoPanel.Show(layoutMarginPanel.Pane, DockAlignment.Bottom, .4);
            //layoutInfoPanel.Text = "Layout Properties";

            scaleLengthPanel = new LayoutEditorPanel<ScaleLengthEditor>();
            scaleLengthPanel.Show(layoutMarginPanel.Pane, DockAlignment.Bottom, .5);
            //scaleLengthPanel.Show(stringConfigPanel.Pane, null);
            scaleLengthPanel.Text = "Scale Length";
            //stringConfigPanel.Activate();

        }

        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if(PreviousDocument != null)
                PreviousDocument.CurrentDocument.LayoutChanged -= CurrentFile_LayoutChanged;

            if (CurrentLayoutDocument != null)
            {
                SetEditorsActiveLayout(CurrentLayoutDocument.Layout);
                tsbMeasureTool.Checked = ActiveDocument.Viewer.EnableMeasureTool;
                tsbMeasureTool.CheckOnClick = true;
                CurrentLayoutDocument.LayoutChanged += CurrentFile_LayoutChanged;
            }
            else
            {
                SetEditorsActiveLayout(null);
                tsbMeasureTool.Checked = false;
                tsbMeasureTool.CheckOnClick = false;
            }

            tsbClose.Enabled = CurrentLayoutDocument != null;
            tsbExport.Enabled = CurrentLayoutDocument != null;


            RebuildUndoRedoMenus();

			PreviousDocument = ActiveDocument;
        }

        private void CurrentFile_LayoutChanged(object sender, EventArgs e)
        {
            if(CurrentLayoutDocument != null)
            {
				RebuildUndoRedoMenus();
			}
        }

        private void SetEditorsActiveLayout(SILayout layout)
        {
            foreach (var editorPanel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
                editorPanel.CurrentLayout = layout;
        }

        private void CleanupEditorsLayoutCache(SILayout removedLayout)
        {
            foreach (var editorPanel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
                editorPanel.Editor.LayoutClosed(removedLayout);
        }

        private void RefreshCurrentLayoutEditors()
		{
			if(ActiveDocument != null)
			{
				foreach (var editorPanel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
					editorPanel.Editor.ReloadPropertyValues();
			}
		}

		private T GetLayoutEditor<T>() where T : LayoutPropertyEditor
		{
			foreach (var editorPanel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
			{
				if (editorPanel.Editor.GetType() == typeof(T))
					return (T)editorPanel.Editor;
			}
			return default(T);
		}

        private DockContent CreateDocumentPanel(LayoutDocument layoutFile)
        {
            var documentPanel = new LayoutViewerPanel(this);

            if (string.IsNullOrEmpty(layoutFile.FileName))
            {
                documentPanel.Text = $"New {layoutFile.Layout.NumberOfStrings} Strings Layout";
            }
            else if (!string.IsNullOrEmpty(layoutFile.Layout.LayoutName))
                documentPanel.Text = layoutFile.Layout.LayoutName;
            else
                documentPanel.Text = Path.GetFileNameWithoutExtension(layoutFile.FileName);

            int num = 0;
            string origName = documentPanel.Text;
            foreach (DockContent otherDoc in dockPanel1.Documents)
            {
                if (otherDoc.Text == documentPanel.Text)
                {
                    documentPanel.Text = origName + " " + (++num);
                }
            }

            if (!string.IsNullOrEmpty(layoutFile.FileName))
                documentPanel.ToolTipText = layoutFile.FileName;

            documentPanel.DockAreas = DockAreas.Document;// | DockAreas.Float;

            if (layoutFile.Layout.VisualElements.Count == 0 || layoutFile.Layout.IsLayoutDirty)
                layoutFile.Layout.RebuildLayout();

            documentPanel.CurrentDocument = layoutFile;
            documentPanel.Viewer.BackColor = Color.White;
            documentPanel.Viewer.Font = new Font(Font.FontFamily, Font.Size * 1.4f);
            documentPanel.Viewer.DisplayConfig.RenderRealStrings = true;
            documentPanel.Viewer.Select();
            documentPanel.Tag = layoutFile;
            documentPanel.FormClosing += DocumentPanel_FormClosing;
            documentPanel.FormClosed += DocumentPanel_FormClosed;
            return documentPanel;
        }

        private void DocumentPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            var documentFile = ((LayoutViewerPanel)sender).CurrentDocument;
            foreach (var panel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
                panel.Editor.ClearLayoutCache(documentFile.Layout);
        }

        private void DocumentPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            var panel = (LayoutViewerPanel)sender;
            var documentFile = panel.CurrentDocument;
            if (documentFile.HasChanged)
            {
                panel.Activate();
                var result = MessageBox.Show(MSG_SaveBeforeClose, LBL_Warning, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                switch (result)
                {
                    case DialogResult.Yes:
                        if(!SaveLayout(documentFile))
                            e.Cancel = true;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            if (!e.Cancel)
                CleanupEditorsLayoutCache(documentFile.Layout);
        }

        #endregion
        
        #region Save

        public bool SaveLayout(LayoutDocument file, bool selectPath = false)
        {
            bool isNew = string.IsNullOrEmpty(file.FileName);

            if (string.IsNullOrEmpty(file.Layout.LayoutName))
            {
                file.Layout.LayoutName = LayoutDocument.GenerateLayoutName(file.Layout);
            }

            if (selectPath || isNew)
            {
                using (var sfd = new SaveFileDialog())
                {
                    if(!string.IsNullOrEmpty(file.FileName))
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(file.FileName);
                        sfd.FileName = Path.GetFileName(file.FileName);
                    }
                    else
                        sfd.FileName = LayoutDocument.GenerateLayoutName(file.Layout) + ".sil";

                    sfd.Filter = "SI Layout Files|*.sil|All Files|*.*";
                    sfd.DefaultExt = ".sil";

                    if (sfd.ShowDialog() == DialogResult.OK)
                        file.FileName = sfd.FileName;
                    else
                        return false;
                }
            }
            
            //if (string.IsNullOrEmpty(file.Layout.LayoutName))
            //    file.Layout.LayoutName = Path.GetFileNameWithoutExtension(file.FileName);

            file.Layout.Save(file.FileName);
            file.HasChanged = false;
            var documentTab = OpenDocuments.FirstOrDefault(d => d.CurrentDocument == file);
            if (documentTab != null)
            {
                documentTab.TabText = string.IsNullOrEmpty(file.Layout.LayoutName ) ? Path.GetFileNameWithoutExtension(file.FileName) : file.Layout.LayoutName;
                documentTab.ToolTipText = file.FileName;
            }

            if (isNew)
            {
                AppConfigManager.AddRecentFile(file.FileName);
                RebuildRecentFilesMenu();
            }

            return true;
        }

        private void tssbSave_ButtonClick(object sender, EventArgs e)
        {
            if (CurrentLayoutDocument != null)
                SaveLayout(CurrentLayoutDocument, string.IsNullOrEmpty(CurrentLayoutDocument.FileName));
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            if (CurrentLayoutDocument != null)
                SaveLayout(CurrentLayoutDocument, string.IsNullOrEmpty(CurrentLayoutDocument.FileName));
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (CurrentLayoutDocument != null)
                SaveLayout(CurrentLayoutDocument, true);
        }

        #endregion

        #region Open

        private void OpenDefaultLayout()
        {
            var defaultLayout = SILayout.GenerateDefaultLayout();
            var layoutDoc = new LayoutDocument(defaultLayout);
            LoadLayout(layoutDoc);
            //OpenLayoutFile("DefaultLayout.sil", true);
        }

        private void OpenLayoutFile(string filename, bool asTemplate = false)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show("Error", "The file does not exist.");
                return;
            }

            try
            {
                bool cancelOpen = false;
                if (!asTemplate)
                {
                    foreach(var doc in dockPanel1.Documents)
                    {
                        if((doc as LayoutViewerPanel).CurrentDocument.FileName == filename)
                        {
                            (doc as LayoutViewerPanel).Activate();
                            var result = MessageBox.Show(MSG_FileAlreadyOpen, "", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                var layout = LayoutDocument.Open(filename, false);
                                layout.Layout.RebuildLayout();
                                (doc as LayoutViewerPanel).CurrentDocument = layout;
                                SetEditorsActiveLayout(layout.Layout);
                            }
                            cancelOpen = true;
                            break;
                        }
                    }
                }

                if(!cancelOpen)
                {
                    var loadedLayout = LayoutDocument.Open(filename, asTemplate);
                    LoadLayout(loadedLayout);
                }

                if (!asTemplate)
                {
                    AppConfigManager.AddRecentFile(filename);
                    RebuildRecentFilesMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while opening the file: " + ex.ToString(), "Error");
            }
        }

 
        private void tssbOpen_MouseHover(object sender, EventArgs e)
        {
            //tssbOpen.ShowDropDown();
        }

        private void LoadLayout(LayoutDocument layout)
        {
            var documentPanel = CreateDocumentPanel(layout);
            documentPanel.Show(dockPanel1, DockState.Document);
        }

        private void RebuildRecentFilesMenu()
        {
            for (int i = tssbOpen.DropDownItems.Count - 1; i >= 0; i--)
            {
                if (tssbOpen.DropDownItems[i].Tag is RecentFile)
                {
                    tssbOpen.DropDownItems[i].Click -= RecentFileMenu_Click;
                    tssbOpen.DropDownItems.RemoveAt(i);
                }
            }

            int counter = 0;

            foreach (var filename in AppConfigManager.Current.RecentFiles)
            {
                var fileMenu = tssbOpen.DropDownItems.Add(string.Format("{0}: {1}", ++counter, filename.Filename));
                fileMenu.Tag = filename;
                fileMenu.Click += RecentFileMenu_Click;
            }

            tsSeparatorOpen.Visible = AppConfigManager.Current.RecentFiles.Count > 0;
        }

        private void RecentFileMenu_Click(object sender, EventArgs e)
        {
            OpenLayoutFile(((sender as ToolStripItem).Tag as RecentFile).Filename);
        }

        #endregion

        
    }
}
