using SiGen.Common;
using SiGen.Resources;
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

        private void RefreshToolbarButtonStates()
        {
            tsbClose.Enabled = CurrentLayoutDocument != null;
            tsbExport.Enabled = CurrentLayoutDocument != null;
            tssbSave.Enabled = CurrentLayoutDocument != null;

        }

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
                if (bc.Component != null)
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

                switch (bc.Name)
                {
                    case "NumberOfFrets":
                        {
                            var propChange = bc.ChangedProperties
                                .FirstOrDefault(x => x.Property == "NumberOfFrets");

                            if (propChange != null && propChange.NewValue is int fretCount)
                            {
                                return $"{Localizations.LayoutProperty_NumberOfFrets}: {fretCount}";
                            }

                            return Localizations.LayoutProperty_NumberOfFrets;
                        }
                }
			}

            if (action is PropertyChange pc)
            {
                if (pc.Component != null)
                {
                    switch (pc.Component)
                    {
                        case ScaleLengthManager slm:
                            {
                                var scaleEditor = GetLayoutEditor<ScaleLengthEditor>();
                                switch (pc.Property)
                                {
                                    case nameof(SingleScaleManager.Length):
                                        return $"{Localizations.Words_ScaleLength}: {pc.NewValue}";
                                    case nameof(DualScaleManager.Bass):
                                        return $"{Localizations.Words_ScaleLength} ({Localizations.FingerboardSide_Bass}): {pc.NewValue}";
                                    case nameof(DualScaleManager.Treble):
                                        return $"{Localizations.Words_ScaleLength} ({Localizations.FingerboardSide_Treble}): {pc.NewValue}";
                                    case nameof(DualScaleManager.PerpendicularFretRatio):
                                        return $"{Localizations.Words_PerpendicularFret}: {scaleEditor.GetPerpendicularFretName((double)pc.NewValue)}";
                                    default:
                                        return $"{Localizations.Words_ScaleLength} {pc.Property}: {pc.NewValue}";
                                }
                            }
                        case StringSpacingManager ssm:
                            {
                                switch (pc.Property)
                                {
                                    case nameof(StringSpacingManager.BridgeAlignment):
                                        return $"{Localizations.StringSpacingProperty_BridgeAlignment}";
                                    case nameof(StringSpacingManager.NutAlignment):
                                        return $"{Localizations.StringSpacingProperty_NutAlignment}";

                                    case nameof(StringSpacingSimple.BridgeSpacingMode):
                                    case nameof(StringSpacingSimple.NutSpacingMode):

                                        {
                                            string propDesc = Localizations.StringSpacingProperty_SpacingMode;
                                            
                                            string modeDesc = GetLocText($"StringSpacingMethod_{pc.NewValue}");

                                            int charIdx = modeDesc.IndexOf('(');
                                            if (charIdx > 0)
                                                modeDesc = modeDesc.Substring(0, charIdx).Trim();

                                            string sideDesc = pc.Property == nameof(StringSpacingSimple.BridgeSpacingMode) ?
                                                Localizations.FingerboardEnd_Bridge : Localizations.FingerboardEnd_Nut;

                                            return $"{propDesc} ({sideDesc}): {modeDesc}";
                                        }

                                    case nameof(StringSpacingSimple.StringSpacingAtBridge):
                                    case nameof(StringSpacingSimple.StringSpacingAtNut):
                                        {
                                            string propDesc = Localizations.StringSpacingProperty_Spacing;
                                            string sideDesc = pc.Property == nameof(StringSpacingSimple.StringSpacingAtBridge) ?
                                                Localizations.FingerboardEnd_Bridge : Localizations.FingerboardEnd_Nut;
                                            
                                            return $"{propDesc} ({sideDesc}): {pc.NewValue}";
                                        }

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
                            {
                                var modeStr = GetLocText($"ScaleLengthType_{pc.NewValue}");
                                return $"{Localizations.LayoutProperty_ScaleLengthMode}: {modeStr}";
                            }
                        case nameof(SILayout.StringSpacingMode):
                            {
                                var modeStr = GetLocText($"StringSpacingType_{pc.NewValue}");
                                return $"{Localizations.LayoutProperty_StringSpacingMode}: {modeStr}";
                            }
                        //case nameof(SILayout.NumberOfStrings):
                        //    return $"Number of strings: {pc.NewValue}";
                    }
                }
            }

            if (action is CollectionChange cc)
            {
                var currentLayout = CurrentLayoutDocument.Layout;
                if (cc.Collection is LayoutItemCollection<SIString> layoutStrings)
                    return $"{Localizations.LayoutProperty_NumberOfStrings}: {cc.CollectionCount}";
            }

            return string.IsNullOrEmpty(action.Name) ? "Unnamed change" : action.Name;
		}

        private static string GetLocText(string textID)
        {
            return Localizations.ResourceManager.GetString(textID);
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

        private void tsbClose_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
            {
                ActiveDocument.Close();
            }
        }
    }
}