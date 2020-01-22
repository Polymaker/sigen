using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Windows
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            AppVersionLabel.Text = $"v{Application.ProductVersion}";

            //var myAssem = Assembly.GetExecutingAssembly();
            //var copyrightAttr = myAssem.GetCustomAttribute<AssemblyCopyrightAttribute>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetExternalLibraries();
        }

        class LibInfo
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Version { get; set; }
            public string Copyright { get; set; }
            public string License { get; set; }
        }

        private void GetExternalLibraries()
        {
            var libraries = new List<LibInfo>();


            libraries.Add(GetLibInfo(typeof(netDxf.DxfDocument).Assembly, "LGPL"));

            libraries.Add(GetLibInfo(typeof(Svg.SvgDocument).Assembly, "MS-PL"));

            libraries.Add(GetLibInfo(typeof(WeifenLuo.WinFormsUI.Docking.DockPanel).Assembly, "MITL"));

            foreach (var libInfo in libraries)
            {
                AppInfoTextbox.AppendText($"{libInfo.Name}: {libInfo.Title}\r\n");

                if (!string.IsNullOrEmpty(libInfo.Description))
                    AppInfoTextbox.AppendText($"{libInfo.Description}\r\n");

                if (!string.IsNullOrEmpty(libInfo.Copyright))
                    AppInfoTextbox.AppendText($"{libInfo.Copyright}\r\n");

                if (!string.IsNullOrEmpty(libInfo.License))
                    AppInfoTextbox.AppendText($"Licensed under {libInfo.License}\r\n");

                AppInfoTextbox.AppendText("\r\n");
            }
        }

        private LibInfo GetLibInfo(Assembly assembly, string license = null)
        {
            var titleAttr = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            var descAttr = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var copyrightAttr = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            var versionAttr = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            var assemName = assembly.GetName();
            
            var info = new LibInfo()
            {
                Name = Path.GetFileName(assembly.Location),
                Title = Coalesce(titleAttr?.Title, assemName.Name),
                Copyright = copyrightAttr?.Copyright,
                Description = descAttr?.Description,
                Version = assemName.Version.ToString(),
                License = license ?? string.Empty
            };

            if (!string.IsNullOrEmpty(info.Copyright) && 
                !info.Copyright.ToLower().Contains("copyright"))
            {
                info.Copyright = "Copyright © " + info.Copyright.Replace("©", string.Empty).Replace("  ", " ");
            }

            return info;
        }

        static string Coalesce(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1))
                return str2;
            return str1;
        }
    }
}
