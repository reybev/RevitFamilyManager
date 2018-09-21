using RevitFamilyManager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
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

            //if (string.IsNullOrEmpty(path))
            //{
            //    TaskDialog.Show("Warning", "Folder" + deviceType + " not found");
            //}
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
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //int nameIndex = userName.IndexOf(@"\");
            //userName = userName.Substring(nameIndex + 1);


            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileName = Path.Combine(path, "FamilyData.xml");
            //string xmlFileName = @"C:\Users\" + userName + @"\HHM\Deployment - General\Revit_Firma\2019\Database\FamilyData.xml";
            //string xmlFileName = @"P:\Revit_Firma\2019\Database\FamilyData.xml";

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ValidationType = ValidationType.DTD;

            XmlSerializer serializer = new XmlSerializer(typeof(List<FamilyData>));
            FileStream fs = new FileStream(xmlFileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs, settings);


            var familyList = (List<FamilyData>)serializer.Deserialize(reader);
            fs.Close();
            string pathDll = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            pathDll = pathDll.Substring(0, pathDll.Length - 4);
           
            //TODO
           
            //string sharepointPath = @"C:\Users\" + userName + @"\HHM\Deployment - General\Revit_Firma\2019\Revit Family\";
            string sharepointPath = @"P:\Revit_Firma\2019\Revit Family\";
            string revitVersion = Properties.Settings.Default.RevitVersion;
            foreach (var item in familyList)
            {
                if (item != null)
                {
                    int index = item.FamilyPath.IndexOf("HHM");
                    item.FamilyPath = item.FamilyPath.Substring(index + 4);
                    item.FamilyPath = Path.Combine(pathDll, revitVersion, "HHM", item.FamilyPath); // local path in Addin
                    //MessageBox.Show(item.FamilyPath);
                    //item.FamilyPath = Path.Combine(sharepointPath, item.FamilyPath); //path on server 
                }
            }
            return familyList;
        }

        public List<FamilyData> GetCategoryTypes(string categoryName)
        {
            List<FamilyData> filteredList = new List<FamilyData>();
            foreach (var item in ReadXML())
            {
                if (item == null) continue;
                if (item.Category == categoryName)
                {
                    filteredList.Add(item);
                }
            }
            return filteredList;
        }

        public List<FamilyData> GetFamilyTypes(string familyName)
        {
            List<FamilyData> filteredList = new List<FamilyData>();
            foreach (var item in ReadXML())
            {
                if (item == null) continue;
                if (item.FamilyName == familyName)
                {
                    filteredList.Add(item);
                }
            }

            return filteredList;
        }
    }
}

