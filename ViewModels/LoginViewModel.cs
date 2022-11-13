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
    public class LoginViewModel:INotifyPropertyChanged
    {
        public ICommand VerifyPasswordCommand { get; private set; }
        string passwd;
        public string Passwd
        {
            set { SetProperty(ref passwd, value); }
            get { return passwd; }
        }
        public LoginViewModel(Page _page)
        {
            VerifyPasswordCommand = new Command(
            async () =>
            {
                string token = await SecureStorage.Default.GetAsync("token");
                if(token == passwd)
                {
                    await _page.Navigation.PushAsync(new Views.AdminPanel());
                }
            });
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
