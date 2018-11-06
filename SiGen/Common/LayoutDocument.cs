﻿using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Common
{
    public class LayoutDocument
    {
        public bool HasChanged { get; set; }
        public SILayout Layout { get; private set; }
        public string FileName { get; set; }

		public List<ILayoutChange> ModificationList { get; } = new List<ILayoutChange>();
		public int CurrentActionIndex { get; private set; }
		private bool IsUndoing { get; set; }

        public LayoutDocument(SILayout layout)
        {
            Layout = layout;
            FileName = string.Empty;
			Layout.LayoutChanged += Layout_LayoutChanged;
			CurrentActionIndex = -1;
		}

		private void Layout_LayoutChanged(object sender, LayoutChangedEventArgs e)
		{
			HasChanged = true;

			if (!IsUndoing)
			{
				if (CurrentActionIndex < ModificationList.Count - 1)
				{
					ModificationList.RemoveRange(CurrentActionIndex + 1, ModificationList.Count - CurrentActionIndex - 1);
				}
				CurrentActionIndex++;
				ModificationList.Add(e.Change);
			}
		}

		public bool Undo()
		{
			if(CanUndo())
			{
				IsUndoing = true;
				Layout.UndoChange(ModificationList[CurrentActionIndex--]);
				IsUndoing = false;
				return true;
			}
			return false;
		}

		public bool CanUndo()
		{
			return ModificationList.Count > 0 && CurrentActionIndex >= 0;
		}

		public bool Redo()
		{
			if (CanRedo())
			{
				IsUndoing = true;
				Layout.RedoChange(ModificationList[++CurrentActionIndex]);
				IsUndoing = false;
				return true;
			}
			return false;
		}

		public bool CanRedo()
		{
			return ModificationList.Count > 0 && CurrentActionIndex < ModificationList.Count - 1;
		}

        public static LayoutDocument Open(string filename, bool asTemplate = false)
        {
            var layout = SILayout.Load(filename);
            var file = new LayoutDocument(layout);
            if (!asTemplate)
                file.FileName = filename;
            return file;
        }

        public static LayoutDocument OpenTemplate(string filename)
        {
            var layout = SILayout.Load(filename);
            return new LayoutDocument(layout) { FileName = string.Empty };
        }
    }
}
