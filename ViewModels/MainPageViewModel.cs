using IMP_reseni.Models;
using IMP_reseni.Views;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMP_reseni.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //        await Navigation.PushAsync(new Views.Login());
        public IList<Items> Items { get; private set; }

        public ICommand NavigateCommand { get; private set; }

        //
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPageViewModel()
        {

        }
        public MainPageViewModel(ContentPage _page)
        {
            NavigateCommand = new Command(
            async () =>
            {
                //Page page = (Page)Activator.CreateInstance(pageType);
                await _page.Navigation.PushAsync(new Login());

            });

            Items= new List<Items>();
            for (int i = 0; i <=20; i++)
            {
                Items.Add(new Items
                {
                    Name = "NIC",
                    ImageUrl = "https://img.darkoviny.cz/images/6657.jpg",
                });
            }
          
 
        }
        



    }
}
