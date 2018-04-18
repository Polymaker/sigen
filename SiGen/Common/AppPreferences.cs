using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SiGen.Common
{
    public static class AppPreferences
    {
        private static string _AppDirectoryPath;
        private static AppConfig _Current;
        private static bool initialized;

        public static AppConfig Current
        {
            get
            {
                if (!initialized)
                    Initialize();
                return _Current;
            }
        }

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
            //Directory.CreateDirectory(Path.Combine(AppDirectoryPath, "Templates"));
        }

        private static void Initialize()
        {
            InitializeAppFolder();

            if (File.Exists(Path.Combine(AppDirectoryPath, "config.xml")))
                _Current = LoadConfig();

            if(_Current == null)
            {
                _Current = new AppConfig();
                SaveConfig(_Current);
            }

            initialized = true;
        }

        private static void SaveConfig(AppConfig config)
        {
            try
            {
                var ser = new XmlSerializer(typeof(AppConfig));
                using (var fs = File.Open(Path.Combine(AppDirectoryPath, "config.xml"), FileMode.Create))
                    ser.Serialize(fs, config);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error saving config file: " + ex.ToString());
            }
        }

        private static AppConfig LoadConfig()
        {
            try
            {
                var ser = new XmlSerializer(typeof(AppConfig));
                using (var fs = File.Open(Path.Combine(AppDirectoryPath, "config.xml"), FileMode.Open, FileAccess.Read))
                    return (AppConfig)ser.Deserialize(fs);
            }
            catch(Exception ex)
            {
                Trace.WriteLine("Error loading config file: " + ex.ToString());
            }
            return null;
        }

        public static void Save()
        {
            SaveConfig(Current);
        }

        public static void Reload()
        {
            _Current = LoadConfig();
        }

        public static void AddRecentFile(string filename)
        {
            Current.RecentFiles.RemoveAll(f => f.Filename == filename);
            Current.RecentFiles.Insert(0, new RecentFile(filename));

            while (Current.RecentFiles.Count > Current.FileHistoryConfig.MaxHistory)
                Current.RecentFiles.RemoveAt(Current.RecentFiles.Count - 1);
            Save();
        }

        public static void ValidateRecentFiles()
        {
            bool changed = false;
            foreach(var file in Current.RecentFiles.ToArray())
            {
                if (!File.Exists(file.Filename))
                {
                    Current.RecentFiles.Remove(file);
                    changed = true;
                }
            }

            if(changed)
                Save();
        }


        #region App Config File

        [XmlRoot("SiGen")]
        public class AppConfig
        {
            [XmlElement("FileHistory")]
            public FileHistory FileHistoryConfig { get; set; }

            [XmlIgnore]
            public List<RecentFile> RecentFiles { get { return FileHistoryConfig.Files; } }

            public AppConfig()
            {
                FileHistoryConfig = new FileHistory();
            }
        }

        public class FileHistory
        {
            [XmlAttribute]
            public int MaxHistory { get; set; }
            [XmlElement("File")]
            public List<RecentFile> Files { get; set; }

            public FileHistory()
            {
                MaxHistory = 10;
                Files = new List<RecentFile>();
            }
        }

        [XmlRoot("File"), XmlType("File")]
        public class RecentFile
        {
            [XmlAttribute("Filename")]
            public string Filename { get; set; }
            public RecentFile() { Filename = string.Empty; }
            public RecentFile(string filename) { Filename = filename; }
        }

        #endregion
    }
}
