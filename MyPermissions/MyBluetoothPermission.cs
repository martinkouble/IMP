using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace IMP_reseni.MyPermissions
{
    public class MyBluetoothPermission : Permissions.BasePlatformPermission
    {
#if ANDROID

        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
                //    // Android 13
                ("android.permission.BLUETOOTH_SCAN", true),
                ("android.permission.BLUETOOTH_CONNECT", true)            
                //(global::Android.Manifest.Permission.BluetoothScan, true),
                //(global::Android.Manifest.Permission.BluetoothConnect, true),           
                //    // Android 10
                //    //("android.permission.BLUETOOTH", true),
                //    //("android.permission.BLUETOOTH_ADMIN", true),
                //    //("android.permission.BLUETOOTH_SCAN", true),
                //    //("android.permission.BLUETOOTH_CONNECT", true)
                //    //("android.permission.BLUETOOTH_ADVERTISE", true),

                //(global::Android.Manifest.Permission.Bluetooth, true),
                //(global::Android.Manifest.Permission.BluetoothScan, true),
                //(global::Android.Manifest.Permission.BluetoothConnect, true),
                //(global::Android.Manifest.Permission.AccessFineLocation, true),

            }.ToArray();
#endif
    }
}
