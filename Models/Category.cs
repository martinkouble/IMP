using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP_reseni.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Disabled { get; set; }
        public string ImageUrl { get; set; }

        public List<SubCategory> SubCategories { get; set; }
        public Category()
        {
            this.SubCategories = new List<SubCategory>();
        }
        public List<string> GetSubCategoriesNames(bool getDisabled = false)
        {
            List<string> output = new List<string>();
            foreach (SubCategory s in SubCategories)
                if (s.Disabled == false || (s.Disabled == true && getDisabled == true))
                    output.Add(s.Name);
            return output;
        }

        public SubCategory FindSubCategory(int subCategoryId)
        {
            return this.SubCategories.Find(f => f.Id == subCategoryId);
        }

    }
}
