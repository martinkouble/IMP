using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using IMP_reseni.Models;
using IMP_reseni.Services;


namespace IMP_reseni.ViewModels
{
    public class AccountingViewModel : INotifyPropertyChanged
    {
        public ICommand DialySalesCommand { get; private set; }
        public ICommand InsertDataCommand { get; private set; }
        public ICommand InsertExcelDataCommand { get; private set; }
        public ICommand GenerateAccountingOnlyStockCommand { get; private set; }
        public ICommand GenerateAccountingCommand { get; private set; }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set 
            {
                SetProperty(ref _startDate, value);
                (GenerateAccountingCommand as Command).ChangeCanExecute();
                (GenerateAccountingOnlyStockCommand as Command).ChangeCanExecute();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set 
            {
                SetProperty(ref _endDate, value);
                (GenerateAccountingCommand as Command).ChangeCanExecute();
                (GenerateAccountingOnlyStockCommand as Command).ChangeCanExecute();
            }
        }
        private SaveHolder saveholder;
        public AccountingViewModel(SaveHolder saveholder)
        {
            this.saveholder= saveholder;
            DialySalesCommand = new Command(
            () =>
            {
                DateTime from = DateTime.Today;
                DateTime to = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                GenerateFile(from, to);
            });

            GenerateAccountingCommand = new Command(
            canExecute: () =>
            {
                if (StartDate<EndDate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            },
            execute: () =>
            {
                GenerateFile(StartDate, EndDate);
            });

            GenerateAccountingOnlyStockCommand = new Command(
            canExecute: () =>
            {
               if (StartDate < EndDate)
               {
                   return true;
               }
               else
               {
                   return false;
               }
            },
            execute: () =>
            {
               GenerateFileStock(StartDate, EndDate);
            });

            InsertDataCommand = new Command(
            async() =>
            {
                var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { ".json" } }, 
                    { DevicePlatform.Android, new[] { ".json" } },
                    { DevicePlatform.WinUI, new[] { ".json" } },
                });
                
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    FileTypes = customFileType
                });
                if (result != null)
                {
                    saveholder.Load(result.FullPath);
                    await Toast.Make("Soubor byl úspěšně načten").Show();
                }
                else
                {
                    await Toast.Make("Soubor nebyl vybrán").Show();
                }
            });
            InsertExcelDataCommand = new Command(
            async() =>
            {
                var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { ".csv" } },
                    { DevicePlatform.Android, new[] { ".csv" } },
                    { DevicePlatform.WinUI, new[] { ".csv" } },
                });

                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    FileTypes = customFileType
                });
                if (result != null)
                {
                    StreamReader sr = new StreamReader(result.FullPath);

                    string[] excelReading = sr.ReadToEnd().Replace('\r', '\0').Split('\n');
                    string[] itemData;

                    for (int i = 1; i < excelReading.Length - 1; i++)
                    {
                        itemData = excelReading[i].Split(';');
                        NewFileItem(itemData);
                    }
                    sr.Close();
                }
                else
                {
                    await Toast.Make("Soubor nebyla vybrána").Show();
                }
            });

            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }


        private void GenerateFile(DateTime from, DateTime to)
        {
            if (from != default(DateTime) && to != default(DateTime))
            {
                AccountingHandler accounting = new AccountingHandler();
                string path;
#if ANDROID
                path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads) + "/účetnictví " + from.ToString("dd-mm-yyyy") + "-" + to.ToString("dd-mm-yyyy") + ".csv";
#else
                path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/účetnictví " + from.ToString("dd-mm-yyyy") + "-" + to.ToString("dd-mm-yyyy") + ".csv";
#endif
                string accFileText = accounting.CreateAccountingFileContent(from, to, saveholder.Inventory, saveholder.OrderHistory, saveholder.StockUpHistory, saveholder.Suppliers);
                if (!File.Exists(path)) 
                {
                    File.WriteAllText(path, accFileText, Encoding.UTF8);
                    Toast.Make("Soubor vytvořen").Show();
                }
                else
                {
                    Toast.Make("Soubor vytvořen").Show();
                }
            }
        }
        private void GenerateFileStock(DateTime from, DateTime to)
        {
            if (from != default(DateTime) && to != default(DateTime))
            {
                AccountingHandler accounting = new AccountingHandler();
                string path;
#if ANDROID
                path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads) + "/sklad " + from.ToString("dd-mm-yyyy") + "-" + to.ToString("dd-mm-yyyy") + ".csv";
#else
                path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/sklad " + from.ToString("dd-mm-yyyy") + "-" + to.ToString("dd-mm-yyyy") + ".csv";
#endif
                //string path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/sklad " + from.ToShortDateString() + "-" + to.ToShortDateString() + ".csv";
                string accFileText = accounting.CreateAccountingStockFileContent(from, to, saveholder.Inventory, saveholder.OrderHistory, saveholder.StockUpHistory, saveholder.Suppliers);

                if (!File.Exists(path))
                {
                    File.WriteAllText(path, accFileText, Encoding.UTF8);
                    Toast.Make("Soubor vytvořen").Show();
                }
                else
                {
                    Toast.Make("Soubor nevytvořen").Show();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;

            OnPropertyChanged(propertyName);
            return true;
        }
        int counter;
        private void NewFileItem(string[] data)
        {
            string item = data[0];
            string cat = data[1];
            string subcat = data[2];
            string sellCost = data[3];
            string buyCost = data[4];
            string supp = data[5];
            string sor = data[6];
            string disabled = data[7];

            //itemImagePath = "/storage/emulated/0/FotkyZBB/Polozky/" + number2 + ".jpg";
            //itemImageUri = Android.Net.Uri.Parse("content://com.mi.android.globalFileexplorer" +
            //                     ".myprovider/external_files/FotkyZBB/Polozky/" + number2 + ".jpg");

            if (cat != "" && subcat != "" && item != "" && sellCost != "")
            {
                //NewNameDialog catholder = new NewNameDialog();
                //catholder.catholder = cat;
                //catholder.subcatholder = subcat;

                Category category = saveholder.FindCategoryByName(cat);

                if (category == null)
                {
                    category = new Category();
                    category.ImageUrl = null;
                    category.Name = cat;
                    if(!saveholder.ExistCategoryByName(category.Name))
                    {
                        saveholder.AddCategory(category);
                    }
                    saveholder.Save();
                }
                int categoryId = saveholder.FindCategoryByName(cat).Id;

                SubCategory subCategory = category.FindSubCategoryByName(subcat);
                if (subCategory == null)
                {
                    subCategory = new SubCategory();
                    subCategory.Name = subcat;
                    subCategory.ImageUrl = null;
                    if (!saveholder.ExistCategoryByName(subCategory.Name))
                    {
                        saveholder.AddSubCategory(categoryId, subCategory);
                    }
                    saveholder.Save();
                }
                //number++;
                int subCategoryId = category.FindSubCategoryByName(subcat).Id;

                Supplier supplier = saveholder.FindSupplierByName(supp);
                if (supplier == null)
                {
                    if (supp == "")
                    {
                        supp = "Vlastní výroba";
                        supplier = new Supplier();
                        supplier.Name = "Vlastní výroba";
                        saveholder.AddSupplier(supplier);
                        saveholder.Save();
                        supplier =saveholder.FindSupplierByName("Vlastní výroba");
                    }
                    else
                    {
                        supplier = new Supplier();
                        supplier.Name = supp;
                        saveholder.AddSupplier(supplier);
                        saveholder.Save();
                    }
                }
                int supplierId = saveholder.FindSupplierByName(supp).Id;

                if (!saveholder.FindCategoryByName(category.Name).FindSubCategoryByName(subCategory.Name).ExistItemByName(item))
                {
                    Items newItem = new Items();
                    if (buyCost == "0")
                    {
                        newItem.Create(item, StringToBool(disabled), 0, StringToDouble(sellCost), StringToBool(sor), supplierId, categoryId, subCategoryId);
                    }
                    else
                    {
                        newItem.Create(item, StringToBool(disabled), StringToDouble(buyCost), StringToDouble(sellCost), StringToBool(sor), supplierId, categoryId, subCategoryId);
                    }
                    saveholder.AddItem(categoryId, subCategoryId, newItem);
                    saveholder.Save();
                }
                else
                {
                    counter++;
                }

            }
            else
            {
                Toast.Make("Zkontrolujte si správnost šablony, nebyla vyplněna pole kategorie, podkategorie, položky nebo její ceny.").Show();
            }
            Toast.Make("Počet stejných jmen u položek: " + counter + ", položky s duplicitním jménem se nevytvoří.").Show();
        }
        private double StringToDouble(string _string)
        {
            return double.Parse(_string, CultureInfo.InvariantCulture);
        }
        private bool StringToBool(string _string)
        {
            if (_string == "/0")
            {
                return true;
            }
            else if (_string == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
