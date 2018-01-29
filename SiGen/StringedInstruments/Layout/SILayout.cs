using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public class SILayout
    {
        #region Fields

        private Temperament _FretsTemperament;
        private bool _LeftHanded;
        private int _NumberOfStrings;
        private SIString[] _Strings;
        private bool _CompensateFretPositions;
        private IStringsSpacing _StringSpacing;
        private ScaleLengthMode _ScaleLengthMode;
        private ScaleLengthManager.SingleScale _SingleScaleMgr;
        private ScaleLengthManager.MultiScale _MultiScaleMgr;
        private ScaleLengthManager.Individual _ManualScaleMgr;
        private List<VisualElement> _VisualElements;

        #endregion

        public int NumberOfStrings
        {
            get { return _NumberOfStrings; }
            set
            {
                if (value != _NumberOfStrings)
                    InitializeStrings(_NumberOfStrings, value);
            }
        }

        public SIString[] Strings { get { return _Strings; } }

        public string LayoutName { get; set; }

        #region Scale length management

        public ScaleLengthMode ScaleLengthMode
        {
            get { return _ScaleLengthMode; }
            set
            {
                if (value != _ScaleLengthMode)
                {
                    _ScaleLengthMode = value;
                    NotifyLayoutChanged(this, "ScaleLengthMode");
                }
            }
        }

        public ScaleLengthManager CurrentScaleLength
        {
            get
            {
                switch (ScaleLengthMode)
                {
                    default:
                    case ScaleLengthMode.Single:
                        return _SingleScaleMgr;
                    case ScaleLengthMode.Multiple:
                        return _ManualScaleMgr;
                    case ScaleLengthMode.Individual:
                        return _ManualScaleMgr;
                }
            }
        }

        #endregion

        public Temperament FretsTemperament
        {
            get { return _FretsTemperament; }
            set
            {
                if (value != _FretsTemperament)
                {
                    _FretsTemperament = value;
                    //AdjustStringsTuning();
                    NotifyLayoutChanged(this, "FretsTemperament");
                }
            }
        }

        public IStringsSpacing StringSpacing
        {
            get { return _StringSpacing; }
            set
            {
                if (value != _StringSpacing)
                {
                    _StringSpacing = value;
                    NotifyLayoutChanged(this, "StringSpacing");
                }
            }
        }

        public bool LeftHanded
        {
            get { return _LeftHanded; }
            set
            {
                if (value != _LeftHanded)
                {
                    _LeftHanded = value;
                    NotifyLayoutChanged(this, "LeftHanded");
                }
            }
        }

        public List<VisualElement> VisualElements { get { return _VisualElements; } }

        #region Events

        public event EventHandler NumberOfStringsChanged;

        #endregion

        public SILayout()
        {
            _StringSpacing = new StringsSpacingSimple(this);
            _SingleScaleMgr = new ScaleLengthManager.SingleScale(this);
            _MultiScaleMgr = new ScaleLengthManager.MultiScale(this);
            _ManualScaleMgr = new ScaleLengthManager.Individual(this);
            _VisualElements = new List<VisualElement>();
            _ScaleLengthMode = ScaleLengthMode.Single;
        }

        private void InitializeStrings(int oldValue, int newValue)
        {
            _NumberOfStrings = newValue;
            var oldStrings = _Strings;
            _Strings = new SIString[NumberOfStrings];
            for(int i = 0; i < _NumberOfStrings; i++)
            {
                if (oldStrings != null && i < oldStrings.Length - 1)
                    _Strings[i] = oldStrings[i];
                else
                    _Strings[i] = new SIString(this, i);
            }
            if (NumberOfStringsChanged != null)
                NumberOfStringsChanged(this, EventArgs.Empty);
        }

        internal void NotifyLayoutChanged(object sender, string propname)
        {

        }

        public bool VerifyFretboardHasStraightFrets()
        {
            if (FretsTemperament != Temperament.Equal || _CompensateFretPositions)
                return false;

            if (!Strings.AllEqual(s => s.RelativeScaleLengthOffset))
                return false;
            if (Strings.Length > 2 && !Strings.AllEqual(s => s.ScaleLength))
            {
                var diff = Measure.Abs(Strings[0].ScaleLength - Strings[1].ScaleLength);
                for (int i = 1; i < NumberOfStrings - 1; i++)
                {
                    var scaleDiff = Measure.Abs(Strings[i].ScaleLength - Strings[i + 1].ScaleLength);
                    if (!Measure.EqualOrClose(diff, scaleDiff, Measure.Mm(0.0001)))
                        return false;
                }
            }
            return true;
        }

        #region XML serialization

        public static SILayout Load(string path)
        {
            return null;
        }

        public void Save(string path)
        {
            var configElem = new XElement("Config");
            var stringsElem = new XElement("Strings");
            var spacingElem = new XElement("Spacings");

            var rootElem = new XElement("SILayout", new XAttribute("Name", LayoutName), 
                configElem, stringsElem, spacingElem);

            configElem.Add(new XElement("Temperament", new XAttribute("Value", FretsTemperament)));
            configElem.Add(new XElement("LeftHanded", new XAttribute("Value", LeftHanded)));

            foreach (var str in Strings)
                stringsElem.Add(str.Serialize());

            for(int i = 0; i < NumberOfStrings - 1; i++)
            {
                spacingElem.Add(new XElement("Spacing", 
                    new XAttribute("Index", i), 
                    _StringSpacing.GetSpacing(i, true).SerializeAsAttribute("Nut"),
                    _StringSpacing.GetSpacing(i, false).SerializeAsAttribute("Bridge")
                    ));
            }

            var doc = new XDocument(rootElem);

            doc.Save(path);
        }

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
