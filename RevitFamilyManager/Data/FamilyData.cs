using System.Collections.Generic;

namespace RevitFamilyManager.Data
{
    public class FamilyData
    {
        public string Category { get; set; }
        public string FamilyName { get; set; }
        public string FamilyPath { get; set; }

        public List<FamilyTypeData> FamilyTypeDatas{ get; set; }

        public override string ToString()
        {
            return "ID: " + "\nFamily Name: " + this.FamilyName;
        }
    }
}
