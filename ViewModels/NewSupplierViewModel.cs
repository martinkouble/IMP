using Microsoft.Maui.Controls;
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
using IMP_reseni.Services;

namespace IMP_reseni.ViewModels
{
    public class NewSupplierViewModel: INotifyPropertyChanged
    {

        public ICommand CreateCommand { get; set; }

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
        public NewSupplierViewModel(SaveHolder saveholder)
        {
            Text = "";
            CreateCommand = new Command<string>(
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
            execute:(string name) =>
            {
                Supplier newSupplier = new Supplier();
                newSupplier.Name = name;
                if (!saveholder.ExistSupplierByName(name))
                {
                    saveholder.AddSupplier(newSupplier);
                    saveholder.Save();
                    Toast.Make("Nový dodavatel vytvořen").Show();
                    Text = "";
                }
                else
                {
                    Toast.Make("Dodavatel s tímto jménem již existuje").Show();
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
