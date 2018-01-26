using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class SILayout
    {
        private int _NumberOfStrings;

        public int NumberOfStrings
        {
            get { return _NumberOfStrings; }
            set
            {
                if (value != _NumberOfStrings)
                    InitializeStrings(_NumberOfStrings, value);
            }
        }

        private void InitializeStrings(int oldValue, int newValue)
        {
            _NumberOfStrings = newValue;

        }

        //internal void OnStringConfigChanged(LayoutString sender, string property, StringConfigChangeType changeType)
        //{

        //}

        internal void OnStringConfigChanged(LayoutString sender, string property, bool rebuildLayout = true)
        {

        }

        #region XML serialization

        public static SILayout Load(string path)
        {
            return null;
        }

        public void Save(string path) { }

        #endregion

        #region Enums

        //public enum StringConfigChangeType
        //{
        //    Layout,
        //    Display
        //}

        #endregion
    }
}
