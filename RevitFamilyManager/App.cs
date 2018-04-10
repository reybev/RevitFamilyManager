#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitFamilyManager.Properties;

#endregion

namespace RevitFamilyManager
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            //Create ribbon tab
            string tabName = "Familien Manager";
            a.CreateRibbonTab(tabName);

            List<RibbonItem> familyButtons = new List<RibbonItem>();
            //Create buttons
            string path = Assembly.GetExecutingAssembly().Location;

            //1 Electrical Fixture - Electroinstallation
            PushButtonData buttonElectricalFixture = new PushButtonData("Electroinstallation", "Electroinstallation", path, "RevitFamilyManager.Families.ElectricalFixture");
            buttonElectricalFixture.ToolTip = "Shows telephon devices families";
            buttonElectricalFixture.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Electroinstallation.png"));

            //2 Communication - Kommunication
            PushButtonData buttonCommunication = new PushButtonData("Kommunication", "Kommunication", path, "RevitFamilyManager.Families.Communication");
            buttonCommunication.ToolTip = "Shows telephon devices families";
            buttonCommunication.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Kommunication.png"));

            //3 Data - Daten
            PushButtonData buttonData = new PushButtonData("Daten", "Daten", path, "RevitFamilyManager.Families.Data");
            buttonData.ToolTip = "Shows telephon devices families";
            buttonData.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Daten.png"));

            //4 FireAlarm - Brandmelder
            PushButtonData buttonFireAlarm = new PushButtonData("Brandmelder", "Brandmelder", path, "RevitFamilyManager.Families.FireAlarm");
            buttonFireAlarm.ToolTip = "Shows telephon devices families";
            buttonFireAlarm.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Brandmelder.png"));

            //5 Lighting - Lichtschalter
            PushButtonData buttonLighting = new PushButtonData("Lichtschalter", "Lichtschalter", path, "RevitFamilyManager.Families.Lighting");
            buttonLighting.ToolTip = "Shows telephon devices families";
            buttonLighting.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Lichtschalter.png"));

            //5 LightingFixtures - Leuchte
            PushButtonData buttonLightingFixtures = new PushButtonData("Leuchte", "Leuchte", path, "RevitFamilyManager.Families.LightingFixture");
            buttonLightingFixtures.ToolTip = "Shows telephon devices families";
            buttonLightingFixtures.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Leuchte.png"));

            //7 NurseCall - Notruf
            PushButtonData buttonNurseCall = new PushButtonData("Notruf", " Notruf ", path, "RevitFamilyManager.Families.NurceCall");
            buttonNurseCall.ToolTip = "Shows telephon devices families";
            buttonNurseCall.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Notruf.png"));

            //8 Security - Sicherheit
            PushButtonData buttonSecurity = new PushButtonData("Sicherheit", "Sicherheit", path, "RevitFamilyManager.Families.Security");
            buttonSecurity.ToolTip = "Shows telephon devices families";
            buttonSecurity.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Sicherheit.png"));

            //9 Phone - Telefon
            PushButtonData buttonPhone = new PushButtonData("Telephonegerate", " Telefon ", path, "RevitFamilyManager.Families.Phone");
            buttonPhone.ToolTip = "Shows telephon devices families";
            buttonPhone.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Telefon.png"));

            //10 Electroinstallation - Elektrische Ausstattung
            PushButtonData buttonElectroinstallation = new PushButtonData("ElektrischeAusstattung", "Elektrische\nAusstattung", path, "RevitFamilyManager.Families.Electroinstallation");
            buttonElectroinstallation.ToolTip = "Shows telephon devices families";
            buttonElectroinstallation.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\ElektrischeAusstattung.png"));


            //11 CableTray - Kabeltrasse
            PushButtonData buttonCableTrays = new PushButtonData("Kabeltrasse", "Kabeltrasse", path, "RevitFamilyManager.Families.CableTray");
            buttonCableTrays.ToolTip = "Shows telephon devices families";
            buttonCableTrays.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Kabeltrasse.png"));

            //12 CableTrayFitting - Kabeltrassenformteil
            PushButtonData buttonCableTrayFittings = new PushButtonData("Kabeltrassenformteil", "Kabeltrassen-\nformteil", path, "RevitFamilyManager.Families.CableTrayFitting");
            buttonCableTrayFittings.ToolTip = "Shows telephon devices families";
            buttonCableTrayFittings.LargeImage = new BitmapImage(new Uri(@"C:\Users\user\documents\visual studio 2017\Projects\RevitFamilyManager\RevitFamilyManager\Resources\Kabeltrassenformteil.png"));


            //Create ribbon panel
            RibbonPanel toolPanel = a.CreateRibbonPanel(tabName, "Tool");

            //Add buttons to panel
            RibbonItem ri1 = toolPanel.AddItem(buttonElectricalFixture);
            RibbonItem ri2 = toolPanel.AddItem(buttonCommunication);
            RibbonItem ri3 = toolPanel.AddItem(buttonData);
            RibbonItem ri4 = toolPanel.AddItem(buttonFireAlarm);
            RibbonItem ri5 = toolPanel.AddItem(buttonLighting);
            RibbonItem ri6 = toolPanel.AddItem(buttonLightingFixtures);
            RibbonItem ri7 = toolPanel.AddItem(buttonNurseCall);
            RibbonItem ri8 = toolPanel.AddItem(buttonSecurity);
            RibbonItem ri9 = toolPanel.AddItem(buttonPhone);
            RibbonItem ri10 = toolPanel.AddItem(buttonElectroinstallation);
            RibbonItem ri11 = toolPanel.AddItem(buttonCableTrays);
            RibbonItem ri12 = toolPanel.AddItem(buttonCableTrayFittings);

            familyButtons.Add(ri1);
            familyButtons.Add(ri2);
            familyButtons.Add(ri3);
            familyButtons.Add(ri4);
            familyButtons.Add(ri5);
            familyButtons.Add(ri6);
            familyButtons.Add(ri7);
            familyButtons.Add(ri8);
            familyButtons.Add(ri9);
            familyButtons.Add(ri10);
            familyButtons.Add(ri11);
            familyButtons.Add(ri12);

            //familyButtons.AddRange(toolPanel.AddStackedItems(buttonPhone, buttonLighting));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
