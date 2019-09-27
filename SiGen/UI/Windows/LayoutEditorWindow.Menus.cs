using SiGen.Common;
using SiGen.StringedInstruments.Layout;
using SiGen.UI.Controls;
using SiGen.UI.Windows;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SiGen.UI
{
	public partial class LayoutEditorWindow
	{
		#region Undo/Redo

		private void RebuildUndoRedoMenus()
		{
			tssbUndo.DropDownItems.Clear();
			tssbRedo.DropDownItems.Clear();
			tssbUndo.Enabled = CurrentLayoutDocument?.CanUndo() ?? false;
			tssbRedo.Enabled = CurrentLayoutDocument?.CanRedo() ?? false;

			if (CurrentLayoutDocument != null)
			{
				int actionIndex = 0;
				foreach(var undoAction in CurrentLayoutDocument.GetUndoList().Take(5))
				{
					var undoActionBtn = tssbUndo.DropDownItems.Add(GetActionName(undoAction));
					undoActionBtn.Tag = actionIndex++;
				}
				actionIndex = 0;
				foreach (var redoAction in CurrentLayoutDocument.GetRedoList().Take(5))
				{
					tssbRedo.DropDownItems.Add(GetActionName(redoAction)).Tag = actionIndex++;
				}
			}
		}

		private string GetActionName(ILayoutChange action)
		{
			if(action is BatchChange bc)
			{
                if(bc.ChangedProperties.Any(p => p.Property == nameof(SILayout.NumberOfStrings)))
                {
                    action = bc.ChangedProperties.First(p => p.Property == nameof(SILayout.NumberOfStrings));
                }
				if(bc.Component != null)
                {
                    switch (bc.Component)
                    {
                        case FingerboardMargin fm:
                            switch (bc.Name)
                            {
                                case nameof(FingerboardMargin.Bass):
                                    return $"Bass margin: {bc.ChangedProperties.First().NewValue}";
                                case nameof(FingerboardMargin.Treble):
                                    return $"Treble margin: {bc.ChangedProperties.First().NewValue}";
                                case nameof(FingerboardMargin.MarginAtNut):
                                    return $"Margin at nut: {bc.ChangedProperties.First().NewValue}";
                                case nameof(FingerboardMargin.MarginAtBridge):
                                    return $"Margin at bridge: {bc.ChangedProperties.First().NewValue}";
                                case nameof(FingerboardMargin.Edges):
                                    return $"Edges margin: {bc.ChangedProperties.First().NewValue}";

                            }
                            break;
                    }
                }

			}

			if(action is PropertyChange pc)
			{
				if(pc.Component != null)
				{
					switch (pc.Component)
					{
						case ScaleLengthManager slm:
							{
								var scaleEditor = GetLayoutEditor<ScaleLengthEditor>();
								switch (pc.Property)
								{
									case nameof(ScaleLengthManager.SingleScale.Length):
										return $"Scale length: {pc.NewValue}";
									case nameof(ScaleLengthManager.MultiScale.Bass):
									case nameof(ScaleLengthManager.MultiScale.Treble):
										return $"{pc.Property} scale length: {pc.NewValue}";
									case nameof(ScaleLengthManager.MultiScale.PerpendicularFretRatio):
										return $"Perpendicular fret: {scaleEditor.GetPerpendicularFretName((double)pc.NewValue)}";
									default:
										return $"Scale length {pc.Property}: {pc.NewValue}";
								}
							}
						case StringSpacingManager ssm:
							{
								switch (pc.Property)
								{
									default:
										return $"String spacing {pc.Property}: {pc.NewValue}";
								}
							}
						case SIString ss:
							{
								switch (pc.Property)
								{
									default:
										return $"String {pc.Property}: {pc.NewValue}";
								}
							}
					}
				}
				else
				{
					switch (pc.Property)
					{
						case nameof(SILayout.ScaleLengthMode):
							return $"Scale length mode: {pc.NewValue}";
						case nameof(SILayout.StringSpacingMode):
							return $"String spacing mode: {pc.NewValue}";
						case nameof(SILayout.NumberOfStrings):
							return $"Number of strings: {pc.NewValue}";
					}
				}
			}
			return action.GetChanges()[0].Property;
		}

		private void tssbUndo_ButtonClick(object sender, EventArgs e)
		{
			if (ActiveDocument != null && ActiveDocument.CurrentDocument.Undo())
			{
				RefreshCurrentLayoutEditors();
				if (ActiveDocument.CurrentDocument.Layout.IsLayoutDirty)
					ActiveDocument.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		private void tssbRedo_ButtonClick(object sender, EventArgs e)
		{
			if (ActiveDocument != null && ActiveDocument.CurrentDocument.Redo())
			{
				RefreshCurrentLayoutEditors();
				if (ActiveDocument.CurrentDocument.Layout.IsLayoutDirty)
					ActiveDocument.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		private void tssbUndo_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Tag is int actionIndex && ActiveDocument != null)
			{
				for (int i = 0; i <= actionIndex; i++)
					ActiveDocument.CurrentDocument.Undo();
				RefreshCurrentLayoutEditors();
				if (ActiveDocument.CurrentDocument.Layout.IsLayoutDirty)
					ActiveDocument.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		private void tssbRedo_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Tag is int actionIndex && ActiveDocument != null)
			{
				for (int i = 0; i <= actionIndex; i++)
					ActiveDocument.CurrentDocument.Redo();
				RefreshCurrentLayoutEditors();
				if (ActiveDocument.CurrentDocument.Layout.IsLayoutDirty)
					ActiveDocument.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

        #endregion


        private void tsbNew_Click(object sender, EventArgs e)
        {
            OpenDefaultLayout();
        }

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

        private void tsbExport_Click(object sender, EventArgs e)
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
            if (ActiveDocument != null)
            {
                ActiveDocument.Viewer.EnableMeasureTool = tsbMeasureTool.Checked;
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
            {
                ActiveDocument.Close();
            }
        }
    }
}