using SiGen.Common;
using SiGen.Configuration;
using SiGen.Export;
using SiGen.Measuring;
using SiGen.Physics;
using SiGen.Resources;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout;
using SiGen.UI.Controls;
using SiGen.UI.Controls.LayoutEditors;
using SiGen.UI.Windows;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        private IEnumerable<LayoutViewerPanel> OpenDocumentPanels
        {
            get
            {
                return dockPanel1.Documents.OfType<LayoutViewerPanel>();
            }
        }

        private IEnumerable<LayoutDocument> OpenDocuments => OpenDocumentPanels.Select(x => x.CurrentDocument);

        //private IEnumerable<LayoutViewerPanel> OpenDocumentPanels
        //{
        //    get
        //    {
        //        return dockPanel1.Documents.OfType<LayoutViewerPanel>();
        //    }
        //}

        public LayoutEditorWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.SiGenIcon;
        }

        public LayoutEditorWindow(string[] args)
        {
            InitializeComponent();
            Icon = Properties.Resources.SiGenIcon;

            
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

        private void UpdateWindowTitle()
        {
            if (CurrentLayoutDocument != null)
                Text = "SiGen: " + CurrentLayoutDocument.DocumentName;
            else
                Text = "SiGen";
        }

        #region Document Management

        private void InitializeEditingPanels()
        {
            dockPanel1.DockBottomPortion = 350d / (double)dockPanel1.Height;
            
            stringConfigPanel = new LayoutEditorPanel<StringsConfigurationEditor>();
            stringConfigPanel.Show(dockPanel1, DockState.DockBottom);
            //stringConfigPanel.Text = "General Configuration";

            layoutMarginPanel = new LayoutEditorPanel<FingerboardMarginEditor>();
            layoutMarginPanel.Show(stringConfigPanel.Pane, DockAlignment.Right, .6);
            //layoutMarginPanel.Text = "Fingerboard Margins";

            stringSpacingPanel = new LayoutEditorPanel<StringSpacingEditor>();
            stringSpacingPanel.Show(layoutMarginPanel.Pane, DockAlignment.Right, 0.5);
            //stringSpacingPanel.Text = "String Spacing";

            //layoutInfoPanel = new LayoutEditorPanel<LayoutProperties>();
            //layoutInfoPanel.Show(layoutMarginPanel.Pane, DockAlignment.Bottom, .4);
            //layoutInfoPanel.Text = "Layout Properties";

            scaleLengthPanel = new LayoutEditorPanel<ScaleLengthEditor>();
            scaleLengthPanel.Show(layoutMarginPanel.Pane, DockAlignment.Bottom, .5);
            //scaleLengthPanel.Show(stringConfigPanel.Pane, null);
            //scaleLengthPanel.Text = "Scale Length";
            //stringConfigPanel.Activate();

        }

        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if(PreviousDocument != null)
                PreviousDocument.CurrentDocument.LayoutChanged -= CurrentFile_LayoutChanged;

            if (CurrentLayoutDocument != null)
            {
                SetEditorsActiveLayout(CurrentLayoutDocument.Layout);
                CurrentLayoutDocument.LayoutChanged += CurrentFile_LayoutChanged;
            }
            else
            {
                SetEditorsActiveLayout(null);
            }

            UpdateWindowTitle();
            RefreshToolbarButtonStates();
            RebuildUndoRedoMenus();

			PreviousDocument = ActiveDocument;
        }

        private void CurrentFile_LayoutChanged(object sender, EventArgs e)
        {
            if (CurrentLayoutDocument != null)
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

        private void OpenLayoutAndActivate(LayoutDocument layout)
        {
            var documentPanel = CreateDocumentPanel(layout);
            documentPanel.Show(dockPanel1, DockState.Document);
        }

        private DockContent CreateDocumentPanel(LayoutDocument layoutFile)
        {
            var documentPanel = new LayoutViewerPanel(this);

            if (string.IsNullOrEmpty(layoutFile.DocumentName))
                layoutFile.DocumentName = Localizations.Misc_NewLayout;

            if (layoutFile.IsNew)
                layoutFile.DocumentName = GetUniqueDocumentName(layoutFile.DocumentName);

            documentPanel.Text = layoutFile.DocumentName;

            if (!string.IsNullOrEmpty(layoutFile.FilePath))
                documentPanel.ToolTipText = layoutFile.FilePath;

            documentPanel.DockAreas = DockAreas.Document;// | DockAreas.Float;

            if (layoutFile.Layout.VisualElements.Count == 0 || layoutFile.Layout.IsLayoutDirty)
                layoutFile.Layout.RebuildLayout();

            documentPanel.CurrentDocument = layoutFile;
            documentPanel.Viewer.BackColor = Color.White;
            documentPanel.Viewer.Font = new Font(Font.FontFamily, Font.Size * 1.4f);
            
            documentPanel.Viewer.Select();
            documentPanel.Tag = layoutFile;

            documentPanel.FormClosing += DocumentPanel_FormClosing;
            documentPanel.FormClosed += DocumentPanel_FormClosed;
            return documentPanel;
        }

        private string GetUniqueDocumentName(string name)
        {
            int count = 1;
            string finalName = name;

            while (OpenDocuments.Any(x => x.IsNew && x.DocumentName == finalName))
            {
                finalName = $"{name} {count++}";
            }

            return finalName;
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

                var result = MessageBox.Show(
                    Localizations.Messages_SaveBeforeClosing, 
                    Localizations.Messages_Warning, 
                    MessageBoxButtons.YesNoCancel, 
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);

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

        public LayoutViewerPanel GetDocumentPanel(LayoutDocument document)
        {
            foreach (var docPanel in dockPanel1.Documents.OfType<LayoutViewerPanel>())
            {
                if (docPanel.CurrentDocument == document)
                    return docPanel;
            }
            return null;
        }

        #endregion

        #region Save

        public bool SaveLayout(LayoutDocument file, bool selectPath = false)
        {
            bool isNew = string.IsNullOrEmpty(file.FilePath);

            string targetPath = file.FilePath ?? string.Empty;

            if (selectPath || isNew)
            {
                using (var sfd = new SaveFileDialog())
                {
                    if(!string.IsNullOrEmpty(file.FilePath))
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(file.FilePath);
                        sfd.FileName = Path.GetFileName(file.FilePath);
                    }
                    else
                        sfd.FileName = LayoutDocument.GenerateLayoutName(file.Layout) + ".sil";

                    sfd.Filter = $"{Localizations.Misc_LayoutFileDesc}|*.sil|{Localizations.Misc_AllFilesDesc}|*.*";
                    sfd.DefaultExt = ".sil";

                    if (sfd.ShowDialog() == DialogResult.OK)
                        targetPath = sfd.FileName;
                    else
                        return false;
                }
            }

            file.Save(targetPath);

            var documentTab = GetDocumentPanel(file);
            if (documentTab != null)
            {
                documentTab.TabText = file.DocumentName;
                documentTab.ToolTipText = file.FilePath;
            }

            UpdateWindowTitle();

            if (isNew)
            {
                AppConfigManager.AddRecentFile(file.FilePath);
                RebuildRecentFilesMenu();
            }

            return true;
        }

        #endregion

        #region Open

        private void OpenDefaultLayout()
        {
            var defaultLayout = SILayout.GenerateDefaultLayout();
            var layoutDoc = new LayoutDocument(defaultLayout);
            OpenLayoutAndActivate(layoutDoc);
        }

        private void OpenLayoutFile(string filename, bool asTemplate = false)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show(Localizations.Errors_FileNotFound, Localizations.Messages_Error);
                return;
            }

            if (!asTemplate)
            {
                var existingDocument = OpenDocuments.FirstOrDefault(x => x.FilePath == filename);

                if (existingDocument != null)
                {
                    var documentPanel = GetDocumentPanel(existingDocument);
                    documentPanel.Activate();

                    var result = MessageBox.Show(Localizations.Messages_FileAlreadyOpen, Localizations.Messages_OpeningFile, MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            var layout = LayoutDocument.Open(filename, false);
                            layout.Layout.RebuildLayout();
                            documentPanel.CurrentDocument = layout;
                            SetEditorsActiveLayout(layout.Layout);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Localizations.Errors_OpeningFile + Environment.NewLine + ex.ToString(), Localizations.Messages_Error);
                        }
                    }

                    return;
                }
            }

            try
            {
                var loadedLayout = LayoutDocument.Open(filename, asTemplate);
                OpenLayoutAndActivate(loadedLayout);

                if (!asTemplate)
                {
                    AppConfigManager.AddRecentFile(filename);
                    RebuildRecentFilesMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localizations.Errors_OpeningFile + Environment.NewLine + ex.ToString(), Localizations.Messages_Error);
            }
        }

 
        private void tssbOpen_MouseHover(object sender, EventArgs e)
        {
            //tssbOpen.ShowDropDown();
        }

        class RecentFileInfo
        {
            public string FilePath { get; set; }

            public RecentFileInfo(string filePath)
            {
                FilePath = filePath;
            }
        }

        private void RebuildRecentFilesMenu()
        {
            for (int i = tssbOpen.DropDownItems.Count - 1; i >= 0; i--)
            {
                if (tssbOpen.DropDownItems[i].Tag is RecentFileInfo)
                {
                    tssbOpen.DropDownItems[i].Click -= RecentFileMenu_Click;
                    tssbOpen.DropDownItems.RemoveAt(i);
                }
            }

            int counter = 0;

            foreach (var filename in AppConfigManager.Current.RecentFiles)
            {
                var fileMenu = tssbOpen.DropDownItems.Add(string.Format("{0}: {1}", ++counter, filename));
                fileMenu.Tag = new RecentFileInfo(filename);
                fileMenu.Click += RecentFileMenu_Click;
            }

            tsSeparatorOpen.Visible = AppConfigManager.Current.RecentFiles.Count > 0;
        }

        private void RecentFileMenu_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripItem tsi && tsi.Tag is RecentFileInfo rfi)
                OpenLayoutFile(rfi.FilePath);
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeUtils.WM_COPYDATA)
            {
                var dataStruct = Marshal.PtrToStructure<NativeUtils.COPYDATASTRUCT>(m.LParam);
                var messageStr = Marshal.PtrToStringUni(dataStruct.lpData, dataStruct.cbData / 2);

                if (messageStr.StartsWith("MUTEX#"))
                    messageStr = messageStr.Substring(6);

                var filesToOpen = new List<string>();

                if (messageStr.Contains(","))
                {
                    var args = messageStr.Split(',').Select(x => x.Replace("&#44;", ",")).ToArray();
                    filesToOpen.AddRange(args);
                }
                else
                    filesToOpen.Add(messageStr);

                BeginInvoke((Action)(() =>
                {
                    ProcessStartArgs(filesToOpen.ToArray());
                }));
                
            }
            base.WndProc(ref m);
        }


        private void ProcessStartArgs(string[] args)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            Activate();
            TopMost = true;
            BringToFront();
            TopMost = false;

            foreach (var fileArg in args)
            {
                if (File.Exists(fileArg))
                    OpenLayoutFile(fileArg, false);
            }
        }
        
    }
}
