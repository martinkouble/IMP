using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public AccountingViewModel()
        {
           
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
                    App.saveholder.Load(result.FullPath);
                }
                else
                {

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
                string accFileText = accounting.CreateAccountingFileContent(from, to, App.saveholder.Inventory, App.saveholder.OrderHistory, App.saveholder.StockUpHistory, App.saveholder.Suppliers);
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
                string accFileText = accounting.CreateAccountingStockFileContent(from, to, App.saveholder.Inventory, App.saveholder.OrderHistory, App.saveholder.StockUpHistory, App.saveholder.Suppliers);

                if (!File.Exists(path))
                {
                    File.WriteAllText(path, accFileText, Encoding.UTF8);
                    Toast.Make("Soubor vytvořen :)").Show();
                }
                else
                {
                    Toast.Make("Soubor vytvořen :(").Show();
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
    }
   
}
