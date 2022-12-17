using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP_reseni.Models;

namespace IMP_reseni.Services
{
    public class AccountingHandler
    {

        public string CreateAccountingFileContent(DateTime fromDate, DateTime toDate, List<Category> inventory, List<Order> orderHistory, List<StockUpItem> StockUpHistory, List<Supplier> suppliers)
        {
            string output = "";
            List<Category> startInventory = ReverseInventory(fromDate, inventory, orderHistory, StockUpHistory);
            List<Category> endInventory = ReverseInventory(toDate, inventory, orderHistory, StockUpHistory);
            List<Order> orderRange = GetOrderHistoryRange(fromDate, toDate, orderHistory);
            List<StockUpItem> stockUpRange = GetStockUpHistoryRange(fromDate, toDate, StockUpHistory);
            List<AccountingItem> accItems = CalculateAccountingItems(startInventory, orderRange, stockUpRange);
            List<AccountingSupplier> sortedAccItems = SortAccountingItems(accItems, suppliers);
            output = CreateOutputString(sortedAccItems, fromDate, toDate);
            return output;
        }
        public string CreateAccountingStockFileContent(DateTime fromDate, DateTime toDate, List<Category> inventory, List<Order> orderHistory, List<StockUpItem> StockUpHistory, List<Supplier> suppliers)
        {
            string output = "";
            List<Category> startInventory = ReverseInventory(fromDate, inventory, orderHistory, StockUpHistory);
            List<Category> endInventory = ReverseInventory(toDate, inventory, orderHistory, StockUpHistory);
            List<Order> orderRange = GetOrderHistoryRange(fromDate, toDate, orderHistory);
            List<StockUpItem> stockUpRange = GetStockUpHistoryRange(fromDate, toDate, StockUpHistory);
            List<AccountingItem> accItems = CalculateAccountingItems(startInventory, orderRange, stockUpRange);
            List<AccountingSupplier> sortedAccItems = SortAccountingItems(accItems, suppliers);
            output = CreateStockOutputString(sortedAccItems, fromDate, toDate);
            return output;
        }
        private string CreateStockOutputString(List<AccountingSupplier> sortedAccItems, DateTime fromDate, DateTime toDate)
        {
            //string output = "";
            Excel startExcel = new Excel();
            Excel excel = new Excel();

            foreach (AccountingSupplier s in sortedAccItems)
            {
                excel.WriteLine(SupplierStockLine(s.Supplier));
                foreach (AccountingItem i in s.Items)
                {
                    excel.WriteLine(AccountingStockItemLine(i));
                }
                excel.WriteLine("");
            }
            startExcel.WriteLine(DateLine(fromDate, toDate));
            return Excel.CombineFiles(startExcel, excel).Content;
        }
        private string CreateOutputString(List<AccountingSupplier> sortedAccItems, DateTime fromDate, DateTime toDate)
        {
            //string output = "";
            Excel startExcel = new Excel();
            Excel excel = new Excel();
            double totalShopNetSale = 0;
            foreach (AccountingSupplier s in sortedAccItems)
            {
                double totalNetSale = 0;
                excel.WriteLine(SupplierLine(s.Supplier));
                foreach (AccountingItem i in s.Items)
                {
                    totalNetSale += i.NetSale;
                    excel.WriteLine(AccountingItemLine(i));
                }
                excel.WriteLine(SupplierEndLine(totalNetSale));
                excel.WriteLine("");
                totalShopNetSale += totalNetSale;
            }
            startExcel.WriteLine(DateLine(fromDate, toDate));
            startExcel.WriteLine(TotalShopNetSaleLine(totalShopNetSale));
            return Excel.CombineFiles(startExcel, excel).Content;
        }
        private string TotalShopNetSaleLine(double totalShopNetSale)
        {
            return "Celková čistá tržba:;" + Math.Round(totalShopNetSale, 2) + " Kč;";
        }
        private string DateLine(DateTime fromDate, DateTime toDate)
        {
            return "Rozmezí:;" + fromDate.ToShortDateString() + " - " + toDate.ToShortDateString() + ";;";
        }
        private string SupplierLine(Supplier supplier)
        {
            return supplier.Name + ";Prodáno;Zbývá;Nák./ks;Prod./ks;Čistá tržba celkem;";
        }
        private string SupplierStockLine(Supplier supplier)
        {
            return supplier.Name + ";Prodáno;Zbývá;";
        }
        private string AccountingItemLine(AccountingItem item)
        {
            return item.Item.Name + ";" + item.SoldAmount + " ks;" + item.Remaining + " ks;" + item.BuyCostText + ";" + item.SellCostText + ";" + item.NetSale + " Kč";
        }
        private string AccountingStockItemLine(AccountingItem item)
        {
            return item.Item.Name + ";" + item.SoldAmount + " ks;" + item.Remaining + " ks;";
        }
        private string SupplierEndLine(double totalNetSale)
        {
            return ";;;;Celkem:;" + Math.Round(totalNetSale, 2) + " Kč";
        }
        private List<AccountingItem> CalculateAccountingItems(List<Category> inventory, List<Order> orderHistory, List<StockUpItem> stockUpHistory)
        {
            List<AccountingItem> accItems = new List<AccountingItem>();
            SaveHolder svh = new SaveHolder();
            svh.Inventory = svh.CreateInventoryCopy(inventory);
            foreach (Category c in svh.Inventory)
                foreach (SubCategory s in c.SubCategories)
                    foreach (Items i in s.Items)
                    {
                        AccountingItem found = new AccountingItem();
                        found.supplierId = i.SupplierId;
                        found.categoryId = c.Id;
                        found.subCategoryId = s.Id;
                        found.itemId = i.Id;
                        found.StartAmount = i.Stock;
                        found.Remaining = i.Stock;
                        found.BuyCost.Add(i.BuyCost);
                        found.SellCost.Add(i.SellCost);
                        accItems.Add(found);
                    }
            foreach (Order o in orderHistory)
            {
                foreach (OrderItem i in o.Items)
                {
                    AccountingItem found = accItems.Find(x => x.categoryId == i.CategoryId && x.subCategoryId == i.SubCategoryId && x.itemId == i.ItemId);
                    found.SoldAmount += i.Amount;
                    found.Remaining -= i.Amount;
                    if (!found.BuyCost.Contains(i.BuyCostPerPiece))
                        found.BuyCost.Add(i.BuyCostPerPiece);
                    if (!found.SellCost.Contains(i.SellCostPerPiece))
                        found.SellCost.Add(i.SellCostPerPiece);
                    found.NetSale += (i.SellCostPerPiece - i.BuyCostPerPiece) * i.Amount;
                }
            }
            foreach (StockUpItem i in stockUpHistory)
            {
                AccountingItem found = accItems.Find(x => x.categoryId == i.CategoryId && x.subCategoryId == i.SubCategoryId && x.itemId == i.ItemId);
                found.Remaining += i.Amount;
            }
            return accItems;
        }
        private List<Category> ReverseInventory(DateTime date, List<Category> inventory, List<Order> orderHistory, List<StockUpItem> stockUpHistory)
        {
            SaveHolder saveHolder = new SaveHolder();
            saveHolder.Inventory = saveHolder.CreateInventoryCopy(inventory);
            foreach (Order o in orderHistory)
                if (o.OrderCompletion > date)
                    foreach (OrderItem i in o.Items)
                        saveHolder.FindCategory(i.CategoryId)
                                  .FindSubCategory(i.SubCategoryId)
                                  .FindItem(i.ItemId).Stock += i.Amount;
            foreach (StockUpItem i in stockUpHistory)
                if (i.StockUpCompletion > date)
                    saveHolder.FindCategory(i.CategoryId)
                              .FindSubCategory(i.SubCategoryId)
                              .FindItem(i.ItemId).Stock -= i.Amount;
            return saveHolder.Inventory;
        }
        private List<Order> GetOrderHistoryRange(DateTime fromDate, DateTime toDate, List<Order> orderHistory)
        {
            List<Order> output = new List<Order>();
            foreach (Order o in orderHistory)
                if (o.OrderCompletion >= fromDate && o.OrderCompletion <= toDate)
                    output.Add(o);
            return output;
        }
        private List<StockUpItem> GetStockUpHistoryRange(DateTime fromDate, DateTime toDate, List<StockUpItem> stockUpHistory)
        {
            List<StockUpItem> output = new List<StockUpItem>();
            foreach (StockUpItem s in stockUpHistory)
                if (s.StockUpCompletion >= fromDate && s.StockUpCompletion <= toDate)
                    output.Add(s);
            return output;
        }
        private List<AccountingSupplier> SortAccountingItems(List<AccountingItem> items, List<Supplier> suppliers)
        {
            List<AccountingSupplier> output = new List<AccountingSupplier>();
            foreach (AccountingItem i in items)
            {
                AccountingSupplier found = output.Find(x => x.Supplier.Id == i.supplierId);
                if (found == null)
                {
                    found = new AccountingSupplier();
                    found.Supplier = suppliers.Find(x => x.Id == i.supplierId);
                    output.Add(found);
                }
                found.Items.Add(i);
            }
            return output;
        }
    }
}
