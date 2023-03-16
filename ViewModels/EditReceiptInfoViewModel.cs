using CommunityToolkit.Maui.Alerts;
using IMP_reseni.Services;
using InTheHand.Net.Bluetooth;
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
    public class EditReceiptInfoViewModel:INotifyPropertyChanged
    {
        private string _companyName;
        public string CompanyName 
        { 
            get { return _companyName; }
            set 
            {
                SetProperty(ref _companyName, value);
            } 
        }
        private string _companyAddress;
        public string CompanyAddress
        {
            get { return _companyAddress; }
            set
            {
                SetProperty(ref _companyAddress, value);
            }
        }
        private string _iC;
        public string IC
        {
            get { return _iC; }
            set
            {
                SetProperty(ref _iC, value);
            }
        }
        private string _dIC;
        public string DIC
        {
            get { return _dIC; }
            set
            {
                SetProperty(ref _dIC, value);
            }
        }

        private string _bLDevice;
        public string BLDevice
        {
            get { return _bLDevice; }
            set
            {
                SetProperty(ref _bLDevice, value);
            }
        }
        public ICommand ChangeCommand { get; private set; }
        public ICommand SetPrinterCommand { get; private set; }

        public EditReceiptInfoViewModel(MyBluetoothService myBluetoothService)
        {
            CompanyName=myBluetoothService.CompanyName;
            CompanyAddress=myBluetoothService.CompanyAddress;
            IC=myBluetoothService.IC;
            DIC=myBluetoothService.DIC;
            BLDevice = Preferences.Get("BL_DeviceName","");

            ChangeCommand = new Command<bool>(
            canExecute:(bool CanChange) =>
            {
                return CanChange;
            },
            execute:(bool CanChange) =>
            {
                myBluetoothService.SetComapnyInfo(CompanyName, CompanyAddress,IC,DIC);
                Toast.Make("Údaje nastaveny").Show();
            });

            SetPrinterCommand = new Command(
           async() =>
           {
               try
               {
                   var picker = await new BluetoothDevicePicker().PickSingleDeviceAsync();
                   Preferences.Set("BL_Address", picker.DeviceAddress.ToString());
                   Preferences.Set("BL_DeviceName", picker.DeviceName);
                   BLDevice = picker.DeviceName;
                   await Toast.Make("Úspěšně uloženo").Show();
               }
               catch (Exception)
               {
                   BLDevice = "";
                   await Toast.Make("Došlo k chybě").Show();
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
