
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StartedServicesDemo
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
    public class BootComplete : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(Intent.ActionBootCompleted))
            {
                Toast.MakeText(context, "Action Boot Completed!", ToastLength.Long).Show();
                Intent serviceToStart = new Intent(context, typeof(TimestampService));
                context.StartService(serviceToStart);
            }
        }
    }
}
