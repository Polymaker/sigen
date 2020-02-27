using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
    public class FlagList
    {
        private Dictionary<string, bool> mFlags;

        public bool this[string flagName]
        {
            get
            {
                if (!mFlags.ContainsKey(flagName))
                    return false;
                return mFlags[flagName];
            }
            set
            {
                Set(flagName, value);
            }
        }

        public bool Any { get { return mFlags.Values.Any(v => v == true); } }

        public bool All { get { return mFlags.Values.All(v => v == true); } }

        public IReadOnlyDictionary<bool, string> Flags
        {
            get { return (IReadOnlyDictionary<bool, string>)mFlags; }
        }

        public FlagList()
        {
            mFlags = new Dictionary<string, bool>();
        }

        public void Toggle(string flagName)
        {
            if (!mFlags.ContainsKey(flagName))
                mFlags.Add(flagName, true);
            else
                mFlags[flagName] = !mFlags[flagName];
        }

        public void Set(string flagName, bool value)
        {
            if (!mFlags.ContainsKey(flagName))
                mFlags.Add(flagName, value);
            else
                mFlags[flagName] = value;
        }

        public bool IsSet(string flagName)
        {
            if (mFlags.ContainsKey(flagName))
                return mFlags[flagName];
            return false;
        }

        public IDisposable UseFlag(string name)
        {
            Set(name, true);
            return new TempFlag(this, name);
        }

        private class TempFlag : IDisposable
        {
            private FlagList Manager;
            private string FlagName;

            public TempFlag(FlagList manager, string flagName)
            {
                Manager = manager;
                FlagName = flagName;
            }

            public void Dispose()
            {
                Manager.Set(FlagName, false);
            }
        }
    }
}
