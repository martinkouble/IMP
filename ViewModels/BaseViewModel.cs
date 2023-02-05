using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Platform;

namespace IMP_reseni.ViewModels
{
    public class BaseViewModel
    {
        public ICommand UnFocusCommand { get;private set; }
        public BaseViewModel() 
        {
         UnFocusCommand = new Command<Entry>(
         (Entry entry) =>
         {
#if ANDROID
             if (Platform.CurrentActivity.CurrentFocus != null)
                 Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#else
                entry.Unfocus();
#endif
         });
    }
    }
}
