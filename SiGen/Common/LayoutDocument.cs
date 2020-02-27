using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Common
{
    public class LayoutDocument
    {
        public bool HasChanged { get; set; }

        public SILayout Layout { get; private set; }

        public string FilePath { get; set; }

		public string DocumentName { get; set; }

		public bool IsNew => string.IsNullOrEmpty(FilePath);

		public List<ILayoutChange> ModificationList { get; } = new List<ILayoutChange>();

		public int CurrentActionIndex { get; private set; }

		private bool IsUndoing { get; set; }

        public event EventHandler LayoutChanged;

        public LayoutDocument(SILayout layout)
        {
            Layout = layout;
            FilePath = string.Empty;
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

            LayoutChanged?.Invoke(this, EventArgs.Empty);
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

		public IEnumerable<ILayoutChange> GetUndoList()
		{
			if (CurrentActionIndex >= 0 && ModificationList.Count > 0)
				return ModificationList.Take(CurrentActionIndex + 1).Reverse();
			return new ILayoutChange[0];
		}

		public IEnumerable<ILayoutChange> GetRedoList()
		{
			if (ModificationList.Count > 0)
				return ModificationList.Skip(CurrentActionIndex + 1);
			return new ILayoutChange[0];
		}

		public static LayoutDocument Open(string filename, bool asTemplate = false)
        {
            var layout = SILayout.Load(filename);
            var file = new LayoutDocument(layout);

			if (!asTemplate)
			{
				file.FilePath = filename;
				file.DocumentName = Path.GetFileNameWithoutExtension(filename);
			}
			else if (!string.IsNullOrEmpty(layout.LayoutName))
				file.DocumentName = layout.LayoutName;

			return file;
        }

		public void Save(string filepath)
		{
			Layout.Save(filepath);
			FilePath = filepath;
			HasChanged = false;
			DocumentName = Path.GetFileNameWithoutExtension(filepath);
		}

        public static string GenerateLayoutName(SILayout layout)
        {
            var keywords = new List<string>();
            keywords.Add($"{layout.NumberOfStrings} Strings");
            if (layout.ScaleLengthMode == ScaleLengthType.Dual)
                keywords.Add("Multiscale");
            keywords.Add("Fingerboard");
            keywords.Add("Layout");
            return string.Join(" ", keywords);
        }
    }
}
