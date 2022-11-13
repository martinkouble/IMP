﻿using IMP_reseni.Views;
using Microsoft.Maui.Controls.Xaml;
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
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //        await Navigation.PushAsync(new Views.Login());
        
        public ICommand NavigateCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPageViewModel()
        {
            NavigateCommand = new Command<ContentPage>(
            async (ContentPage Page) =>
            {
                //Page page = (Page)Activator.CreateInstance(pageType);
                await Page.Navigation.PushAsync(new Login());
            });
        }
      

    }
}
