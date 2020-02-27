using SiGen.Resources;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Localization
{
    static class LayoutChangeTranslator
    {

        public static string GetChangeDescription(SILayout layout, ILayoutChange change)
        {
            string changeDesc;

            if (change is BatchChange batchChange)
            {
                changeDesc = GetBatchDescription(layout, batchChange);
                if (!string.IsNullOrEmpty(changeDesc))
                    return changeDesc;
            }
            else if (change is PropertyChange propertyChange)
            {
                changeDesc = GetPropertyDescription(layout, propertyChange);
                if (!string.IsNullOrEmpty(changeDesc))
                    return changeDesc;
            }
            else if (change is CollectionChange collection)
            {
                changeDesc = GetCollectionDescription(layout, collection);
                if (!string.IsNullOrEmpty(changeDesc))
                    return changeDesc;
            }
            return "Unnamed Change";
        }

        private static string GetBatchDescription(SILayout layout, BatchChange batchChange)
        {
            switch (batchChange.Name)
            {
                case "NumberOfFrets":
                    {
                        var propChange = batchChange.ChangedProperties
                            .FirstOrDefault(x => x.Property == nameof(SIString.NumberOfFrets));

                        if (propChange != null && propChange.NewValue is int fretCount)
                            return $"{Localizations.LayoutProperty_NumberOfFrets}: {fretCount}";

                        return Localizations.LayoutProperty_NumberOfFrets;
                    }

                case "AddStrings":
                case "RemoveStrings":
                    {
                        var lastChange = batchChange.LayoutChanges
                            .OfType<CollectionChange>()
                            .Where(x => x.ElementType == typeof(SIString))
                            .LastOrDefault();

                        if (lastChange != null)
                            return $"{Localizations.LayoutProperty_NumberOfStrings}: {lastChange.CollectionCount}";

                        break;
                    }
            }

            if (batchChange.Component != null)
            {
                var marginChange = batchChange.ChangedProperties.FirstOrDefault(x => x.Component is FingerboardMargin);
                if (marginChange != null)
                    return GetFingerboardMarginDescription(layout, batchChange.Name, marginChange);
            }

            return null;
        }

        private static string GetPropertyDescription(SILayout layout, PropertyChange propChange)
        {
            if (propChange.Component != null)
            {
                switch (propChange.Component)
                {
                    case ScaleLengthManager _:
                        {
                            switch (propChange.Property)
                            {
                                case nameof(SingleScaleManager.Length):
                                    return  $"{Localizations.Words_ScaleLength}: {propChange.NewValue}";
                                case nameof(DualScaleManager.Bass):
                                    return  $"{Localizations.Words_ScaleLength} ({Localizations.FingerboardSide_Bass}): {propChange.NewValue}";
                                case nameof(DualScaleManager.Treble):
                                    return $"{Localizations.Words_ScaleLength} ({Localizations.FingerboardSide_Treble}): {propChange.NewValue}";
                                case nameof(DualScaleManager.PerpendicularFretRatio):
                                    if (propChange.NewValue is double fretRatio)
                                    {
                                        int fretNumber = FretHelper.GetFretNumberFromRatio(fretRatio);
                                        string fretName = Localizations.Words_CustomRatio;
                                        switch (fretNumber)
                                        {
                                            case 0:
                                                fretName = Localizations.FingerboardEnd_Nut;
                                                break;
                                            case 1:
                                                fretName = Localizations.FingerboardEnd_Bridge;
                                                break;
                                            default:
                                                fretName = $"{fretNumber} {Localizations.Words_Fret}";
                                                break;
                                        }
                                        return $"{Localizations.Words_PerpendicularFret}: {fretName}";
                                    }
                                    else
                                        return $"{Localizations.Words_PerpendicularFret}";
                                default:
                                    return $"{Localizations.Words_ScaleLength} {propChange.Property}: {propChange.NewValue}";
                            }
                        }

                    case StringSpacingManager _:
                        {
                            switch (propChange.Property)
                            {
                                case nameof(StringSpacingManager.BridgeAlignment):
                                    return $"{Localizations.StringSpacingProperty_BridgeAlignment}";
                                case nameof(StringSpacingManager.NutAlignment):
                                    return $"{Localizations.StringSpacingProperty_NutAlignment}";

                                case nameof(StringSpacingSimple.BridgeSpacingMode):
                                case nameof(StringSpacingSimple.NutSpacingMode):

                                    {
                                        string propDesc = Localizations.StringSpacingProperty_SpacingMode;

                                        string modeDesc = GetLocText($"StringSpacingMethod_{propChange.NewValue}");

                                        int charIdx = modeDesc.IndexOf('(');
                                        if (charIdx > 0)
                                            modeDesc = modeDesc.Substring(0, charIdx).Trim();

                                        string sideDesc = propChange.Property == nameof(StringSpacingSimple.BridgeSpacingMode) ?
                                            Localizations.FingerboardEnd_Bridge : Localizations.FingerboardEnd_Nut;

                                        return $"{propDesc} ({sideDesc}): {modeDesc}";
                                    }

                                case nameof(StringSpacingSimple.StringSpacingAtBridge):
                                case nameof(StringSpacingSimple.StringSpacingAtNut):
                                    {
                                        string propDesc = Localizations.StringSpacingProperty_Spacing;
                                        string sideDesc = propChange.Property == nameof(StringSpacingSimple.StringSpacingAtBridge) ?
                                            Localizations.FingerboardEnd_Bridge : Localizations.FingerboardEnd_Nut;

                                        return $"{propDesc} ({sideDesc}): {propChange.NewValue}";
                                    }

                                default:
                                    return $"String spacing {propChange.Property}: {propChange.NewValue}";
                            }
                        }
                    
                    case FingerboardMargin _:
                        return GetFingerboardMarginDescription(layout, propChange.Name, propChange);

                    case SIString str:
                        {
                            

                            switch (propChange.Property)
                            {
                                case nameof(SIString.Gauge):
                                case nameof(SIString.PhysicalProperties) + "." + nameof(StringProperties.StringDiameter):
                                    return $"{Localizations.StringProperty_Gauge} ({Localizations.Words_String} {str.Index + 1}): {propChange.NewValue}";
                                case nameof(SIString.StartingFret):
                                    return $"{Localizations.StringProperty_StartingFret} ({Localizations.Words_String} {str.Index + 1}): {propChange.NewValue}";
                                case nameof(SIString.ScaleLength):
                                    return $"{Localizations.Words_ScaleLength} ({Localizations.Words_String} {str.Index + 1}): {propChange.NewValue}";
                                case nameof(SIString.NumberOfFrets):
                                    return $"{Localizations.LayoutProperty_NumberOfFrets} ({Localizations.Words_String} {str.Index + 1}): {propChange.NewValue}";
                            }

                            if (propChange.Property.StartsWith(nameof(SIString.PhysicalProperties)))
                            {
                                return Localizations.StringProperties;
                            }
                            break;
                        }
                }

            }
            else
            {
                switch (propChange.Property)
                {
                    case nameof(SILayout.ScaleLengthMode):
                        {
                            var modeStr = GetLocText($"ScaleLengthType_{propChange.NewValue}");
                            return $"{Localizations.LayoutProperty_ScaleLengthMode}: {modeStr}";
                        }
                    case nameof(SILayout.StringSpacingMode):
                        {
                            var modeStr = GetLocText($"StringSpacingType_{propChange.NewValue}");
                            return $"{Localizations.LayoutProperty_StringSpacingMode}: {modeStr}";
                        }
                    case nameof(SILayout.LeftHanded):
                        {
                            return Localizations.Layout_LeftHanded;
                        }
                        //case nameof(SILayout.NumberOfStrings):
                        //    return $"Number of strings: {pc.NewValue}";
                }
            }

            return null;
        }

        private static string GetCollectionDescription(SILayout layout, CollectionChange colChange)
        {
            if (colChange.ElementType == typeof(SIString))
            {
                return $"{Localizations.LayoutProperty_NumberOfStrings}: {colChange.CollectionCount}";
            }

            return null;
        }

        private static string GetFingerboardMarginDescription(SILayout layout, string changeName, PropertyChange propChange)
        {
            string specifedSide = string.Empty;

            switch (changeName)
            {
                case nameof(FingerboardMargin.LastFret):
                    return $"{Localizations.Words_Margins} ({Localizations.FingerboardMarginProperty_LastFret}): {propChange.NewValue}";
                case nameof(FingerboardMargin.Bass):
                    specifedSide = Localizations.FingerboardSide_Bass; break;
                case nameof(FingerboardMargin.Treble):
                    specifedSide = Localizations.FingerboardSide_Treble; break;
                case nameof(FingerboardMargin.MarginAtNut):
                    specifedSide = Localizations.FingerboardEnd_Nut; break;
                case nameof(FingerboardMargin.MarginAtBridge):
                    specifedSide = Localizations.FingerboardEnd_Bridge; break;
                case nameof(FingerboardMargin.Edges):
                    specifedSide = Localizations.Margins_Edges; break;
            }

            if (!string.IsNullOrEmpty(specifedSide))
                return $"{Localizations.Words_Margins} ({specifedSide}): {propChange.NewValue}";

            return $"{Localizations.Words_Margins}: {propChange.NewValue}";
        }

        private static string GetLocText(string textID)
        {
            return Localizations.ResourceManager.GetString(textID);
        }
    }
}
