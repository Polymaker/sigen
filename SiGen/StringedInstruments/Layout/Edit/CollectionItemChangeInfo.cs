using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class CollectionItemChangeInfo
    {
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
        public object Item { get; set; }

        public CollectionItemChangeInfo(object item, int oldIndex, int newIndex)
        {
            Item = item;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
    }
}
