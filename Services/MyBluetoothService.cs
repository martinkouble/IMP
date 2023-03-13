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

namespace IMP_reseni.Services
{
    
    public class MyBluetoothService
    {
        byte[] buffer;
        NetworkStream outStream;
        private SaveHolder saveHolder;
        private BasketHolder basketHolder;

        public string CompanyName { get; private set; }
        public string CompanyAddress { get; private set; }
        public string IC { get; private set; }
        public string DIC { get; private set; }

        public MyBluetoothService(SaveHolder saveHolder,BasketHolder basketHolder)
        {
            this.saveHolder= saveHolder;
            this.basketHolder= basketHolder;
            CompanyName= Preferences.Default.Get("CompanyName", "");
            CompanyAddress= Preferences.Default.Get("CompanyAddress", "");
            IC= Preferences.Default.Get("IC", "");
            DIC= Preferences.Default.Get("DIC", "");
        }
        public void SetComapnyInfo(string CompanyName, string CompanyAddress, string IC, string DIC)
        {
            this.CompanyName = CompanyName;
            this.CompanyAddress = CompanyAddress;
            this.IC = IC;
            this.DIC = DIC;
            Preferences.Default.Set("CompanyName", CompanyName);
            Preferences.Default.Set("CompanyAddress", CompanyAddress);
            Preferences.Default.Set("IC", IC);
            Preferences.Default.Set("DIC", DIC);
        }

        private void SendMessage()
        {
            uint messageLength = (uint)buffer.Length;
            byte[] countBuffer = BitConverter.GetBytes(messageLength);

            outStream.Write(countBuffer, 0, countBuffer.Length);
            outStream.Write(buffer, 0, buffer.Length);
        }
        private string RemoveDiacritics(String s)
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
        private void ReceiptPrint(double TotalCostWithNoDPH, double TotalCost)
        {
            basketHolder.receiptNumber++;
            string stringNumber = basketHolder.receiptNumber.ToString().PadLeft(7, '0');
            Printer.output.Clear();
            Printer.Align("center");
            Printer.PrintLine(CompanyName);
            Printer.PrintLine(CompanyAddress);
            Printer.PrintLine("IC:"+ IC+" DIC: "+DIC);
            Printer.PrintLine(new string('-',32));
            Printer.Align("left");
            Printer.PrintLine("Datum: " + DateTime.Now.ToString("HH:mm:ss"));
            Printer.PrintLine("Čas: " + DateTime.Now.ToString("dd.MM.yyyy"));
            Printer.PrintLine("Cislo uctenky: " + stringNumber);
            Printer.PrintLine(new string('-', 32));
            foreach (OrderItem o in basketHolder.Order.Items)
            {
                Items item = saveHolder.FindCategory(o.CategoryId).FindSubCategory(o.SubCategoryId).FindItem(o.ItemId);
                Printer.PrintLine(RemoveDiacritics(item.Name));
                Printer.tab(16, 22);
                Printer.Print(item.SellCost.ToString() + " Kc/ks");
                Printer.tabSkok();
                Printer.Print("x"+o.Amount.ToString());
                Printer.tabSkok();
                Printer.Print("="+(item.SellCost * o.Amount).ToString() + " Kc");
                Printer.PrintLine(new string('*', 32));
            }
            Printer.Align("center");
            Printer.PrintLine("Mezisoučet bez DPH: " + TotalCostWithNoDPH + "Kc");
            Printer.PrintLine("DPH: " + (TotalCost-TotalCostWithNoDPH) + "Kc");
            Printer.PrintLine("Celkova castka: " + TotalCost + "Kc");
            buffer = Printer.output.ToArray();
            SendMessage();
        }
        public async Task<bool> BluetoothConnection(double TotalCostWithNoDPH, double TotalCost)
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
                    /*
                    //ulong sevenItems = 0x020000000000;
                    //BluetoothAddress address = new BluetoothAddress(sevenItems);
                    //bool paired = BluetoothSecurity.PairRequest(address, "0000");
                    //var guid = picker.GetRfcommServicesAsync().Result.;
                    //var guid =await picker.GetRfcommServicesAsync();
                    //var guid = picker.GetRfcommServicesAsync().Result.FirstOrDefault();*/
                    var guid = InTheHand.Net.Bluetooth.BluetoothService.SerialPort;
                    client.Connect(address, guid);

                    outStream = client.GetStream();
                    Printer.output.Add(0x1B);
                    Printer.output.Add(0x40);
                    buffer = Printer.output.ToArray();
                    ReceiptPrint( TotalCostWithNoDPH,  TotalCost);
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
                await Toast.Make("Zapněte Bluetooth a zkontroluje oprávnění").Show();
                return false;
            }
        }
    }
}
