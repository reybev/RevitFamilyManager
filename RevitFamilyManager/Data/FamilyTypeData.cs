using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace RevitFamilyManager.Data
{
    public class FamilyTypeData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Placement { get; set; }
        public string MountType { get; set; }
        public string InstallationMedium { get; set; }
        public string Path { get; set; }
        public string Image { get; set; }
        public string CombinedTypeData { get; set; }
        public string Diameter { get; set; }
        public string Width { get; set; }
        public string Hight { get; set; }
        public string Depth { get; set; }
        public string eBKP_H { get; set; }
        public string BKP { get; set; }
        public string Manufacturer { get; set; }
        public string Product { get; set; }
        public string ProductNumber { get; set; }
        public string E_Number { get; set; }
        public string RevitCategory { get; set; }
        public string OmniClass { get; set; }

        [XmlIgnore]
        public Uri ImageUri { get; set; }

        [XmlElement("ImageUri")]
        //public string MyURIAsString
        //{
        //    get { return ImageUri != null ? ImageUri.AbsoluteUri : null; }
        //    set { ImageUri = value != null ? new Uri(value) : null; }
        //}

        public string IfcExportAs { get; set; }
        public string IfcExportType { get; set; }
       
    }

   
}
