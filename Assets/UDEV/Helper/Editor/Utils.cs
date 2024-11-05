using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDEV
{
    public static class Utils
    {
        public static void CreateMissingDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

        public static string GenerateUID()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            int z1 = UnityEngine.Random.Range(0, 1000000);
            int z2 = UnityEngine.Random.Range(0, 1000000);
            string uid = currentEpochTime + "" + z1 + "" + z2;
            return uid;
        }

        public static void SaveDataToFile<T>(string path, string fileName, T data)
        {
            CreateMissingDirectory(path);
            FileStream fs = new FileStream(path + fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, data);
            fs.Close();
        }

        public static T LoadDataFromFile<T>(string filePath, T data)
        {
            if (File.Exists(filePath))
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var bformatter = new BinaryFormatter();

                    data = (T)bformatter.Deserialize(stream);

                    return data;
                }
            }

            return default;
        }

        public static bool IsFileExist(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
