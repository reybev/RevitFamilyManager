using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string IfcExportAs { get; set; }
        public string IfcExportType { get; set; }
    }
}
