namespace SiGen.Physics
{
    public enum IntonationMethod
    {
        Just,
        EqualTempered,
        /// <summary>
        /// This method exists but I don't use it
        /// </summary>
        Pythagorean,
        /// <summary>
        /// Used for notes calculated from pitch alone (eg. notes created from an offsetted/calculated pitch)
        /// </summary>
        Computed
    }
}
