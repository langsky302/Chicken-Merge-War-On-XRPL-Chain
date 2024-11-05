using System.IO;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class CSV_Ulti
    {
        private string m_directoryName;
        private string m_fileName;
        private string m_separator = ",";
        private string[] m_headers;

        public CSV_Ulti(string directoryName,string[] headers)
        {
            m_directoryName = directoryName;
            m_headers = headers;
        }

        public void SetFilename(string filename)
        {
            m_fileName = filename;
        }

        public void VerifyDirectory()
        {
            string dir = GetDirectoryPath();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public void AppendToCSV(string[] contents)
        {
            using (StreamWriter sw = File.AppendText(GetFilePath()))
            {
                string finalString = "";
                for (int i = 0; i < contents.Length; i++)
                {
                    if (!string.IsNullOrEmpty(finalString))
                    {
                        finalString += m_separator;
                    }
                    finalString += contents[i];
                }
                finalString += m_separator;
                sw.WriteLine(finalString);
            }
        }

        public void CreateCSV()
        {
            using (StreamWriter sw = File.CreateText(GetFilePath()))
            {
                string finalString = "";
                for (int i = 0; i < m_headers.Length; i++)
                {
                    if (!string.IsNullOrEmpty(finalString))
                    {
                        finalString += m_separator;
                    }
                    finalString += m_headers[i];
                }
                sw.WriteLine(finalString);
            }
        }

        private string GetDirectoryPath()
        {
            return Application.dataPath + "/" + m_directoryName;
        }

        private string GetFilePath()
        {
            return GetDirectoryPath() + "/" + m_fileName;
        }
    }
}
