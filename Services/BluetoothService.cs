using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;

namespace IMP_reseni.Services
{
    
    public class BluetoothService
    {
        BluetoothService()
        {

        }
        private void ConnectionCheck()
        {
            var picker= new BluetoothDevicePicker().PickSingleDeviceAsync();
        }
    }
}
