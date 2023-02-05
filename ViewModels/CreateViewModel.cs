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
    public class CreateViewModel: INotifyPropertyChanged
    {
        public ICommand NavigateCommand { get; private set; }
        public CreateViewModel()
        {

        }
        public CreateViewModel(Page _page) 
        {
           NavigateCommand = new Command<Type>(
           async (Type _targetPageType) =>
           {
               Page _targetPage = (Page)Activator.CreateInstance(_targetPageType);
               await _page.Navigation.PushAsync(_targetPage);
           }
           );
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
