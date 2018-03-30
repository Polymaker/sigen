using SiGen.Export;
using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout;
using SiGen.UI.Controls;
using SiGen.UI.Controls.LayoutEditors;
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
    public partial class FrmLayoutBuilder : Form
    {
        private LayoutEditorPanel<StringSpacingEditor> stringSpacingPanel;
        private LayoutEditorPanel<StringsConfigurationEditor> stringConfigPanel;
        private LayoutEditorPanel<FingerboardMarginEditor> layoutMarginPanel;
        private LayoutEditorPanel<ScaleLengthEditor> scaleLengthPanel;

        private LayoutFile CurrentFile
        {
            get
            {
                if(dockPanel1.ActiveDocument != null)
                    return (LayoutFile)(dockPanel1.ActiveDocument as DockContent).Tag;
                return null;
            }
        }

        public FrmLayoutBuilder()
        {
            InitializeComponent();
            //dockPanel1.Theme = vS2005Theme1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            //splitContainer1.Panel1.Controls.Remove(layoutViewer1);
            InitializeEditingPanels();
            
            LoadLayout(new LayoutFile(CreateDefaultLayout()));
        }

        #region Document Management

        private void InitializeEditingPanels()
        {
            dockPanel1.DockBottomPortion = 200d / (double)Height;

            stringConfigPanel = new LayoutEditorPanel<StringsConfigurationEditor>();
            stringConfigPanel.Show(dockPanel1, DockState.DockBottom);
            stringConfigPanel.Text = "Strings Configuration";

            stringSpacingPanel = new LayoutEditorPanel<StringSpacingEditor>();
            stringSpacingPanel.Show(stringConfigPanel.Pane, DockAlignment.Right, 0.80);
            stringSpacingPanel.Text = "String Spacing";

            scaleLengthPanel = new LayoutEditorPanel<ScaleLengthEditor>();
            scaleLengthPanel.Show(stringSpacingPanel.Pane, DockAlignment.Right, 0.66);
            scaleLengthPanel.Text = "Scale Length Configuration";

            layoutMarginPanel = new LayoutEditorPanel<FingerboardMarginEditor>();
            layoutMarginPanel.Show(scaleLengthPanel.Pane, DockAlignment.Right, .5);
            layoutMarginPanel.Text = "Fingerboard Margins";
        }

        private void dockPanel1_ActiveDocumentChanged(object sender, EventArgs e)
        {
            foreach(var panel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
            {
                if (dockPanel1.ActiveDocument == null)
                    panel.CurrentLayout = null;
                else
                    panel.CurrentLayout = (((DockContent)dockPanel1.ActiveDocument).Tag as LayoutFile).Layout;
            }
        }

        private DockContent CreateDocumentPanel(LayoutFile layoutFile)
        {
            var documentPanel = new DockContent();
            if (string.IsNullOrEmpty(layoutFile.FileName))
                documentPanel.Text = "New Layout";
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
                    num++;
                    documentPanel.Text = origName + " " + num;
                }
            }

            if (!string.IsNullOrEmpty(layoutFile.FileName))
                documentPanel.ToolTipText = layoutFile.FileName;

            documentPanel.DockAreas = DockAreas.Document;// | DockAreas.Float;
            var viewer = new LayoutViewer();
            documentPanel.Controls.Add(viewer);
            viewer.Dock = DockStyle.Fill;
            if (layoutFile.Layout.VisualElements.Count == 0 || layoutFile.Layout.IsLayoutDirty)
                layoutFile.Layout.RebuildLayout();
            viewer.CurrentLayout = layoutFile.Layout;
            viewer.BackColor = Color.White;
            viewer.Font = new Font(Font.FontFamily, Font.Size + 3);
            viewer.DisplayConfig.RenderRealStrings = true;
            viewer.Select();
            documentPanel.Tag = layoutFile;
            documentPanel.FormClosing += DocumentPanel_FormClosing;
            documentPanel.FormClosed += DocumentPanel_FormClosed;
            return documentPanel;
        }

        private void DocumentPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            var documentFile = (LayoutFile)((DockContent)dockPanel1.ActiveDocument).Tag;
            foreach (var panel in dockPanel1.Contents.OfType<ILayoutEditorPanel>())
                panel.Editor.ClearLayoutCache(documentFile.Layout);
        }

        private void DocumentPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        #endregion

        private DockContent CreateDockPanel(Control ctrl, string name)
        {
            var tmpDoc = new DockContent();
            tmpDoc.CloseButtonVisible = false;
            tmpDoc.AllowEndUserDocking = false;
            tmpDoc.Text = name;
            tmpDoc.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
            return tmpDoc;
        }

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
            layout.SimpleStringSpacing.NutSpacingMode = NutSpacingMode.BetweenStrings;
            //layout.ScaleLengthMode = ScaleLengthType.Individual;
            layout.Margins.Edges = Measure.Mm(3.25);
            layout.Margins.LastFret = Measure.Mm(10);
            layout.RebuildLayout();
            return layout;
        }
        
        #region Save

        private void SaveLayout(LayoutFile file, bool selectPath)
        {
            if (selectPath)
            {
                using (var sfd = new SaveFileDialog())
                {
                    if(!string.IsNullOrEmpty(file.FileName))
                        sfd.FileName = file.FileName;
                    else
                        sfd.FileName = "test.sil";
                    sfd.Filter = "SI Layout file (*.sil)|*.sil";
                    sfd.DefaultExt = ".sil";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        file.FileName = sfd.FileName;
                    }
                    else
                        return;
                }
            }

            if (string.IsNullOrEmpty(file.Layout.LayoutName))
                file.Layout.LayoutName = Path.GetFileNameWithoutExtension(file.FileName);
            file.Layout.Save(file.FileName);
        }

        private void tssbSave_ButtonClick(object sender, EventArgs e)
        {
            if (CurrentFile != null)
                SaveLayout(CurrentFile, string.IsNullOrEmpty(CurrentFile.FileName));
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
                SaveLayout(CurrentFile, string.IsNullOrEmpty(CurrentFile.FileName));
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
                SaveLayout(CurrentFile, true);
        }

        #endregion

        private void tsbNew_Click(object sender, EventArgs e)
        {
            LoadLayout(new LayoutFile(CreateDefaultLayout()));
        }

        private void tssbOpen_ButtonClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "SI Layout file (*.sil)|*.sil";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var layoutToOpen = new LayoutFile(ofd.FileName);
                        LoadLayout(layoutToOpen);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("An error occured: " + ex.ToString());
                    }
                }
            }
        }

        private class LayoutFile
        {
            public SILayout Layout { get; set; }
            public string FileName { get; set; }

            public LayoutFile(SILayout layout)
            {
                Layout = layout;
                FileName = string.Empty;
            }

            public LayoutFile(string filename)
            {
                FileName = filename;
                Layout = SILayout.Load(filename);
            }
        }

        private void LoadLayout(LayoutFile layout)
        {
            var documentPanel = CreateDocumentPanel(layout);
            documentPanel.Show(dockPanel1, DockState.Document);
        }

        private void exportAsSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(CurrentFile.Layout.LayoutName))
                    sfd.FileName = CurrentFile.Layout.LayoutName + ".svg";
                else
                    sfd.FileName = "layout.svg";

                sfd.Filter = "Scalable Vector Graphics File (*.svg)|*.svg";
                sfd.DefaultExt = ".svg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SvgLayoutExporter.ExportLayout(sfd.FileName, CurrentFile.Layout,
                        new LayoutSvgExportOptions()
                        {
                            ExportStrings = false,
                            ExportStringCenters = false
                        });
                }
            }
        }

        
    }
}
