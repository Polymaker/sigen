using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
    static class NativeUtils
    {
        [DllImport("user32", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr Hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public const int WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;    // Any value the sender chooses.  Perhaps its main window handle?
            public int cbData;       // The count of bytes in the message.
            public IntPtr lpData;    // The address of the message.
        }

        public static void SendDataMessage(IntPtr procHandle, string msg)
        {
            IntPtr messagePtr = Marshal.StringToHGlobalUni(msg);

            var copyData = new COPYDATASTRUCT
            {
                dwData = IntPtr.Zero,
                lpData = messagePtr,
                cbData = msg.Length * 2
            };

            IntPtr dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<COPYDATASTRUCT>());
            Marshal.StructureToPtr(copyData, dataPtr, false);

            SendMessage(procHandle, WM_COPYDATA, IntPtr.Zero, dataPtr);

            Marshal.FreeHGlobal(dataPtr);
            Marshal.FreeHGlobal(messagePtr);
        }
    }
}
