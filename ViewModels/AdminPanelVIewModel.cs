using CommunityToolkit.Maui.Alerts;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using IMP_reseni.Controls;

namespace IMP_reseni.ViewModels
{
    public class AdminPanelViewModel: INotifyPropertyChanged
    {
        public ICommand NavigateCommand { get; private set; }
        public ICommand ChangePasswordCommand { get; private set; }

        public AdminPanelViewModel(Page _page)
        {
            NavigateCommand = new Command<Type>(
            async (Type _targetPageType) =>
            {
                //Page _targetPage = (Page)Activator.CreateInstance(_targetPageType);
                //await _page.Navigation.PushAsync(_targetPage);
                await MopupService.Instance.PushAsync(new SpinnerPopup());
                var method = typeof(NavigationExtensions).GetMethods().Where(x => x.Name == "PushAsync").ElementAt(0);
                MethodInfo generic = method.MakeGenericMethod(_targetPageType);
                generic.Invoke(_page, new object[1] { _page.Navigation });
                await MopupService.Instance.PopAsync();
            }
            );
            ChangePasswordCommand = new Command(
            async () =>
            {
                string result = await _page.DisplayPromptAsync("Změna hesla", "Nové heslo");
                if (result != null) 
                {
                    await SecureStorage.SetAsync("token", result);
                    await Toast.Make("Heslo změněno").Show();
                }
                else
                {
                    await Toast.Make("Heslo nezměněno").Show();
                }
            });
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
