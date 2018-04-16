using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.Common
{
    public static class AppPreferences
    {
        private static string _AppDirectoryPath;

        public static string AppDirectoryPath
        {
            get
            {
                if(_AppDirectoryPath == null)
                    _AppDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
                return _AppDirectoryPath;
            }
        }

        public static void InitializeAppFolder()
        {
            if (!Directory.Exists(AppDirectoryPath))
                Directory.CreateDirectory(AppDirectoryPath);
            Directory.CreateDirectory(Path.Combine(AppDirectoryPath, "Templates"));

            if (!File.Exists(Path.Combine(AppDirectoryPath, "config.xml")))
                CreateConfigFile();
        }

        private static void CreateConfigFile()
        {

        }

        #region App Config File



        #endregion
    }
}
