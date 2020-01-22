using SiGen.Common;
using SiGen.Localization;
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
            return LayoutChangeTranslator.GetChangeDescription(CurrentLayoutDocument.Layout, action);
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
				ofd.Filter = $"{Localizations.Misc_LayoutFileDesc}|*.sil|{Localizations.Misc_AllFilesDesc}|*.*";

				if (ofd.ShowDialog() == DialogResult.OK)
                    OpenLayoutFile(ofd.FileName);
            }
        }

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            tssbOpen_ButtonClick(sender, e);
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

		private void tsbExport_Click(object sender, EventArgs e)
        {
            using (var exportDialog = new ExportLayoutDialog(CurrentLayoutDocument))
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