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
using IMP_reseni.Services;
using System.Net.Sockets;
using InTheHand.Net;
using System.Collections.ObjectModel;
using IMP_reseni.Models;
using CommunityToolkit.Maui.Alerts;

namespace IMP_reseni.ViewModels
{
    public class BasketViewModel: INotifyPropertyChanged
    {
        public ICommand Button_Clicked { get; private set; }
        public ICommand DeleteCommand { get; private set; }


        public ObservableCollection<OrderItem> BasketItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        byte[] buffer;
        NetworkStream outStream;
        public BasketViewModel(BasketHolder basketHolder) 
        {
            BasketItems=new ObservableCollection<OrderItem>();
            foreach (var item in App.basketHolder.Items)
            {
                BasketItems.Add(item);
            }
            Button_Clicked = new Command(
           async () =>
           {
               if (basketHolder.Items.Count!=0)
               {
                   //PermissionStatus status = PermissionStatus.Granted;
                   PermissionStatus status=await Permissions.RequestAsync<MyBluetoothPermission>();
                   //PermissionStatus status= await CheckAndRequestContactsReadPermission();
                   // PermissionStatus status =await Permissions.CheckStatusAsync<MyBluetoothPermission>();
                   if (status == PermissionStatus.Granted)
                   {
                       var picker = await new BluetoothDevicePicker().PickSingleDeviceAsync();
                       BluetoothClient client = new BluetoothClient();
                       var address = picker.DeviceAddress;
                       //ulong sevenItems = 0x020000000000;
                       //BluetoothAddress address = new BluetoothAddress(sevenItems);
                       //bool paired = BluetoothSecurity.PairRequest(address, "0000");

                       //var guid = picker.GetRfcommServicesAsync().Result.;
                       //var guid =await picker.GetRfcommServicesAsync();
                       //var guid = picker.GetRfcommServicesAsync().Result.FirstOrDefault();
                       var guid = InTheHand.Net.Bluetooth.BluetoothService.SerialPort;
                       client.Connect(address, guid);

                       outStream = client.GetStream();
                       Printer.output.Add(0x1B);
                       Printer.output.Add(0x40);
                       buffer = Printer.output.ToArray();
                       ReceiptPrint();
                       client.Close();
                   }
               }
          
           });

            DeleteCommand = new Command<OrderItem>(
                (OrderItem a) => 
           {
               BasketItems.Remove(a);
               App.basketHolder.RemoveItemFromBasket(a.CategoryId, a.SubCategoryId, a.ItemId);
           });
        }
        private void ReceiptPrint()
        {
            Printer.output.Clear();
            Printer.Align("center");
            Printer.PrintLine("Zivot bez barier Nova Paka");
            Printer.PrintLine("Lomena 533, 509 01 Nova Paka");
            Printer.PrintLine("IC: 26652561    DIC: CZ26652561");
            Printer.PrintLine("--------------------------------");
            Printer.Align("left");
            Printer.PrintLine("Datum: " + DateTime.Now);
            Printer.PrintLine("--------------------------------");
            
            Printer.PrintLine("-------------------------------");
            Printer.Align("right");
            Printer.PrintLine("-------------------------------");
            Printer.PrintLine();
            Printer.PrintLine();

            buffer = Printer.output.ToArray();
            SendMessage();
        }
        private  void SendMessage()
        {
            uint messageLength = (uint)buffer.Length;
            byte[] countBuffer = BitConverter.GetBytes(messageLength);

            outStream.Write(countBuffer, 0, countBuffer.Length);
            outStream.Write(buffer, 0, buffer.Length);
        }


        public async Task<PermissionStatus> CheckAndRequestContactsReadPermission(Type Permission)
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
