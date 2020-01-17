using Newtonsoft.Json;
using SiGen.Export;
using SiGen.UI;
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

namespace SiGen.Configuration
{
    public static partial class AppConfigManager
    {
        private static string _AppConfigDirectory;
        private static AppConfig _Current;
        private static bool initialized;
        public const string CONFIG_FILENAME = "AppConfig.json";

        public static AppConfig Current
        {
            get
            {
                if (!initialized)
                    Initialize();
                return _Current;
            }
        }

        public static string AppConfigDirectory
        {
            get
            {
                if (_AppConfigDirectory == null)
                    _AppConfigDirectory = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "SiGen"/*Application.ProductName*/);
                return _AppConfigDirectory;
            }
        }

        public static string AppConfigPath => Path.Combine(AppConfigDirectory, CONFIG_FILENAME);

        public static void InitializeAppDataFolder()
        {
            if (!Directory.Exists(AppConfigDirectory))
                Directory.CreateDirectory(AppConfigDirectory);
            //Directory.CreateDirectory(Path.Combine(AppConfigDirectory, "ExportConfigs"));
        }

        private static void Initialize()
        {
            InitializeAppDataFolder();

            if (File.Exists(AppConfigPath))
                _Current = LoadAppConfig();

            if (_Current == null)
            {
                _Current = AppConfig.CreateDefault();
                SaveAppConfig(_Current);
            }
            
            if (_Current.DisplayConfig == null || _Current.ExportConfig == null)
            {
                if (_Current.DisplayConfig == null)
                    _Current.DisplayConfig = LayoutViewerDisplayConfig.CreateDefault();
                if (_Current.ExportConfig == null)
                    _Current.ExportConfig = LayoutExportConfig.CreateDefault();
                SaveAppConfig(_Current);
            }

            initialized = true;
        }



        public static string GetAppDataFolder(string folderName)
        {
            return Path.Combine(AppConfigDirectory, folderName);
        }

        private static void SaveAppConfig(AppConfig config)
        {
            try
            {
                var jsonConfig = new JsonSerializerSettings
                {
                    ContractResolver = new ShouldSerializeContractResolver(),
                    Formatting = Formatting.Indented
                };

                string json = JsonConvert.SerializeObject(config, jsonConfig);
                using (var fs = File.Open(AppConfigPath, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error saving config file: " + ex.ToString());
            }
        }

        private static AppConfig LoadAppConfig()
        {
            try
            {
                //var ser = new XmlSerializer(typeof(AppConfig));
                AppConfig config = null;

                using (var file = File.OpenText(AppConfigPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    config = (AppConfig)serializer.Deserialize(file, typeof(AppConfig));
                }

                return config;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error loading config file: " + ex.ToString());
            }
            return null;
        }

        //public static IEnumerable<LayoutExportConfig> LoadExportConfigs()
        //{
        //    foreach (var cfgFile in Directory.GetFiles(GetAppDataFolder("ExportConfigs"), "*.json"))
        //    {
        //        LayoutExportConfig exportCfg = null;
        //        try
        //        {
        //            var jsonContent = File.ReadAllText(cfgFile);
        //            exportCfg = JsonConvert.DeserializeObject<LayoutExportConfig>(jsonContent);

        //        }
        //        catch { }

        //        if (exportCfg != null)
        //            yield return exportCfg;
        //    }
        //    yield break;
        //}

        public static void Save()
        {
            SaveAppConfig(Current);
        }

        public static void Reload()
        {
            _Current = LoadAppConfig();
        }

        public static void AddRecentFile(string filename, string layoutName = null)
        {
            Current.RecentFiles.RemoveAll(f => f.Filename == filename);
            Current.RecentFiles.Insert(0, new RecentFile(filename, layoutName));

            while (Current.RecentFiles.Count > Current.MaxRecentFileHistory)
                Current.RecentFiles.RemoveAt(Current.RecentFiles.Count - 1);
            Save();
        }

        public static void ValidateRecentFiles()
        {
            bool changed = false;

            foreach (var file in Current.RecentFiles.ToArray())
            {
                if (!File.Exists(file.Filename))
                {
                    Current.RecentFiles.Remove(file);
                    changed = true;
                }
            }

            if (changed)
                Save();
        }

    }
}
