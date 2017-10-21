using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;

namespace ScalingCleverDemo.Android
{
    [Activity(Label = "ScalingCleverDemo.Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new Game1();
            var view = (View)g.Services.GetService(typeof(View));
            //获取设备实际分辨率大小，
            Point point = new Point();
            WindowManager.DefaultDisplay.GetSize(point);
            int width = point.X;
            int height = point.Y;
            //全屏放大游戏触控坐标
            var _view=ScalingClever.ResolutionScaling.OnCreate(view, new Point(800, 480), new Point(width, height));
            SetContentView(_view);
            g.Run();
        }
    }
}

