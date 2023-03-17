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
        public ICommand SellCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Page page;


        private string _totalCostText="";
        public string TotalCostText
        {
            get
            {
                return _totalCostText;
            }
            set
            {
                SetProperty(ref _totalCostText, value);
            }
        }
        private string _totalCostWithNoDPHText = "";
        public string TotalCostWithNoDPHText
        {
            get
            {
                return _totalCostWithNoDPHText;
            }
            set
            {
                SetProperty(ref _totalCostWithNoDPHText, value);
            }
        }

        public double TotalCost
        {
            get
            { 
                double Cost=0;
                foreach (OrderItem o in basketHolder.Order.Items)
                {
                    //Items item = o.Item;
                    //Cost += item.SellCost * o.Amount;
                    Cost += o.SellCostPerPiece * o.Amount;
                }
                Cost = Math.Round(Cost, 0);
                return Cost; 
            } 
        }
        public double TotalCostWithNoDPH
        {
            get
            {
                return Math.Round(Convert.ToDouble(TotalCost) / 1.21, 2);
            }
        }

        public ObservableCollection<OrderItem> BasketItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //int receiptNumber;
        //byte[] buffer;
        //NetworkStream outStream;
        private BasketHolder basketHolder;
        private SaveHolder saveHolder;
        public BasketViewModel(BasketHolder basketHolder,SaveHolder saveHolder, MyBluetoothService bluetoothService) 
        {
            this.basketHolder = basketHolder;
            this.saveHolder = saveHolder;
            BasketItems =new ObservableCollection<OrderItem>();
            foreach (var item in basketHolder.Items)
            {
                BasketItems.Add(item);
            }
            TotalCostWithNoDPHText = "Cena bez DPH: "+TotalCostWithNoDPH+" Kč";
            TotalCostText = $"Cena s DPH: "+TotalCost.ToString()+" Kč";
            SellCommand = new Command(
           async() =>
            {
               if (basketHolder.Items.Count != 0)
               {
                   bool decision = await page.DisplayAlert("Účtenka", "Přejete si vytisknout účtenku?", "Ano", "Ne");
                    if (decision==true) 
                    {
                        bool success=await bluetoothService.BluetoothConnection(TotalCostWithNoDPH,TotalCost);
                        if (success==true)
                        {
                            basketHolder.CompleteOrder();
                            TotalCostWithNoDPHText = "Cena bez DPH: 0 Kč";
                            TotalCostText = $"Cena s DPH: 0 Kč";
                            BasketItems.Clear();
                            await Toast.Make("Prodáno s účtenkou").Show();
                        }
                        else
                        {
                            await Toast.Make("Došlo k chybě! Zkontrolujte Bluetooth a tiskárnu").Show();
                        }
                    }
                    else
                    {
                        basketHolder.CompleteOrder();
                        TotalCostWithNoDPHText = "Cena bez DPH: 0 Kč";
                        TotalCostText = $"Cena s DPH: 0 Kč";
                        BasketItems.Clear();
                        await Toast.Make("Prodáno").Show();
                    }
               }
               else
               {
                   await Toast.Make("V košíku není žádná položka").Show();
               }

            });

            DeleteCommand = new Command<OrderItem>(
                (OrderItem a) => 
           {
               BasketItems.Clear();
               basketHolder.RemoveItemFromBasket(a.CategoryId, a.SubCategoryId, a.ItemId);
               foreach (var item in basketHolder.Items)
               {
                   BasketItems.Add(item);
               }
               TotalCostWithNoDPHText = "Cena bez DPH: " + TotalCostWithNoDPH + " Kč";
               TotalCostText = $"Cena s DPH: " + TotalCost.ToString() + " Kč";
           });
        }
        /*
        private async Task<bool> BluetoothConnection()
        {
            PermissionStatus status = PermissionStatus.Granted;
            //PermissionStatus status;
            if (DeviceInfo.Version.Major >= 12)
            {
                status = await Permissions.RequestAsync<MyBluetoothPermission>();
            }
            else
            {
                status = await Permissions.RequestAsync<MyBluetoothPermissionOldVersion>();
            }
            //PermissionStatus status= await CheckAndRequestContactsReadPermission();
            // PermissionStatus status =await Permissions.CheckStatusAsync<MyBluetoothPermission>();

            //IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;

            if (status == PermissionStatus.Granted)
            {
                try
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
                    return true;
                }
                catch (Exception)
                {
                    await Toast.Make("Došlo k chybě! Zkontrolujte Bluetooth a tiskárnu").Show();
                    return false;

                }
            }
            else
            {
                await Toast.Make("Zapněte Bluetooth").Show();
                return false;
            }
        }




        private void ReceiptPrint()
        {
            basketHolder.receiptNumber++;
            string stringNumber = basketHolder.receiptNumber.ToString().PadLeft(7, '0');
            Printer.output.Clear();
            Printer.Align("center");
            Printer.PrintLine("Zivot bez barier Nova Paka");
            Printer.PrintLine("Lomena 533, 509 01 Nova Paka");
            Printer.PrintLine("IC: 26652561    DIC: CZ26652561");
            Printer.PrintLine("--------------------------------");
            Printer.Align("left");
            Printer.PrintLine("Datum: " + DateTime.Now.ToString("HH:mm:ss"));
            Printer.PrintLine("Čas: " + DateTime.Now.ToString("dd.MM.yyyy"));
            Printer.PrintLine("Cislo uctenky: " + stringNumber);
            Printer.PrintLine("--------------------------------");
            foreach (OrderItem o in basketHolder.Order.Items)
            {
                Items item = saveHolder.FindCategory(o.CategoryId).FindSubCategory(o.SubCategoryId).FindItem(o.ItemId);

                Printer.tab(16, 22);
                Printer.Print(RemoveDiacritics(item.Name));
                Printer.tabSkok();
                Printer.PrintLine(item.SellCost.ToString() + " Kc/ks");
                Printer.tabSkok();
                Printer.Print(o.Amount.ToString() + "x");
                Printer.tabSkok();
                Printer.PrintLine((item.SellCost * o.Amount).ToString() + " Kc");
            }
            Printer.PrintLine("-------------------------------");
            Printer.Align("right");
            Printer.PrintLine("Mezisoucet bez DPH: " + TotalCostWithNoDPH + "Kc");
            Printer.PrintLine("DPH: " + (TotalCost -TotalCostWithNoDPH) + "Kc");
            Printer.PrintLine("-------------------------------");
            Printer.PrintLine("Celkova castka: " + TotalCost + "Kc");
            Printer.PrintLine();
            Printer.PrintLine();
            buffer = Printer.output.ToArray();
            SendMessage();
        }
        // new string('*', 4)
        private void SendMessage()
        {
            uint messageLength = (uint)buffer.Length;
            byte[] countBuffer = BitConverter.GetBytes(messageLength);

            outStream.Write(countBuffer, 0, countBuffer.Length);
            outStream.Write(buffer, 0, buffer.Length);
        }

        
        //public async Task<PermissionStatus> CheckAndRequestContactsReadPermission(Type Permission)
        //{
        //    PermissionStatus status = await Permissions.CheckStatusAsync<MyBluetoothPermission>();

        //    if (status == PermissionStatus.Granted)
        //        return status;

        //    if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        //    {
        //        // Prompt the user to turn on in settings
        //        // On iOS once a permission has been denied it may not be requested again from the application
        //        return status;
        //    }

        //    if (Permissions.ShouldShowRationale<MyBluetoothPermission>())
        //    {
        //        // Prompt the user with additional information as to why the permission is needed
        //    }

        //    status = await Permissions.RequestAsync<MyBluetoothPermission>();

        //    return status;
        //}

        public static string RemoveDiacritics(String s)
        {
            s = s.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(s[i]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[i]);
                }
            }
            return sb.ToString();
        }
        */
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
