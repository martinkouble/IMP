using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace IMP_reseni.MyPermissions
{
    public class MyReadWritePermission : Permissions.BasePlatformPermission
    {
#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
        //(global::Android.Manifest.Permission.ReadExternalStorage, true),
        //(global::Android.Manifest.Permission.WriteExternalStorage, true)
           ("android.permission.READ_EXTERNAL_STORAGE", true),
           ("android.permission.WRITE_EXTERNAL_STORAGE", true)
            }.ToArray();
#endif
    }
}
