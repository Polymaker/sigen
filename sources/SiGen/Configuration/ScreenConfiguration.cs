using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiGen.Configuration
{
    public class ScreenConfiguration
    {
        [XmlAttribute]
        public string ID { get; set; }
        [XmlIgnore]
        public int DisplayIndex { get; set; }
        [XmlIgnore]
        public string Name { get; set; }
        [XmlIgnore]
        public bool IsPrimary { get; set; }
        [XmlAttribute]
        public int DPI { get; set; }
        [XmlAttribute]
        public int ResolutionX { get; set; }
        [XmlAttribute]
        public int ResolutionY { get; set; }
        [XmlIgnore]
        public bool HasChanged { get; set; }

        #region PInvoke

        [Flags]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;

            public static DISPLAY_DEVICE NewInstance
            {
                get
                {
                    return new DISPLAY_DEVICE() { cb = Marshal.SizeOf<DISPLAY_DEVICE>() };
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        #endregion

        private static Regex GrabDisplayIndex = new Regex("DISPLAY(\\d+)", RegexOptions.Compiled);

        public static List<ScreenConfiguration> EnumarateScreens()
        {
            var screenList = new List<ScreenConfiguration>();
            try
            {
                var d = DISPLAY_DEVICE.NewInstance;
                for (uint id = 0; EnumDisplayDevices(null, id, ref d, 0); id++)
                {
                    if (d.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                    {
                        int displayIndex = 0;
                        var match = GrabDisplayIndex.Match(d.DeviceName);
                        if (match.Success)
                            displayIndex = int.Parse(match.Groups[1].Value);
                        
                        screenList.Add(new ScreenConfiguration()
                        {
                            ID = d.DeviceID,
                            Name = d.DeviceName,
                            DisplayIndex = displayIndex,
                            IsPrimary = d.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice),
                            DPI = 96
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return screenList;
        }

    }
}
