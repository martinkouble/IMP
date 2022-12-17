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

        public Category MakeCopy()
        {
            Category output = new Category();
            output.Id = this.Id;
            output.Name = this.Name;
            output.Disabled = this.Disabled;
            //output.Color = this.Color;
            foreach (SubCategory s in this.SubCategories)
                output.AddSubCategory(s.MakeCopy());
            return output;
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
        public void AddSubCategory(SubCategory subCategory)
        {
            if (SubCategories == null)
                SubCategories = new List<SubCategory>();
            TryToSetSubId(subCategory);
            SubCategories.Add(subCategory);
        }

        private void TryToSetSubId(SubCategory subCategory)
        {
            if (subCategory.Id == default(int))
                SetSubId(subCategory);
        }
        private void SetSubId(SubCategory subCategory)
        {
            if (SubCategories.Count > 0)
            {
                int maxId = SubCategories.Max(o => o.Id);
                subCategory.Id = maxId + 1;
            }
            else
                subCategory.Id = 1;
        }
    }
}
