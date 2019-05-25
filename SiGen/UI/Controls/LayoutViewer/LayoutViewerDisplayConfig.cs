using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public class LayoutViewerDisplayConfig : INotifyPropertyChanged
    {
        private bool _ShowCenterLine;
        private bool _ShowStrings;
        private bool _ShowFrets;
        private bool _ShowMidlines;
        private bool _ShowMargins;
        private bool _ShowFingerboard;
        private bool _ShowTheoreticalFrets;
        private bool _RenderRealStrings;
        private bool _RenderRealFrets;
        private Color _FretLineColor;
        private UnitOfMeasure _DefaultDisplayUnit;
        private Measure _FretWidth;
        private Orientation _FingerboardOrientation;

        [DefaultValue(false)]
        public bool ShowCenterLine
        {
            get { return _ShowCenterLine; }
            set
            {
                if (value != _ShowCenterLine)
                {
                    _ShowCenterLine = value;
                    OnPropertyChanged("ShowCenterLine");
                }
            }
        }

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

        [DefaultValue(true)]
        public bool ShowMargins
        {
            get { return _ShowMargins; }
            set
            {
                if (value != _ShowMargins)
                {
                    _ShowMargins = value;
                    OnPropertyChanged("ShowMargins");
                }
            }
        }

        [DefaultValue(true)]
        public bool ShowFingerboard
        {
            get { return _ShowFingerboard; }
            set
            {
                if (value != _ShowFingerboard)
                {
                    _ShowFingerboard = value;
                    OnPropertyChanged("ShowFingerboard");
                }
            }
        }

        [DefaultValue(Orientation.Horizontal)]
        public Orientation FingerboardOrientation
        {
            get { return _FingerboardOrientation; }
            set
            {
                if (value != _FingerboardOrientation)
                {
                    _FingerboardOrientation = value;
                    OnPropertyChanged("FingerboardOrientation");
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

        [DefaultValue(typeof(Color), "0xFF0000")]
        public Color FretLineColor
        {
            get => _FretLineColor;
            set
            {
                if (value != _FretLineColor)
                {
                    _FretLineColor = value;
                    OnPropertyChanged(nameof(FretLineColor));
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

        [Editor(typeof(Designers.MeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Editor(typeof(Designers.UnitOfMeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
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
            _ShowMargins = true;
            _RenderRealFrets = true;
            _ShowFingerboard = true;
            _FingerboardOrientation = Orientation.Horizontal;
            _DefaultDisplayUnit = UnitOfMeasure.Mm;
            _FretWidth = Measure.Mm(2.5);
            _FretLineColor = Color.Red;
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
