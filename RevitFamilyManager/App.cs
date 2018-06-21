#region Namespaces
using Autodesk.Revit.UI;
using RevitFamilyManager.Properties;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using RevitFamilyManager.Data;

#endregion

namespace RevitFamilyManager
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            //string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string xmlFileName = Path.Combine(assemblyFolder, "FamilyData.xml");
            //if (!File.Exists(xmlFileName))
            //{
            //    XmlWriter writer = XmlWriter.Create(xmlFileName);
            //}
            DownloadDataBase();
            //Create ribbon tab
            string tabName = "Familien Manager";
            a.CreateRibbonTab(tabName);

            #region Ribbon buttons
            //Create buttons
            string path = Assembly.GetExecutingAssembly().Location;

            //1 Electrical Fixture - Elektroinstallationen
            PushButtonData buttonElectricalFixture = new PushButtonData("Elektroinstallationen", "Elektro-\ninstallationen", path, "RevitFamilyManager.Families.ElectricalFixture");
            buttonElectricalFixture.ToolTip = "Shows telephon devices families";
            buttonElectricalFixture.LargeImage = GetImage(Resources.Electroinstallation.GetHbitmap());

            //2 Communication - Kommunikationsgeräte
            PushButtonData buttonCommunication = new PushButtonData("Kommunikationsgeräte", "Kommunikations-\ngeräte", path, "RevitFamilyManager.Families.Communication");
            buttonCommunication.ToolTip = "Shows telephon devices families";
            buttonCommunication.LargeImage = GetImage(Resources.Kommunication.GetHbitmap());

            //3 Data - Datengeräte
            PushButtonData buttonData = new PushButtonData("Datengeräte", "Daten-\ngeräte", path, "RevitFamilyManager.Families.Data");
            buttonData.ToolTip = "Shows telephon devices families";
            buttonData.LargeImage = GetImage(Resources.Daten.GetHbitmap());

            //4 FireAlarm - Brandmeldegeräte
            PushButtonData buttonFireAlarm = new PushButtonData("Brandmeldegeräte", "Brandmelde-\ngeräte", path, "RevitFamilyManager.Families.FireAlarm");
            buttonFireAlarm.ToolTip = "Shows telephon devices families";
            buttonFireAlarm.LargeImage = GetImage(Resources.Brandmelder.GetHbitmap());

            //5 Lighting - Lichtschalter
            PushButtonData buttonLighting = new PushButtonData("Lichtschalter", "Lichtschalter", path, "RevitFamilyManager.Families.Lighting");
            buttonLighting.ToolTip = "Shows telephon devices families";
            buttonLighting.LargeImage = GetImage(Resources.Lichtschalter.GetHbitmap());

            //5 LightingFixtures - Leuchten
            PushButtonData buttonLightingFixtures = new PushButtonData("Leuchten", "Leuchten", path, "RevitFamilyManager.Families.LightingFixture");
            buttonLightingFixtures.ToolTip = "Shows telephon devices families";
            buttonLightingFixtures.LargeImage = GetImage(Resources.Leuchte.GetHbitmap());

            //7 NurseCall - Notrufgeräte
            PushButtonData buttonNurseCall = new PushButtonData("Notrufgeräte", " Notruf-\ngeräte ", path, "RevitFamilyManager.Families.NurceCall");
            buttonNurseCall.ToolTip = "Shows telephon devices families";
            buttonNurseCall.LargeImage = GetImage(Resources.Notruf.GetHbitmap());

            //8 Security - Sicherheitsgeräte
            PushButtonData buttonSecurity = new PushButtonData("Sicherheitsgeräte", "Sicherheits-\ngeräte", path, "RevitFamilyManager.Families.Security");
            buttonSecurity.ToolTip = "Shows telephon devices families";
            buttonSecurity.LargeImage = GetImage(Resources.Sicherheit.GetHbitmap());

            //9 Phone - Telefongeräte
            PushButtonData buttonPhone = new PushButtonData("Telefongeräte", " Telefon-\ngeräte ", path, "RevitFamilyManager.Families.Phone");
            buttonPhone.ToolTip = "Shows telephon devices families";
            buttonPhone.LargeImage = GetImage(Resources.Telefon.GetHbitmap());

            //10 Electroinstallation - Elektrische Ausstattung
            PushButtonData buttonElectroinstallation = new PushButtonData("ElektrischeAusstattung", "Elektrische\nAusstattung", path, "RevitFamilyManager.Families.Electroinstallation");
            buttonElectroinstallation.ToolTip = "Shows telephon devices families";
            buttonElectroinstallation.LargeImage = GetImage(Resources.ElektrischeAusstattung.GetHbitmap());

            //11 Annotation - Beschriftungen
            PushButtonData buttonAnnotation = new PushButtonData("Beschriftungen", "Beschriftungen", path, "RevitFamilyManager.Families.Descriptions");
            buttonAnnotation.ToolTip = "Shows telephon devices families";
            buttonAnnotation.LargeImage = GetImage(Resources.Description.GetHbitmap());

            //12 CableTrayFitting - Leerrohrformteile
            PushButtonData buttonCableTrayFittings = new PushButtonData("Leerrohrformteile", "Leerrohrform-\nteile", path, "RevitFamilyManager.Families.CableTrayFitting");
            buttonCableTrayFittings.ToolTip = "Shows telephon devices families";
            buttonCableTrayFittings.LargeImage = GetImage(Resources.Kabeltrassenformteil.GetHbitmap());

            //13 Earthing - Erdung
            PushButtonData buttonEarthing = new PushButtonData("Erdung", "Erdung", path, "RevitFamilyManager.Families.Earthing");
            buttonEarthing.ToolTip = "Shows earthing families";
            buttonEarthing.LargeImage = GetImage(Resources.Erdnung.GetHbitmap());

            //14 GenericModels - Allgemeines Modell
            PushButtonData buttonGenericModels = new PushButtonData("Allgemeines Modell", "Allgemeines\nModell", path, "RevitFamilyManager.Families.GenericModels");
            buttonGenericModels.ToolTip = "User Preferences";
            buttonGenericModels.LargeImage = GetImage(Resources.GenericModels.GetHbitmap());

            //15 Legend - Legende
            PushButtonData buttonLegend = new PushButtonData("Legende", "Legende", path, "RevitFamilyManager.Families.Legend");
            buttonLegend.ToolTip = "Legend families";
            buttonLegend.LargeImage = GetImage(Resources.Legende.GetHbitmap());

            //15 Cable Trays - Kabeltrassen
            PushButtonData buttonCables = new PushButtonData("Kabeltrassen", "Kabeltrassen", path, "RevitFamilyManager.Families.CableTrays");
            buttonCables.ToolTip = "Cable trays families";
            buttonCables.LargeImage = GetImage(Resources.Kabeltrasse.GetHbitmap());

            //---------------------------------------------------------------------
            //14 Settings
            PushButtonData buttonSettings = new PushButtonData("Settings", "Familien Ordner", path, "RevitFamilyManager.UserSettings");
            buttonSettings.ToolTip = "User Preferences";
            buttonSettings.LargeImage = GetImage(Resources.Settings.GetHbitmap());

            //15 UpdateDB
            PushButtonData buttonUpdateDb = new PushButtonData("Update DB", "Datenbank\naktualisieren", path, "RevitFamilyManager.Data.UpdateDB");
            buttonUpdateDb.ToolTip = "UpdateDB";
            buttonUpdateDb.LargeImage = GetImage(Resources.UpdateDB.GetHbitmap());

            //16 Create Type Projects ----/Developer tool for Web Application
            PushButtonData buttonCreateProjects = new PushButtonData("ProjectCreator", "CreateProject", path, "RevitFamilyManager.Data.ProjectCreator");
            buttonCreateProjects.ToolTip = "Create Projects From Family Type";


            //Create ribbon panel
            RibbonPanel toolPanel = a.CreateRibbonPanel(tabName, "Familien Kategorien");

            //Add buttons to panel
            toolPanel.AddItem(buttonElectricalFixture);
            toolPanel.AddItem(buttonElectroinstallation);
            toolPanel.AddItem(buttonCables);
            toolPanel.AddSeparator();

            toolPanel.AddItem(buttonLighting);
            toolPanel.AddItem(buttonLightingFixtures);
            toolPanel.AddSeparator();

            toolPanel.AddItem(buttonCommunication);
            toolPanel.AddItem(buttonData);
            toolPanel.AddItem(buttonPhone);
            toolPanel.AddSeparator();

            toolPanel.AddItem(buttonNurseCall);
            toolPanel.AddItem(buttonSecurity);
            toolPanel.AddItem(buttonFireAlarm);
            toolPanel.AddItem(buttonEarthing);
            
            toolPanel.AddSeparator();
            
            // toolPanel.AddItem(buttonCableTrayFittings);
            toolPanel.AddItem(buttonGenericModels);
            //toolPanel.AddItem(buttonAnnotation);
            toolPanel.AddItem(buttonLegend);

            ///////////////////////////////////////////////
            //----Dev Tools---
            //////////////////////////////////////////////

            //RibbonPanel settingsPanel = a.CreateRibbonPanel(tabName, "Einstellungen");
            //settingsPanel.AddItem(buttonSettings);
            //settingsPanel.AddItem(buttonUpdateDb);
            //settingsPanel.AddItem(buttonCreateProjects);

            //////////////////////////////////////////////
            #endregion
            //Registering Docking panel
            SingleInstallEvent handler = new SingleInstallEvent();
            ExternalEvent exEvent = ExternalEvent.Create(handler);
            FamilyManagerDockable dock = new FamilyManagerDockable(exEvent, handler);
            //new FamilyManagerDockable();
            FamilyManagerDockable.WPFpanel = dock;

            DockablePaneProviderData data = new DockablePaneProviderData();
            dock.SetupDockablePane(data);

            DockablePaneId dpId = new DockablePaneId(new Guid("209923d1-7cdc-4a1c-a4ad-1e2f9aae1dc5"));
            a.RegisterDockablePane(dpId, "Familien Manager", dock);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        private static BitmapSource GetImage(IntPtr bm)
        {
            BitmapSource bmSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bm,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return bmSource;
        }

        private void DownloadDataBase()
        {
            //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string xmlFileName = Path.Combine(path, "FamilyData.xml");

            //string link =
            //    @"https://forgefiles.blob.core.windows.net/forgefiles/FamilyData.xml";
            //using (var client = new WebClient())
            //{
            //    client.DownloadFile(link, xmlFileName);
            //}
        }

    }
}
