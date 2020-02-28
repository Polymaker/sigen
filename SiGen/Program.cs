using SiGen.UI;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SiGen
{
    static class Program
    {
        static Mutex AppInstance = new Mutex(true, "{B795E643-1BF8-43E1-A238-801961D8F8AD}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (AppInstance.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-CA");
                Application.Run(new LayoutEditorWindow(args));
            }
            else
            {
                var myProc = Process.GetCurrentProcess();
                var procs = Process.GetProcessesByName("SiGen");

                string message = "MUTEX#";
                if (args != null)
                    message += string.Join(",", args.Select(x => x.Replace(",", "&#44;")));

                foreach (var proc in procs)
                {
                    if (proc.Id == myProc.Id)
                        continue;
                    NativeUtils.SendDataMessage(proc.MainWindowHandle, message);
                    break;
                }
            }
        }
    }
}
