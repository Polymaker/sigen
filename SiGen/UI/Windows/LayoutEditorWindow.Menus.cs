using SiGen.StringedInstruments.Layout;
using SiGen.UI.Controls;
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
			if (ActiveLayoutPanel != null && ActiveLayoutPanel.CurrentDocument.Undo())
			{
				RefreshCurrentLayoutEditors();
				if (ActiveLayoutPanel.CurrentDocument.Layout.IsLayoutDirty)
					ActiveLayoutPanel.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		private void tssbRedo_ButtonClick(object sender, EventArgs e)
		{
			if (ActiveLayoutPanel != null && ActiveLayoutPanel.CurrentDocument.Redo())
			{
				RefreshCurrentLayoutEditors();
				if (ActiveLayoutPanel.CurrentDocument.Layout.IsLayoutDirty)
					ActiveLayoutPanel.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		private void tssbUndo_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Tag is int actionIndex && ActiveLayoutPanel != null)
			{
				for (int i = 0; i <= actionIndex; i++)
					ActiveLayoutPanel.CurrentDocument.Undo();
				RefreshCurrentLayoutEditors();
				if (ActiveLayoutPanel.CurrentDocument.Layout.IsLayoutDirty)
					ActiveLayoutPanel.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		private void tssbRedo_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Tag is int actionIndex && ActiveLayoutPanel != null)
			{
				for (int i = 0; i <= actionIndex; i++)
					ActiveLayoutPanel.CurrentDocument.Redo();
				RefreshCurrentLayoutEditors();
				if (ActiveLayoutPanel.CurrentDocument.Layout.IsLayoutDirty)
					ActiveLayoutPanel.CurrentDocument.Layout.RebuildLayout();
				RebuildUndoRedoMenus();
			}
		}

		#endregion
	}
}