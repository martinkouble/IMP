using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IMP_reseni.Models;
using CommunityToolkit.Maui.Alerts;
using System.Collections.ObjectModel;
using System.Collections;
namespace IMP_reseni.ViewModels
{
    class ModifySupplierViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<string> ListOfSupplier { get; set; }

        public ICommand ModifyCommand { get; set; }

        private string _selectedSupplier;

        public string SelectedSupplier
        {
            set
            {
                SetProperty(ref _selectedSupplier, value);
                if (value != "")
                {
                    Text = value;
                }
            }

            get { return _selectedSupplier; }
        }

        private string _text;
        public string Text
        {
            set { SetProperty(ref _text, value); }
            get { return _text; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ModifySupplierViewModel()
        {
            //Text = "";
            List<string> list = new List<string>(App.saveholder.GetSupplierNames());
            list.Sort();
            ListOfSupplier = new ObservableCollection<string>(list);

            ModifyCommand = new Command<string>(
            canExecute: (string name) =>
            {
                if (name == "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            },

            execute: (string name) =>
            {
                Supplier supplier = new Supplier();
                supplier = App.saveholder.FindSupplierByName(SelectedSupplier);
                supplier.Name = name;
                App.saveholder.ModifySupplier(supplier);
                App.saveholder.Save();
                Toast.Make("Dodavatel změněn").Show();
                Text = "";
                SelectedSupplier = null;
                List<string> list = new List<string>(App.saveholder.GetSupplierNames());
                list.Sort();
                ListOfSupplier.Clear();
                foreach (var Item in list)
                {
                    ListOfSupplier.Add(Item);
                }
            });
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
