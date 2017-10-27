#if __IOS__
using CoreGraphics;
using UIKit;
#endif

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace ScalingClever
{
    public class ResolutionScaling
    {
        private static RenderTarget2D renderTarget2D;

        private static Matrix _scalingMatrix;
        public static Matrix ScalingMatrix
        {
            set
            {
                _scalingMatrix = value;
            }
            get
            {
                if (_scalingMatrix == null)
                {
                    _scalingMatrix = Matrix.CreateScale(1f, 1f, 1f);
                }
                return _scalingMatrix;
            }
        }
        private static float scalingPositionX = 1f;
        private static float scalingPositionY = 1f;

        private static Point _sourceResolution;
        private static Point _destinationResolution;

        private static float scaleX;
        private static float scaleY;
        /// <summary>
        /// renderTarget2D放大视图（步骤1）：初始化放大视图（如果是UWP将该方法放在BeginDraw方法中） | Matrix放大视图（步骤1）：修改spriteBatch.Begin()中的Matrix来放大视图 
        /// </summary>
        /// <param name="sourceResolution">源分辨率</param>
        /// <param name="destinationResolution">目标分辨率</param>
        public static void Initialize(Point sourceResolution, Point destinationResolution)
        {
            _sourceResolution = sourceResolution;
            _destinationResolution = destinationResolution;

            float sourceResolutionX = (float)_sourceResolution.X;
            float sourceResolutionY = (float)_sourceResolution.Y;

            float destinationResolutionX = destinationResolution.X;
            float destinationResolutionY = destinationResolution.Y;

            scaleX = destinationResolutionX / sourceResolutionX;
            scaleY = destinationResolutionY / sourceResolutionY;

            scalingPositionX = sourceResolutionX / destinationResolutionX;
            scalingPositionY = sourceResolutionY / destinationResolutionY;

            _scalingMatrix = Matrix.CreateScale(scaleX, scaleY, 1f);
        }
        /// <summary>
        /// renderTarget2D放大视图（步骤1）：初始化放大视图（如果是UWP将该方法放在BeginDraw方法中） | Matrix放大视图（步骤1）：修改spriteBatch.Begin()中的Matrix来放大视图 
        /// </summary>
        /// <param name="game">游戏类</param>
        /// <param name="sourceResolution">源分辨率</param>
        public static void Initialize(Game game,Point sourceResolution)
        {
            Point destinationResolution = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            Initialize(sourceResolution,destinationResolution);
        }

        /// <summary>
        ///  renderTarget2D放大视图（步骤2）：实例化renderTarget2D
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sourceResolution"></param>
        public static void LoadContent(Game game, Point sourceResolution)
        {
            renderTarget2D = new RenderTarget2D(game.GraphicsDevice, sourceResolution.X, sourceResolution.Y);
        }
        /// <summary>
        /// renderTarget2D放大视图（步骤3）：设置设备渲染目标为RenderTarget2D
        /// </summary>
        /// <param name="game">Game类实例</param>
        public static void BeginDraw(Game game)
        {
            game.GraphicsDevice.SetRenderTarget(ResolutionScaling.renderTarget2D);
        }
        /// <summary>
        /// renderTarget2D放大视图（步骤4）：将renderTarget2D渲染到全屏幕
        /// </summary>
        /// <param name="game">Game类实例</param>
        /// <param name="spriteBatch">画刷实例</param>
        public static void EndDraw(Game game,SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(ResolutionScaling.renderTarget2D, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Bounds.Width, game.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
        }
        /// <summary>
        /// renderTarget2D放大视图（步骤4）:将renderTarget2D渲染到（指定放大尺寸）
        /// </summary>
        /// <param name="game">Game类实例</param>
        /// <param name="spriteBatch">画刷实例</param>
        /// <param name="destinationResolution">放大视图尺寸</param>
        public static void EndDraw(Game game, SpriteBatch spriteBatch, Point destinationResolution)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(ResolutionScaling.renderTarget2D, new Rectangle(0, 0, destinationResolution.X, destinationResolution.Y), Color.White);
            spriteBatch.End();
        }



        /// <summary>
        /// Matrix放大视图（步骤2）:设置PreferredBackBuffer宽度和高度且窗口大小会改变（UWP），触控坐标与绘图坐标不一致时使用
        /// 
        /// </summary>
        /// <param name="sourceResolution">源分辨率</param>
        /// <param name="destinationResolution">目标分辨率，通常设置设备实际分辨率</param>
        public static void Draw(Point sourceResolution,Point destinationResolution)
        {
            Initialize(sourceResolution, destinationResolution);
        }

        /// <summary>
        /// Matrix放大视图（步骤2）:设置PreferredBackBuffer宽度和高度且窗口大小会改变（UWP），触控坐标与绘图坐标不一致时使用，全屏放大
        /// </summary>
        /// <param name="game">游戏类</param>
        /// <param name="sourceResolution">源分辨率</param>
        public static void Draw(Game game, Point sourceResolution)
        {
            Point destinationResolution = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            Initialize(sourceResolution, destinationResolution);
        }
        public static Point Position(Point point)
        {
            return new Point((int)(point.X * scalingPositionX), (int)(point.Y * scalingPositionY));
        }


        /// <summary>
        /// 放大触控（点击）坐标点
        /// 使用顺序：renderTarget2D放大视图（步骤5）
        /// </summary>
        /// <param name="vector2">坐标点</param>
        /// <returns></returns>
        public static Vector2 Position(Vector2 vector2)
        {
            return new Vector2(vector2.X * scalingPositionX, vector2.Y * scalingPositionY);
        }
        /// <summary>
        /// 放大触控（点击）坐标点X
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float X(float x)
        {
            return x * scalingPositionX;
        }
        /// <summary>
        /// 放大触控（点击）坐标点Y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float Y(float y)
        {
            return y * scalingPositionY;
        }

#if __IOS__
        /// <summary>
        /// 不设置PreferredBackBuffer宽度和高度使用此方法
        /// </summary>
        /// <param name="sourceResolution">源分辨率</param>
        public static void FinishedLaunching(CGSize sourceResolution)
        {
            //原游戏界面尺寸大小
            float viewWidth = (float)sourceResolution.Width;
            float viewHeight = (float)sourceResolution.Height;
            CGRect rect_screen = UIApplication.SharedApplication.Windows[0].Screen.Bounds;
            CGSize size_screen = rect_screen.Size;
            float scale_screen = (float)UIApplication.SharedApplication.Windows[0].Screen.Scale;
            //获取iOS设备实际分辨率尺寸
            float screenWidth = (float)((float)size_screen.Width * scale_screen);
            float screenHeight = (float)((float)size_screen.Height * scale_screen);
            //计算需要放大的倍数X方向，y方向
            float scaleXValue = screenWidth / viewWidth;
            float scaleYValue = screenHeight / viewHeight;
            UIApplication.SharedApplication.Windows[0].RootViewController.View.Transform = CGAffineTransform.MakeScale(scaleXValue, scaleYValue);
            //如果界面存在偏移，可用一下方法移动视图位置 
            UIApplication.SharedApplication.Windows[0].RootViewController.View.Frame = new CGRect(0 ,0, (int)screenWidth , (int)screenHeight);
        }
        /// <summary>
        /// 不设置PreferredBackBuffer宽度和高度使用此方法
        /// </summary>
        /// <param name="sourceResolution">源分辨率</param>
        /// <param name="destinationResolution">目标分辨率，通常设置设备实际分辨率</param>
        public static void FinishedLaunching(CGSize sourceResolution, CGSize destinationResolution)
        {
            //原游戏界面尺寸大小
            float viewWidth = (float)sourceResolution.Width; float viewHeight = (float)sourceResolution.Height;
            //设备实际分辨率尺寸
            float screenWidth = (float)destinationResolution.Width;
            float screenHeight = (float)destinationResolution.Height;
            //计算需要放大的倍数X方向，y方向
            float scaleXValue = screenWidth / viewWidth;
            float scaleYValue = screenHeight / viewHeight;
            UIApplication.SharedApplication.Windows[0].RootViewController.View.Transform = CGAffineTransform.MakeScale(scaleXValue, scaleYValue);
            //如果界面存在偏移，可用一下方法移动视图位置 
            UIApplication.SharedApplication.Windows[0].RootViewController.View.Frame = new CGRect(0, 0, (int)screenWidth, (int)screenHeight);
        }
#endif
#if __ANDROID__
        /// <summary>
        ///  不设置PreferredBackBuffer宽度和高度使用此方法,放大触控点坐标
        /// </summary>
        /// <param name="view">Android项目渲染游戏界面的view</param>
        /// <param name="sourceResolution">源分辨率</param>
        /// <param name="destinationResolution">目标分辨率，通常设置设备实际分辨率</param>
        /// <returns></returns>
        public static global::Android.Views.View OnCreate(global::Android.Views.View view,global::Android.Graphics.Point sourceResolution, global::Android.Graphics.Point destinationResolution)
        {
            view.ScaleX = ((float)destinationResolution.X)/((float)sourceResolution.X);
            view.ScaleY = ((float)destinationResolution.Y) / ((float)sourceResolution.Y); ;
            view.PivotX = 0;
            view.PivotY = 0;
            return (view);
        }
        /// <summary>
        ///  不设置PreferredBackBuffer宽度和高度使用此方法,全屏放大触控点坐标
        /// </summary>
        /// <param name="view">Android项目渲染游戏界面的view</param>
        /// <param name="sourceResolution">源分辨率</param>
        /// <returns></returns>
        public static global::Android.Views.View OnCreate(global::Android.Views.View view, global::Android.Graphics.Point sourceResolution)
        {
            global::Android.Graphics.Point destinationResolution = new global::Android.Graphics.Point();

            (view.Context as global::Android.App.Activity).WindowManager.DefaultDisplay.GetSize(destinationResolution);
            int width = destinationResolution.X;
            int height = destinationResolution.Y;

            var _view = ScalingClever.ResolutionScaling.OnCreate(view, sourceResolution, new global::Android.Graphics.Point(width, height));

            return (_view);
        }
#endif
#if WINDOWS_PHONE
        private static Microsoft.Phone.Controls.PhoneApplicationPage Instance = null;
        /// <summary>
        /// WP8适配WP8.1,UWP手机等比例放大
        /// </summary>
        /// <param name="gamePage">Page类</param>
        /// <param name="xnaSurface">XnaSurface</param>
        /// <param name="thickness">设置游戏屏幕与手机屏幕边缘的距离</param>
        public static void SetupScreenAutoScaling(Microsoft.Phone.Controls.PhoneApplicationPage gamePage, System.Windows.Controls.DrawingSurface xnaSurface,ViewportMargin viewportMargin)
        {
            if (Instance != null)
                throw new InvalidOperationException("There can be only one GamePage object!");

            Instance = gamePage;
           
            int? scaleFactor = null;
            var content = System.Windows.Application.Current.Host.Content; 
             var scaleFactorProperty = content.GetType().GetProperty("ScaleFactor");

            if (scaleFactorProperty != null)
                scaleFactor = scaleFactorProperty.GetValue(content, null) as int?;

            if (scaleFactor == null)
                scaleFactor = 100; 

            double scale = ((double)scaleFactor) / 100.0;

     

            if (scaleFactor == 150)
            {
                // Centered letterboxing - move Margin.Left to the right by ((1280-1200)/2)/scale
                if (viewportMargin == ViewportMargin.Top)
                {
                    xnaSurface.Margin = new System.Windows.Thickness(0, 40 / scale, 0, 0);
                }
                else if (viewportMargin == ViewportMargin.Left)
                {
                    xnaSurface.Margin = new System.Windows.Thickness(40 / scale,0 , 0, 0);
                }
                else if (viewportMargin == ViewportMargin.Right)
                {
                    xnaSurface.Margin = new System.Windows.Thickness(0, 0, 40 / scale, 0);
                }
                else if (viewportMargin == ViewportMargin.Right)
                {
                    xnaSurface.Margin = new System.Windows.Thickness(0, 0,0, 40 / scale);
                }
            }

            // Scale the XnaSurface: 

            System.Windows.Media.ScaleTransform scaleTransform = new System.Windows.Media.ScaleTransform();
            scaleTransform.ScaleX = scaleTransform.ScaleY = scale;
            xnaSurface.RenderTransform = scaleTransform;
        }
        /// <summary>
        /// WP8适配WP8.1,UWP手机等比例放大
        /// </summary>
        /// <param name="gamePage">Page类</param>
        /// <param name="xnaSurface">XnaSurface</param>
        public static void SetupScreenAutoScaling(Microsoft.Phone.Controls.PhoneApplicationPage gamePage, System.Windows.Controls.DrawingSurface xnaSurface)
        {
            if (Instance != null)
                throw new InvalidOperationException("There can be only one GamePage object!");

            Instance = gamePage;
            // Get the screen’s WVGA ''ScaleFactor'' via reflection.  scaleFactor will be 100 (WVGA), 160 (WXGA), or 150 (WXGA).
            int? scaleFactor = null;
            var content = System.Windows.Application.Current.Host.Content;
            var scaleFactorProperty = content.GetType().GetProperty("ScaleFactor");

            if (scaleFactorProperty != null)
                scaleFactor = scaleFactorProperty.GetValue(content, null) as int?;

            if (scaleFactor == null)
                scaleFactor = 100; 

            double scale = ((double)scaleFactor) / 100.0; 

        

            System.Windows.Media.ScaleTransform scaleTransform = new System.Windows.Media.ScaleTransform();
            scaleTransform.ScaleX = scaleTransform.ScaleY = scale;
            xnaSurface.RenderTransform = scaleTransform;
        }

#endif
    }
#if WINDOWS_PHONE
    public  enum ViewportMargin
    {
        Left=0,
        Top,
        Right,
        Bottom
    }
#endif
    }
