using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace CommonLibrary.Handlers
{
    public static class ConfigurationHandler <T>
    {
        // T will be the main configuartion class
        private static string configurationFolderName = "Config";

        static ConfigurationHandler()
        {
            SetupConfiguration();
        }

        private static void SetupConfiguration()
        {
            string configurationName = GetConfigFile();
            if (!File.Exists(configurationName))
            {
                WriteConfig(default(T));
            }
        }

        private static string GetConfigFile()
        {
            // Return the configuration file
            return $@"{AppDomain.CurrentDomain.BaseDirectory}/{configurationFolderName}/{typeof(T).Name}.json";
        }

        public static T ReadConfig()
        {
            // Get the congfiguration File
            string configFile = GetConfigFile();
            
            // Deserialize Json to Object and Return It
            using (var fileStream = new FileStream(configFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var textReader = new StreamReader(fileStream))
            {
                string content = textReader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public static void WriteConfig(T configurationObject)
        {
            // Grab the configuration file
            string configFile = GetConfigFile();

            // Serialize the object to JSON indented
            string fileContent = JsonConvert.SerializeObject(configurationObject, Formatting.Indented);

            // Write the string into the config file
            using (StreamWriter writer = new StreamWriter(configFile, false, Encoding.Unicode))
            {
                writer.Write(fileContent);
            }
        }
    }
}
