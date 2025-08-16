using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SiGen.Configuration;
using SiGen.Configuration.Display;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public class ViewerDisplayConfig : INotifyPropertyChanged
    {
        private UnitOfMeasure _DefaultDisplayUnit;
        private Orientation _FingerboardOrientation;
        private Measure _FretExtensionAmount;
        private StringsDisplayConfig _Strings;
        private FretsDisplayConfigs _Frets;
        private FingerboardDisplayConfig _Fingerboard;
        private LineDisplayConfig _CenterLine;
        private LineDisplayConfig _Midlines;
        private LineDisplayConfig _Margins;

        [JsonProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public StringsDisplayConfig Strings
        {
            get => _Strings;
            set => SetPropertyValue(ref _Strings, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowStrings
        {
            get => Strings.Visible;
            set => Strings.Visible = value;
        }

        [JsonProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public FretsDisplayConfigs Frets
        {
            get => _Frets;
            set => SetPropertyValue(ref _Frets, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowFrets
        {
            get => Frets.Visible;
            set => Frets.Visible = value;
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowFretNumbers
        {
            get => Frets.ShowNumbers;
            set => Frets.ShowNumbers = value;
        }

        [JsonProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public FingerboardDisplayConfig Fingerboard
        {
            get => _Fingerboard;
            set => SetPropertyValue(ref _Fingerboard, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowFingerboard
        {
            get => Fingerboard.Visible;
            set => Fingerboard.Visible = value;
        }

        [JsonProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public LineDisplayConfig Margins
        {
            get => _Margins;
            set => SetPropertyValue(ref _Margins, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowMargins
        {
            get => Margins.Visible;
            set => Margins.Visible = value;
        }

        [JsonProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public LineDisplayConfig CenterLine
        {
            get => _CenterLine;
            set => SetPropertyValue(ref _CenterLine, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowCenterLine
        {
            get => CenterLine.Visible;
            set => CenterLine.Visible = value;
        }

        [JsonProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public LineDisplayConfig Midlines
        {
            get => _Midlines;
            set => SetPropertyValue(ref _Midlines, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ShowMidlines
        {
            get => Midlines.Visible;
            set => Midlines.Visible = value;
        }


        [DefaultValue(Orientation.Horizontal)]
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation FingerboardOrientation
        {
            get => _FingerboardOrientation;
            set => SetPropertyValue(ref _FingerboardOrientation, value);
        }

        [JsonIgnore, Browsable(false)]
        public bool ExtendFrets => !FretExtensionAmount.IsEmpty;

        [Browsable(false)]
        [JsonIgnore]
        public Measure FretExtensionAmount
        {
            get => _FretExtensionAmount;
            set => SetPropertyValue(ref _FretExtensionAmount, value);
        }

        [Editor(typeof(Designers.UnitOfMeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [JsonProperty, JsonConverter(typeof(MeasureJsonConverter))]
        public UnitOfMeasure DefaultDisplayUnit
        {
            get => _DefaultDisplayUnit;
            set => SetPropertyValue(ref _DefaultDisplayUnit, value);
        }

        [Browsable(false), JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineDisplayConfig[] LineConfigs => new LineDisplayConfig[]
        {
            Strings,
            Frets,
            Fingerboard,
            Margins,
            Midlines,
            CenterLine
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewerDisplayConfig()
        {
            _Strings = new StringsDisplayConfig();

            _Frets = new FretsDisplayConfigs();

            _Fingerboard = new FingerboardDisplayConfig();

            _Margins = new LineDisplayConfig();

            _CenterLine = new LineDisplayConfig();

            _Midlines = new LineDisplayConfig();

            /*
            _Strings = new StringsDisplayConfig()
            {
                Visible = true,
                Color = Color.Black,
                RenderMode = LineRenderMode.RealisticLook
            };

            _Frets = new FretsDisplayConfigs()
            {
                Visible = true,
                Color = Color.Red,
                RenderMode = LineRenderMode.RealisticLook,
                RenderWidth = Measure.Mm(2.6)
            };

            _Fingerboard = new FingerboardDisplayConfig()
            {
                Visible = true,
                Color = Color.Blue,
                ContinueLines = true
            };

            _Margins = new LineDisplayConfig()
            {
                Visible = true,
                Color = Color.Gray
            };

            _CenterLine = new LineDisplayConfig()
            {
                Visible = false,
                Color = Color.Black
            };

            _Midlines = new LineDisplayConfig()
            {
                Visible = true,
                Color = Color.Gainsboro
            };
            */
            _FingerboardOrientation = Orientation.Horizontal;
            _DefaultDisplayUnit = UnitOfMeasure.Mm;
            _FretExtensionAmount = Measure.Empty;
        }

        internal void InitDefaultDesignerValues()
        {
            for (int i = 0; i < LineConfigs.Length; i++)
                LineConfigs[i].InitDefaultValues();
        }

        internal void SetDefaultValues(ViewerDisplayConfig displayConfig)
        {
            for (int i = 0; i < LineConfigs.Length; i++)
                LineConfigs[i].SetDefaultValues(displayConfig.LineConfigs[i]);
        }

        internal void CopyValues(ViewerDisplayConfig displayConfig)
        {
            for (int i = 0; i < LineConfigs.Length; i++)
                LineConfigs[i].CopyValues(displayConfig.LineConfigs[i]);
        }

        public static ViewerDisplayConfig CreateDefault()
        {
            var defaultCfg = new ViewerDisplayConfig
            {
                Strings = new StringsDisplayConfig()
                {
                    Visible = true,
                    Color = Color.Black,
                    RenderMode = LineRenderMode.RealisticLook
                },
                Frets = new FretsDisplayConfigs()
                {
                    Visible = true,
                    Color = Color.Red,
                    RenderMode = LineRenderMode.RealisticLook,
                    RenderWidth = Measure.Mm(2.6),
                    DisplayBridgeLine = true
                },
                Fingerboard = new FingerboardDisplayConfig()
                {
                    Visible = true,
                    Color = Color.Blue,
                    ContinueLines = true
                },
                Margins = new LineDisplayConfig()
                {
                    Visible = true,
                    Color = Color.Gray
                },
                CenterLine = new LineDisplayConfig()
                {
                    Visible = false,
                    Color = Color.Black
                },
                Midlines = new LineDisplayConfig()
                {
                    Visible = true,
                    Color = Color.Gainsboro
                },
                FingerboardOrientation = Orientation.Horizontal,
                DefaultDisplayUnit = UnitOfMeasure.Mm,
                FretExtensionAmount = Measure.Empty
            };
            //defaultCfg.InitDefaultDesignerValues();
            return defaultCfg;
        }

        public ViewerDisplayConfig Clone()
        {
            var jsonConfig = new JsonSerializerSettings
            {
                ContractResolver = new ShouldSerializeContractResolver(true)
            };
            string json = JsonConvert.SerializeObject(this, jsonConfig);
            JsonSerializer serializer = new JsonSerializer();
            using (var sr = new StringReader(json))
                return (ViewerDisplayConfig)serializer.Deserialize(sr, typeof(ViewerDisplayConfig));
        }

        #region INotifyPropertyChanged

        protected void SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DettachPropertyChangedEvent()
        {
            for (int i = 0; i < LineConfigs.Length; i++)
                LineConfigs[i].PropertyChanged -= ConfigObj_PropertyChanged;
        }

        public void AttachPropertyChangedEvent()
        {
            for (int i = 0; i < LineConfigs.Length; i++)
            {
                LineConfigs[i].PropertyChanged -= ConfigObj_PropertyChanged;
                LineConfigs[i].PropertyChanged += ConfigObj_PropertyChanged;
            }
        }

        private void ConfigObj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var sourceConfig = (LineDisplayConfig)sender;
            if (sourceConfig == Strings)
                OnPropertyChanged($"{nameof(Strings)}.{e.PropertyName}");

            else if (sourceConfig == Midlines)
                OnPropertyChanged($"{nameof(Midlines)}.{e.PropertyName}");

            else if (sourceConfig == CenterLine)
                OnPropertyChanged($"{nameof(CenterLine)}.{e.PropertyName}");

            else if (sourceConfig == Frets)
                OnPropertyChanged($"{nameof(Frets)}.{e.PropertyName}");

            else if (sourceConfig == Fingerboard)
                OnPropertyChanged($"{nameof(Fingerboard)}.{e.PropertyName}");

            else if (sourceConfig == Margins)
                OnPropertyChanged($"{nameof(Margins)}.{e.PropertyName}");

            //else if (sourceConfig == GuideLines)
            //    OnPropertyChanged($"{nameof(GuideLines)}.{e.PropertyName}");
        }

        #endregion

        #region Winform Designer Specifics

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetStrings()
        {
            Strings.ResetConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeStrings()
        {
            return Strings.ShouldSerializeConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetFrets()
        {
            Frets.ResetConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeFrets()
        {
            return Frets.ShouldSerializeConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetFingerboard()
        {
            Fingerboard.ResetConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeFingerboard()
        {
            return Fingerboard.ShouldSerializeConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetMargins()
        {
            Margins.ResetConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeMargins()
        {
            return Margins.ShouldSerializeConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetCenterLine()
        {
            CenterLine.ResetConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeCenterLine()
        {
            return CenterLine.ShouldSerializeConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetMidlines()
        {
            Midlines.ResetConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeMidlines()
        {
            return Midlines.ShouldSerializeConfig();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetFretExtensionAmount()
        {
            FretExtensionAmount = Measure.Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeFretExtensionAmount()
        {
            return !FretExtensionAmount.IsEmpty;
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

        #endregion



    }
}
