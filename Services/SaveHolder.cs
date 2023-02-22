﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP_reseni.Models;
using Newtonsoft.Json;


namespace IMP_reseni.Services
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class SaveHolder
    {
        public List<Category> Inventory { get;  set; }
        public List<Supplier> Suppliers { get;  set; }
        public List<StockUpItem> StockUpHistory { get; set; }
        public List<Order> OrderHistory { get; set; }

        public List<Category> CreateInventoryCopy(List<Category> inventory = null)
        {
            if (inventory == null)
                inventory = this.Inventory;
            List<Category> output = new List<Category>();
            foreach (Category c in inventory)
                output.Add(c.MakeCopy());
            return output;
        }

        public SaveHolder()
        {
            Inventory = new List<Category>();
            Suppliers = new List<Supplier>();
            StockUpHistory= new List<StockUpItem>();
            OrderHistory=new List<Order>();
    }

    private void SetSaveHolderProperties(SaveHolder save)
        {
            this.Inventory = save.Inventory;
            this.OrderHistory = save.OrderHistory;
            this.StockUpHistory = save.StockUpHistory;
            this.Suppliers = save.Suppliers;
        }

        public void Save()
        {
            string _appDirectory = FileSystem.Current.AppDataDirectory;
            string _fileName = "IMP.json";
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            string jsonString = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(_appDirectory + "/" + _fileName, jsonString);
        }

        public void Save(string name)
        {
            string cacheDir = FileSystem.Current.CacheDirectory;
            string _appDirectory = FileSystem.Current.AppDataDirectory;
            string _fileName = name;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            string jsonString = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(cacheDir + "/" + _fileName, jsonString);          
        }



        public void Load()
        {
            string _appDirectory = FileSystem.Current.AppDataDirectory;
            string _fileName = "IMP.json";
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            if (File.Exists(_appDirectory + "/" + _fileName))
            {
                string json = File.ReadAllText(_appDirectory + "/" + _fileName);
                SaveHolder save = JsonConvert.DeserializeObject<SaveHolder>(json, settings);
                SetSaveHolderProperties(save);
            }
        }
        public void Load(string name)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            if (File.Exists(name))
            {
                string json = File.ReadAllText(name);
                SaveHolder save = JsonConvert.DeserializeObject<SaveHolder>(json, settings);
                SetSaveHolderProperties(save);
            }
        }


        //Category
        public void AddCategory(Category category)
        {
            TryToSetCatId(category);
            Inventory.Add(category);
        }

        private void TryToSetCatId(Category category)
        {
            if (category.Id == default(int))
                SetCatId(category);
        }
        private void SetCatId(Category category)
        {
            if (Inventory.Count > 0)
            {
                int maxId = Inventory.Max(o => o.Id);
                category.Id = maxId + 1;
            }
            else
                category.Id = 1;
        }
        public Category FindCategory(int categoryId)
        {
            return this.Inventory.Find(f => f.Id == categoryId);
        }
        public Category FindCategoryByName(string categoryName)
        {
            return this.Inventory.Find(f => f.Name == categoryName);
        }
        public List<string> GetCategoriesNames(bool getDisabled = false)
        {
            List<string> output = new List<string>();
            foreach (Category c in Inventory)
                if (c.Disabled == false || (c.Disabled == true && getDisabled == true))
                    output.Add(c.Name);
            return output;
        }
        public List<string> GetCategoriesIds(bool getDisabled = false)
        {
            List<string> output = new List<string>();
            foreach (Category c in Inventory)
                if (c.Disabled == false || (c.Disabled == true && getDisabled == true))
                    output.Add(c.Id.ToString());
            return output;
        }
        public void ModifyCategory(Category category)
        {
           int index= Inventory.FindIndex(f => f.Id == category.Id);
           if(index != -1)
              Inventory[index] = category;
        }

        //Subcategory

        public void AddSubCategory(int categoryId, SubCategory subCategory)
        {
            FindCategory(categoryId).AddSubCategory(subCategory);
        }

        public void ModifySubCategory(Category category,SubCategory subCategory)
        {
            int index = Inventory.FindIndex(f => f.Id == category.Id);
            if (index != -1)
            {
               int subCategoryIndex= Inventory[index].SubCategories.FindIndex(f => f.Id == subCategory.Id);
               Inventory[index].SubCategories[subCategoryIndex] = subCategory;
            }
        }
        //Item
        public void AddItem(int categoryId, int subCategoryId, Items item)
        {
            FindCategory(categoryId).FindSubCategory(subCategoryId).AddItem(item);
        }

        public void ModifyItem(Category category, SubCategory subCategory,Items item)
        {
            int index = Inventory.FindIndex(f => f.Id == category.Id);
            if (index != -1)
            {
                int subCategoryIndex = Inventory[index].SubCategories.FindIndex(f => f.Id == subCategory.Id);
                int itemIndex = Inventory[index].SubCategories[subCategoryIndex].Items.FindIndex(f => f.Id == item.Id);
                Inventory[index].SubCategories[subCategoryIndex].Items[itemIndex] = item;
            }
        }
        //Supplier
        public void AddSupplier(Supplier supplier)
        {
            TryToSetSupplierId(supplier);
            Suppliers.Add(supplier);
        }
        private void TryToSetSupplierId(Supplier supplier)
        {
            if (supplier.Id == default(int))
                SetSupplierId(supplier);
        }
        private void SetSupplierId(Supplier supplier)
        {
            if (Suppliers.Count > 0)
            {
                int maxId = Suppliers.Max(o => o.Id);
                supplier.Id = maxId + 1;
            }
            else
                supplier.Id = 1;
        }
        public Supplier FindSupplierByName(string supplierName)
        {
            return this.Suppliers.Find(f => f.Name == supplierName);
        }

        public Supplier FindSupplier(int supplierId)
        {
            return this.Suppliers.Find(f => f.Id == supplierId);
        }
        public List<string> GetSupplierNames()
        {
            List<string> output = new List<string>();
            foreach (Supplier s in Suppliers)
                output.Add(s.Name);
            return output;
        }

        public void ModifySupplier(Supplier supplier)
        {
            int index = Inventory.FindIndex(f => f.Id == supplier.Id);
            if (index != -1)
                Suppliers[index] = supplier;
        }

        //StockUpItem
        public void AddToStockUpHistory(StockUpItem stockUp)
        {
            StockUpHistory.Add(stockUp);
            this.Save();
        }
        public void RemoveToStockUpHistory(StockUpItem stockUp)
        {
            StockUpHistory.Remove(stockUp);
            this.Save();
        }

        //OrderHistory
        public void AddToOrderHistory(Order order)
        {
            OrderHistory.Add(order);
            this.Save();
        }
    }
}