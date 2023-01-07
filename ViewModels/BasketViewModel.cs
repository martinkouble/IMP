using IMP_reseni.MyPermissions;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace IMP_reseni.ViewModels
{
    public class BasketViewModel: INotifyPropertyChanged
    {
        public ICommand Button_Clicked { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public BasketViewModel() 
        {
            Button_Clicked = new Command(
           async () =>
           {
               PermissionStatus status=PermissionStatus.Granted;
               //PermissionStatus status=await Permissions.RequestAsync<MyBluetoothPermission>();
               //PermissionStatus status= await CheckAndRequestContactsReadPermission();
               // PermissionStatus status =await Permissions.CheckStatusAsync<MyBluetoothPermission>();
               if (status == PermissionStatus.Granted)
               {
                   var picker = await new BluetoothDevicePicker().PickSingleDeviceAsync();
                   BluetoothClient client = new BluetoothClient();
                   var address = picker.DeviceAddress;
                   bool paired = BluetoothSecurity.PairRequest(address, "0000");

                   //var guid = picker.GetRfcommServicesAsync().Result.;
                   //var guid =await picker.GetRfcommServicesAsync();
                   var guid = picker.GetRfcommServicesAsync().Result.FirstOrDefault();
                   client.Connect(address, guid);
                   
                   var outStream = client.GetStream();
               }
           });

        }
        public async Task<PermissionStatus> CheckAndRequestContactsReadPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<MyBluetoothPermission>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<MyBluetoothPermission>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<MyBluetoothPermission>();

            return status;
        }
    }
}
