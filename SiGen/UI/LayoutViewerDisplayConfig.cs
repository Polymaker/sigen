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
        private bool _RenderRealStrings;
        private bool _RenderRealFrets;
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

        //[EditorAttribute(typeof(MeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Measure FretWidth
        {
            get { return _FretWidth; }
            set
            {
                if (value != _FretWidth || (_FretWidth.Unit != value.Unit))
                {
                    _FretWidth = value;
                    OnPropertyChanged("FretWidth");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LayoutViewerDisplayConfig()
        {
            _ShowStrings = true;
            _ShowFrets = true;
            _ShowMidlines = true;
            _RenderRealFrets = true;
            _FretboardOrientation = Orientation.Horizontal;
            _FretWidth = Measure.Mm(2.5);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
