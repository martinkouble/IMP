using IMP_reseni.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMP_reseni.ViewModels
{
    
    public class SetPasswordViewModel : INotifyPropertyChanged
    {
        public ICommand SetPasswordCommand { get; private set; }
        string name;
        public string Name
        {
            set { SetProperty(ref name, value); }
            get { return name; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SetPasswordViewModel(ContentPage _page)
        {
            Name = "";
            SetPasswordCommand = new Command(
            async () =>
            {
                await SecureStorage.SetAsync("token", name);
                var _root = _page.Navigation.NavigationStack[0];
                _page.Navigation.InsertPageBefore(new MainPage(), _root);
                await _page.Navigation.PopToRootAsync();
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
