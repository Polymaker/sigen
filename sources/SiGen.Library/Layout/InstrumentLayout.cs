using SiGen.Layout.Editing;
using SiGen.StringedInstruments.Fingerboard;
using SiGen.StringedInstruments.Strings;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout
{
    public class InstrumentLayout
    {
        private int _NumberOfStrings;
		private List<ILayoutChange> ChangesSinceLastBuild;

		public LayoutComponentCollection Components { get; }

		public FingerboardSide FirstStringSide { get; set; }

		public int NumberOfStrings
        {
            get { return _NumberOfStrings; }
			set
			{

			}
        }

		public FingerboardMargins FingerboardMargins { get; private set; }

		public bool IsLoading { get; private set; }

		internal bool IsAssigningProperties { get; private set; }

		public event EventHandler<LayoutChangedEventArgs> LayoutChanged;

		public InstrumentLayout()
		{
			Components = new LayoutComponentCollection(this);
			ChangesSinceLastBuild = new List<ILayoutChange>();

			InitializeCoreComponents();
		}

		#region Loading & Intialization

		private void InitializeCoreComponents()
		{
			//using (new TempStateObject(() => IsLoading = true, () => IsLoading = false))
			//{
			//	FingerboardMargins = new FingerboardMargins(this);

			//}
		}

		#endregion

		#region MyRegion

		#endregion

		#region Layout Change Tracking & Notification

		private List<PropertyChange> CurrentBatchChanges;
		private int NestedBatches;

		internal void NotifyLayoutChanged(ILayoutChange change)
		{
			if (!(IsLoading || IsAssigningProperties))
			{
				if (change is PropertyChange propChange && CurrentBatchChanges != null)
					CurrentBatchChanges.Add(propChange);
				else
					OnLayoutChanged(new LayoutChangedEventArgs(change));
			}
		}

		protected void OnLayoutChanged(LayoutChangedEventArgs args)
		{
			ChangesSinceLastBuild.Add(args.Change);
			LayoutChanged?.Invoke(this, args);
		}

		internal void StartBatchChanges()
		{
			if (!(IsLoading || IsAssigningProperties))
			{
				if (CurrentBatchChanges == null)
					CurrentBatchChanges = new List<PropertyChange>();
				NestedBatches++;
			}

		}

		internal void FinishBatchChanges()
		{
			if (!(IsLoading || IsAssigningProperties) && CurrentBatchChanges != null)
			{
				if (--NestedBatches == 0)
				{
					if (CurrentBatchChanges.Count == 1)
						OnLayoutChanged(new LayoutChangedEventArgs(CurrentBatchChanges[0]));
					else
						OnLayoutChanged(new LayoutChangedEventArgs(new BatchChange(CurrentBatchChanges)));
					CurrentBatchChanges = null;
				}
			}
		}

		#endregion


	}
}
