using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SiGen.StringedInstruments.Layout
{
    public class CollectionChange : ILayoutChange
    {
        public string Name { get; set; }

        public IList Collection { get; }

        public LayoutComponent Component => (Collection is ILayoutItemCollection itemCollection ? 
            itemCollection.ParentComponent : null);

        public CollectionChangeAction Action { get; }

        public CollectionItemChangeInfo[] ChangedItems { get; }

        public int CollectionCount { get; set; }

        public bool AffectsLayout { get; set; }

        public CollectionChange(IList collection, CollectionChangeAction action, 
            CollectionItemChangeInfo[] changedItems, bool affectsLayout = true)
        {
            Collection = collection;
            CollectionCount = collection.Count;
            Action = action;
            ChangedItems = changedItems;
            AffectsLayout = affectsLayout;
        }
    }
}
