using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace IMP_reseni.MyPermissions
{
    public class MyBluetoothPermissionOldVersion : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {       
                // Android 10
                ("android.permission.BLUETOOTH", true),
                //("android.permission.BLUETOOTH_ADMIN", true),
                //("android.permission.BLUETOOTH_SCAN", true),
                //("android.permission.BLUETOOTH_CONNECT", true),
                //("android.permission.BLUETOOTH_ADVERTISE", true),
                ("android.permission.ACCESS_FINE_LOCATION", true)

                //(global::Android.Manifest.Permission.Bluetooth, true),
                //(global::Android.Manifest.Permission.BluetoothScan, true),
                //(global::Android.Manifest.Permission.BluetoothConnect, true),
                //(global::Android.Manifest.Permission.AccessFineLocation, true),

            }.ToArray();
    }
}
