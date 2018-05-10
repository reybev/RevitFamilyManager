using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RevitFamilyManager.Data;

namespace RevitFamilyManager
{
    public partial class FamilyManagerDockable : UserControl, IDockablePaneProvider
    {
        private const string AllCategories = "Alle Kategorien";
        private const string SelectCategory = "Wählen Sie Medium";

        private ExternalEvent m_ExEvent;
        private SingleInstallEvent m_Handler;

        public static FamilyManagerDockable WPFpanel;

        public List<FamilyData> ListFamilies { get; set; }
        public List<FamilyTypeData> ListFamilyTypes { get; set; }

        public FamilyManagerDockable(ExternalEvent exEvent, SingleInstallEvent handler)
        {
            InitializeComponent();
            m_ExEvent = exEvent;
            m_Handler = handler;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState.DockPosition = DockPosition.Left;
        }

        public void GenerateGrid(List<FamilyData> listFamilyData)
        {
            // --- Reset Buttons State ---
            this.BtnOverheadFilter.IsChecked = false;
            this.BtnRecessedFilter.IsChecked = false;
            this.BtnCeilingFilter.IsChecked = false;
            this.BtnWallFilter.IsChecked = false;
            this.BtnFloorFilter.IsChecked = false;
            this.ComboBoxInstallationMedium.SelectedItem = null;
            this.ComboBoxInstallationMedium.Text = AllCategories;
            this.ComboBoxInstallationMedium.SelectedIndex = 0;
            // ---

            List<FamilyTypeData> familyTypes = new List<FamilyTypeData>();

            ListFamilies = listFamilyData;
            foreach (var item in ListFamilies)
            {
                foreach (var type in item.FamilyTypeDatas)
                {
                    familyTypes.Add(type);
                }
            }

            ListFamilyTypes = familyTypes;
            ObservableCollection<FamilyTypeData> collectionFamilyTypes = new ObservableCollection<FamilyTypeData>(familyTypes);
            FamiliesDataGrid.RowBackground = Brushes.WhiteSmoke;
            FamiliesDataGrid.ItemsSource = collectionFamilyTypes;
            GetInstallationMedium();
        }

        #region Filters Methods
        private List<FamilyTypeData> FilterMountType(string mountParameter)
        {
            List<FamilyTypeData> filteredFamilies = new List<FamilyTypeData>();
            foreach (var item in ListFamilyTypes)
            {
                if (item.MountType == mountParameter)
                {
                    filteredFamilies.Add(item);
                }
            }
            return filteredFamilies;
        }

        private List<FamilyTypeData> FilterPlacement(string placementParameter)
        {
            List<FamilyTypeData> filteredFamilies = new List<FamilyTypeData>();
            foreach (var item in ListFamilyTypes)
            {
                if (item.Placement == placementParameter)
                {
                    filteredFamilies.Add(item);
                }
            }
            return filteredFamilies;
        }

        private List<FamilyTypeData> FilterInstallationMedium()
        {
            List<FamilyTypeData> filteredInstallationMedium = new List<FamilyTypeData>();
            foreach (var item in ListFamilyTypes)
            {
                if (item.InstallationMedium != null && this.ComboBoxInstallationMedium.SelectedItem != null)
                {
                    if (item.InstallationMedium.Contains(this.ComboBoxInstallationMedium.SelectedItem.ToString()))
                    {
                        filteredInstallationMedium.Add(item);
                    }
                    else if (this.ComboBoxInstallationMedium.SelectedItem.ToString().Contains(AllCategories))
                    {
                        filteredInstallationMedium.Add(item);
                    }
                }
            }
            return filteredInstallationMedium;
        }
        #endregion

        private void GetInstallationMedium()
        {
            List<string> installationMediumCategories = new List<string>();
            installationMediumCategories.Add(AllCategories);
            foreach (var item in ListFamilyTypes)
            {
                installationMediumCategories.Add(item.InstallationMedium);
                installationMediumCategories = installationMediumCategories.Distinct().ToList();
                this.ComboBoxInstallationMedium.ItemsSource = installationMediumCategories;
            }
        }

        private void FilterFamily()
        {
            List<FamilyTypeData> filterMount = new List<FamilyTypeData>();
            List<FamilyTypeData> filterPlacement = new List<FamilyTypeData>();
            List<FamilyTypeData> filterInstallationMedium = FilterInstallationMedium();

            if (this.BtnOverheadFilter.IsChecked == false)
            {
                filterMount.AddRange(FilterMountType("AP"));
            }

            if (this.BtnRecessedFilter.IsChecked == false)
            {
                filterMount.AddRange(FilterMountType("UP"));
            }

            if (this.BtnOverheadFilter.IsChecked == true && BtnRecessedFilter.IsChecked == true)
            {
                filterMount.AddRange(FilterMountType("AP"));
                filterMount.AddRange(FilterMountType("UP"));
            }

            filterMount.AddRange(FilterMountType(" --- "));

            if (this.BtnCeilingFilter.IsChecked == true)
            {
                filterPlacement.AddRange(FilterPlacement("Decke"));
            }

            if (this.BtnWallFilter.IsChecked == true)
            {
                filterPlacement.AddRange(FilterPlacement("Wand"));
            }

            if (this.BtnFloorFilter.IsChecked == true)
            {
                filterPlacement.AddRange(FilterPlacement("Boden"));
            }

            if (this.BtnCeilingFilter.IsChecked == false && this.BtnWallFilter.IsChecked == false &&
                this.BtnFloorFilter.IsChecked == false)
            {
                filterPlacement.AddRange(FilterPlacement("Decke"));
                filterPlacement.AddRange(FilterPlacement("Wand"));
                filterPlacement.AddRange(FilterPlacement("Boden"));
            }

            filterPlacement.AddRange(FilterPlacement(" --- "));

            var filteredList = filterMount.Intersect(filterPlacement);
            filteredList = filteredList.Intersect(filterInstallationMedium);
            ObservableCollection<FamilyTypeData> fd = new ObservableCollection<FamilyTypeData>(filteredList);

            FamiliesDataGrid.ItemsSource = fd;
        }

        #region Filter Button Action

        // --- Installation Medium ---
        private void ComboBoxInstallationMedium_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            FilterFamily();
        }
        // --- UP ---
        private void BtnOverheadFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        private void BtnOverheadFilter_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        // --- AP ---
        private void BtnRecessedFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        private void BtnRecessedFilter_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        // --- D --- Decke
        private void BtnCeilingFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        private void BtnCeilingFilter_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        // --- W --- Wande
        private void BtnWallFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        private void BtnWallFilter_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        // --- B --- Boden
        private void BtnFloorFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }

        private void BtnFloorFilter_OnUnchecked(object sender, RoutedEventArgs e)
        {
            FilterFamily();
        }
        #endregion
        
        private string FileNameCut(string file)
        {
            int lastSlash = file.LastIndexOf("\\", StringComparison.Ordinal);
            int lastDot = file.LastIndexOf(".", StringComparison.Ordinal);
            string fileName = file.Substring(++lastSlash, lastDot - lastSlash);
            return fileName;
        }

        private void FamiliesDataGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.FamilyType))
                m_ExEvent.Raise();
        }

        private void SetProperty(FamilyTypeData instance)
        {
            Properties.Settings.Default.FamilyPath = instance.Path;
            Properties.Settings.Default.FamilyType = instance.Name;
            Properties.Settings.Default.FamilyName = FileNameCut(instance.Path);
        }

        private void ClearSelectedType()
        {
            Properties.Settings.Default.FamilyPath = string.Empty;
            Properties.Settings.Default.FamilyType = string.Empty;
            Properties.Settings.Default.FamilyName = string.Empty;
        }

        private void FamiliesDataGrid_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ClearSelectedType();
        }

        private void FamiliesDataGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (FamiliesDataGrid.Items.Count <= 0) return;
            var instance = this.FamiliesDataGrid.SelectedItem as FamilyTypeData;
            if (instance != null)
            {
                SetProperty(instance);
            }
        }

        
    }
}
