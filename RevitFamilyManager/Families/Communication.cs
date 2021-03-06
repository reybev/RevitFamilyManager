﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using RevitFamilyManager.Data;

namespace RevitFamilyManager.Families
{
    [Transaction(TransactionMode.Manual)]
    class Communication : IExternalCommand
    {
        private string CategoryName { get; set; }

        public Communication()
        {
            CategoryName = "Kommunikationsgeräte";
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            FamilyFolderProcess folderProcess = new FamilyFolderProcess();
            List<FamilyData> familyData = folderProcess.GetCategoryTypes(CategoryName);
            SetPanelData(commandData, familyData);
            return Result.Succeeded;
        }
        
        private void SetPanelData(ExternalCommandData commandData, List<FamilyData> familyData)
        {
            DockablePaneId dpid = new DockablePaneId(new Guid("209923d1-7cdc-4a1c-a4ad-1e2f9aae1dc5"));
            DockablePane dp = commandData.Application.GetDockablePane(dpid);
            FamilyManagerDockable.WPFpanel.CategoryName.Content = " " + CategoryName + " ";
            FamilyManagerDockable.WPFpanel.ListFamilies = familyData;
            FamilyManagerDockable.WPFpanel.GenerateGrid(familyData);

            dp.Show();
        }
    }
}

