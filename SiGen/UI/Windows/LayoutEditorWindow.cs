﻿using SiGen.Common;
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

        private LayoutDocument CurrentLayoutDocument
        {
            get
            {
                if ((dockPanel1.ActiveDocument as LayoutViewerPanel) != null)
                    return (dockPanel1.ActiveDocument as LayoutViewerPanel).CurrentFile;
                return null;
            }
        }

        private LayoutViewerPanel ActiveLayoutPanel
        {
            get
            {
                return dockPanel1.ActiveDocument as LayoutViewerPanel;
            }
        }

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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeEditingPanels();
            OpenLayoutFile("DefaultLayout.sil", true);
            AppPreferences.ValidateRecentFiles();
            RebuildRecentFilesMenu();
        }

        #region Document Management

        private void InitializeEditingPanels()
        {
            dockPanel1.DockBottomPortion = 300d / (double)dockPanel1.Height;
            
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
                PreviousDocument.CurrentFile.LayoutChanged -= CurrentFile_LayoutChanged;

            if (ActiveLayoutPanel != null && ActiveLayoutPanel.CurrentFile != null)
            {
                SetEditorsActiveLayout(CurrentLayoutDocument.Layout);
                tsbMeasureTool.Checked = ActiveLayoutPanel.Viewer.EnableMeasureTool;
                tsbMeasureTool.CheckOnClick = true;
                CurrentLayoutDocument.LayoutChanged += CurrentFile_LayoutChanged;
            }
            else
            {
                SetEditorsActiveLayout(null);
                tsbMeasureTool.Checked = false;
                tsbMeasureTool.CheckOnClick = false;
            }
			RebuildUndoRedoMenus();

			PreviousDocument = ActiveLayoutPanel;
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

		private void RefreshCurrentLayoutEditors()
		{
			if(ActiveLayoutPanel != null)
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
                documentPanel.Text = "New Layout";
            //else if (!string.IsNullOrEmpty(layoutFile.Layout.LayoutName))
            //    documentPanel.Text = layoutFile.Layout.LayoutName;
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

            documentPanel.CurrentFile = layoutFile;
            //documentPanel.Viewer.CurrentLayout = layoutFile.Layout;
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
            var documentFile = ((LayoutViewerPanel)sender).CurrentFile;
            foreach (var panel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
                panel.Editor.ClearLayoutCache(documentFile.Layout);
        }

        private void DocumentPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            var panel = (LayoutViewerPanel)sender;
            var documentFile = panel.CurrentFile;
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
        }

        #endregion
        
        /*
        private static SILayout CreateDefaultLayout()
        {
            var layout = new SILayout();
            layout.NumberOfStrings = 6;
            layout.SingleScaleConfig.Length = Measure.Inches(25.5);
            layout.MultiScaleConfig.Treble = Measure.Inches(25.5);
            layout.MultiScaleConfig.Bass = Measure.Inches(27);
            
            layout.SetStringsTuning(
                MusicalNote.EqualNote(NoteName.E, 4),
                MusicalNote.EqualNote(NoteName.B, 3),
                MusicalNote.EqualNote(NoteName.G, 3),
                MusicalNote.EqualNote(NoteName.D, 3),
                MusicalNote.EqualNote(NoteName.A, 2),
                MusicalNote.EqualNote(NoteName.E, 2)
                );
            layout.Strings.MassAssign(s => s.PhysicalProperties,
                new StringProperties(Measure.Inches(0.010), Measure.Inches(0.010), 0.00002215, 29442660.75919),
                new StringProperties(Measure.Inches(0.013), Measure.Inches(0.013), 0.00003744, 29442660.75919),
                new StringProperties(Measure.Inches(0.017), Measure.Inches(0.017), 0.00006402, 29442660.75919),
                new StringProperties(Measure.Inches(0.014), Measure.Inches(0.026), 0.00012671, 29442660.75919),
                new StringProperties(Measure.Inches(0.014), Measure.Inches(0.036), 0.00023964, 29442660.75919),
                new StringProperties(Measure.Inches(0.016), Measure.Inches(0.046), 0.00038216, 29442660.75919)
                );
            layout.Strings.MassAssign(s => s.ActionAtTwelfthFret,
                Measure.Inches(0.063),
                Measure.Inches(0.069),
                Measure.Inches(0.075),
                Measure.Inches(0.082),
                Measure.Inches(0.088),
                Measure.Inches(0.094)
                );

            layout.SimpleStringSpacing.StringSpacingAtNut = Measure.Mm(7.3);
            layout.SimpleStringSpacing.StringSpacingAtBridge = Measure.Mm(10.5);
            layout.SimpleStringSpacing.NutSpacingMode = StringSpacingMethod.BetweenStrings;
            //layout.ScaleLengthMode = ScaleLengthType.Individual;
            layout.Margins.Edges = Measure.Mm(3.25);
            layout.Margins.LastFret = Measure.Mm(10);
            layout.RebuildLayout();
            return layout;
        }*/
        
        #region Save

        public bool SaveLayout(LayoutDocument file, bool selectPath = false)
        {
            bool isNew = string.IsNullOrEmpty(file.FileName);
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
                        sfd.FileName = GenerateDefaultFileName(file);

                    sfd.Filter = "SI Layout Files|*.sil|All Files|*.*";
                    sfd.DefaultExt = ".sil";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        file.FileName = sfd.FileName;
                        file.HasChanged = false;
                    }
                    else
                        return false;
                }
            }

            //if (string.IsNullOrEmpty(file.Layout.LayoutName))
            //    file.Layout.LayoutName = Path.GetFileNameWithoutExtension(file.FileName);

            file.Layout.Save(file.FileName);
            var documentTab = OpenDocuments.FirstOrDefault(d => d.CurrentFile == file);
            if (documentTab != null)
            {
                documentTab.TabText = string.IsNullOrEmpty(file.Layout.LayoutName ) ? Path.GetFileNameWithoutExtension(file.FileName) : file.Layout.LayoutName;
                documentTab.ToolTipText = file.FileName;
            }

            if (isNew)
            {
                AppPreferences.AddRecentFile(file.FileName);
                RebuildRecentFilesMenu();
            }

            return true;
        }

        private string GenerateDefaultFileName(LayoutDocument file)
        {
            var keywords = new List<string>();
            keywords.Add($"{file.Layout.NumberOfStrings} Strings");
            if (file.Layout.ScaleLengthMode == ScaleLengthType.Multiple)
                keywords.Add("Multiscale");
            keywords.Add("Fingerboard");
            keywords.Add("Layout");
            return string.Join(" ", keywords) + ".sil";
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

        private void tssbOpen_ButtonClick(object sender, EventArgs e)
        {
            tssbOpen.HideDropDown();
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "SI Layout file (*.sil)|*.sil";
                if (ofd.ShowDialog() == DialogResult.OK)
                    OpenLayoutFile(ofd.FileName);
            }
        }

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            tssbOpen_ButtonClick(sender, e);
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
                        if((doc as LayoutViewerPanel).CurrentFile.FileName == filename)
                        {
                            (doc as LayoutViewerPanel).Activate();
                            var result = MessageBox.Show(MSG_FileAlreadyOpen, "", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                var layout = LayoutDocument.Open(filename, false);
                                layout.Layout.RebuildLayout();
                                (doc as LayoutViewerPanel).CurrentFile = layout;
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
                    AppPreferences.AddRecentFile(filename);
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
            tssbOpen.ShowDropDown();
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
                if (tssbOpen.DropDownItems[i].Tag is AppPreferences.RecentFile)
                {
                    tssbOpen.DropDownItems[i].Click -= RecentFileMenu_Click;
                    tssbOpen.DropDownItems.RemoveAt(i);
                }
            }

            int counter = 0;

            foreach (var filename in AppPreferences.Current.RecentFiles)
            {
                var fileMenu = tssbOpen.DropDownItems.Add(string.Format("{0}: {1}", ++counter, filename.Filename));
                fileMenu.Tag = filename;
                fileMenu.Click += RecentFileMenu_Click;
            }

            tsSeparatorOpen.Visible = AppPreferences.Current.RecentFiles.Count > 0;
        }

        private void RecentFileMenu_Click(object sender, EventArgs e)
        {
            OpenLayoutFile(((sender as ToolStripItem).Tag as AppPreferences.RecentFile).Filename);
        }

        #endregion

        private void tsbNew_Click(object sender, EventArgs e)
        {
            OpenLayoutFile("DefaultLayout.sil", true);
        }

        private void tssbExport_Click(object sender, EventArgs e)
        {
            using (var exportDialog = new ExportLayoutDialog(CurrentLayoutDocument.Layout))
                exportDialog.ShowDialog();
        }

        private void tsbOptions_Click(object sender, EventArgs e)
        {
            using (var dlg = new AppPreferencesWindow())
                dlg.ShowDialog();
        }

        private void tsbMeasureTool_Click(object sender, EventArgs e)
        {
            if(ActiveLayoutPanel != null)
            {
                ActiveLayoutPanel.Viewer.EnableMeasureTool = tsbMeasureTool.Checked;
            }
        }

		
	}
}