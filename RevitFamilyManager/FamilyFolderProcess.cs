using Autodesk.Revit.UI;
using RevitFamilyManager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace RevitFamilyManager
{
    class FamilyFolderProcess
    {
        public string GetDeviceFolder(string deviceType)
        {
            UserSettings userSettings = new UserSettings();
            if (string.IsNullOrEmpty(Properties.Settings.Default.RootFolder))
            {
                userSettings.GetStartFolder();
            }
            string[] allPaths = Directory.GetDirectories(Properties.Settings.Default.RootFolder);
            string path = string.Empty;
            foreach (string folder in allPaths)
            {
                if (folder.Contains(deviceType))
                {
                    path = folder;
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                TaskDialog.Show("Warning", "Folder \"" + deviceType + "\" not found");
            }
            return path;
        }

        public List<FamilyData> GetFamilyData(string path)
        {
            List<FamilyData> familyDataList = new List<FamilyData>();
            foreach (string file in Directory.GetFiles(path))
            {
                if (FileIsFamilyType(file))
                {
                    FamilyData familyItem = new FamilyData();
                    familyItem.Category = FamilyCategoryCut(file);
                    familyItem.FamilyPath = file;
                    familyItem.FamilyName = FileNameCut(file);
                    familyDataList.Add(familyItem);
                }
            }
            return familyDataList;
        }

        private string FamilyCategoryCut(string file)
        {
            int lastSlash = file.LastIndexOf("\\", StringComparison.Ordinal);
            string category = file.Substring(0, lastSlash);
            lastSlash = category.LastIndexOf("\\", StringComparison.Ordinal);
            category = category.Substring(lastSlash+1);
            return category;
        }

        private string FileNameCut(string file)
        {
            int lastSlash = file.LastIndexOf("\\", StringComparison.Ordinal);
            int lastDot = file.LastIndexOf(".", StringComparison.Ordinal);
            string fileName = file.Substring(++lastSlash, lastDot - lastSlash);
            return fileName;
        }

        private bool FileIsFamilyType(string file)
        {
            int lastDot = file.LastIndexOf(".", StringComparison.Ordinal);
            string fileType = file.Substring(lastDot);
            return fileType.Contains("rfa");
        }

        private List<FamilyData> ReadXML()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileName = Path.Combine(assemblyFolder, "FamilyData.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<FamilyData>));
            FileStream fs = new FileStream(xmlFileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            var familyList = (List<FamilyData>)serializer.Deserialize(reader);
            fs.Close();
            return familyList;
        }

        public List<FamilyData> GetCategoryTypes(string categoryName)
        {
            string temp = string.Empty;
            List<FamilyData> filteredList = new List<FamilyData>();
            foreach (var item in ReadXML())
            {
                if (item != null)
                {
                    if (item.Category == categoryName)
                    {
                        filteredList.Add(item);
                        temp += item.Category + " || " + item.FamilyName + "\n";
                    }
                }
            }

            //TaskDialog.Show("Filtered", temp);
            return filteredList;
        }


    }
}

