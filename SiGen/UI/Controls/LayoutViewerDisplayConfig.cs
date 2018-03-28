using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public class LayoutViewerDisplayConfig : INotifyPropertyChanged
    {
        private bool _ShowStrings;
        private bool _ShowFrets;
        private bool _ShowMidlines;
        private bool _ShowTheoreticalFrets;
        private bool _RenderRealStrings;
        private bool _RenderRealFrets;
        private UnitOfMeasure _DefaultDisplayUnit;
        private Measure _FretWidth;
        private Orientation _FretboardOrientation;

        [DefaultValue(true)]
        public bool ShowStrings
        {
            get { return _ShowStrings; }
            set
            {
                if (value != _ShowStrings)
                {
                    _ShowStrings = value;
                    OnPropertyChanged("ShowStrings");
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowFrets
        {
            get { return _ShowFrets; }
            set
            {
                if (value != _ShowFrets)
                {
                    _ShowFrets = value;
                    OnPropertyChanged("ShowFrets");
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowTheoreticalFrets
        {
            get { return _ShowTheoreticalFrets; }
            set
            {
                if (value != _ShowTheoreticalFrets)
                {
                    _ShowTheoreticalFrets = value;
                    OnPropertyChanged("ShowTheoreticalFrets");
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowMidlines
        {
            get { return _ShowMidlines; }
            set
            {
                if (value != _ShowMidlines)
                {
                    _ShowMidlines = value;
                    OnPropertyChanged("ShowMidlines");
                }
            }
        }

        [DefaultValue(Orientation.Horizontal)]
        public Orientation FretboardOrientation
        {
            get { return _FretboardOrientation; }
            set
            {
                if (value != _FretboardOrientation)
                {
                    _FretboardOrientation = value;
                    OnPropertyChanged("FretboardOrientation");
                }
            }
        }

        [DefaultValue(true)]
        public bool RenderRealFrets
        {
            get { return _RenderRealFrets; }
            set
            {
                if (value != _RenderRealFrets)
                {
                    _RenderRealFrets = value;
                    OnPropertyChanged("RenderRealFrets");
                }
            }
        }

        [DefaultValue(false)]
        public bool RenderRealStrings
        {
            get { return _RenderRealStrings; }
            set
            {
                if (value != _RenderRealStrings)
                {
                    _RenderRealStrings = value;
                    OnPropertyChanged("RenderRealStrings");
                }
            }
        }

        [EditorAttribute(typeof(Designers.MeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Measure FretWidth
        {
            get { return _FretWidth; }
            set
            {
                if (!value.IsEmpty && (value != _FretWidth || _FretWidth.Unit != value.Unit))
                {
                    _FretWidth = value;
                    OnPropertyChanged("FretWidth");
                }
            }
        }

        [EditorAttribute(typeof(Designers.UnitOfMeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public UnitOfMeasure DefaultDisplayUnit
        {
            get { return _DefaultDisplayUnit; }
            set
            {
                if (value != _DefaultDisplayUnit)
                {
                    _DefaultDisplayUnit = value;
                    OnPropertyChanged("DefaultDisplayUnit");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LayoutViewerDisplayConfig()
        {
            _ShowStrings = true;
            _ShowFrets = true;
            _ShowTheoreticalFrets = false;
            _ShowMidlines = true;
            _RenderRealFrets = true;
            _FretboardOrientation = Orientation.Horizontal;
            _DefaultDisplayUnit = UnitOfMeasure.Mm;
            _FretWidth = Measure.Mm(2.5);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetFretWidth()
        {
            FretWidth = Measure.Mm(2.5);
        }

        public bool ShouldSerializeFretWidth()
        {
            return FretWidth.NormalizedValue != 0.25;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetDefaultDisplayUnit()
        {
            DefaultDisplayUnit = UnitOfMeasure.Mm;
        }

        public bool ShouldSerializeDefaultDisplayUnit()
        {
            return DefaultDisplayUnit != UnitOfMeasure.Mm;
        }
    }
}
