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
            // Android 13
            ("android.permission.BLUETOOTH_SCAN", true),
            ("android.permission.BLUETOOTH_CONNECT", true)
            // Android 10
            //("android.permission.BLUETOOTH", true),
            //("android.permission.BLUETOOTH_ADMIN", true)
            
            //("android.permission.BLUETOOTH_ADMIN", true),
            //("android.permission.BLUETOOTH_SCAN", true),
            //("android.permission.BLUETOOTH_ADVERTISE", true),
            //("android.permission.BLUETOOTH_CONNECT", true)
            }.ToArray();
#endif
    }
}
