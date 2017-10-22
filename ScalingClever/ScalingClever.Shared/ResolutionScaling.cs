﻿#if __IOS__
using CoreGraphics;
using UIKit;
#endif

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


namespace ScalingClever
{
    public class ResolutionScaling
    {
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
        /// 设置PreferredBackBuffer宽度和高度且窗口大小不会改变（iOS，Android），触控坐标与绘图坐标不一致时使用
        /// </summary>
        /// <param name="sourceResolution">源分辨率</param>
        /// <param name="destinationResolution">目标分辨率，通常设置设备实际分辨率</param>
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
        /// 设置PreferredBackBuffer宽度和高度且窗口大小不会改变（iOS，Android），触控坐标与绘图坐标不一致时使用，全屏放大
        /// </summary>
        /// <param name="game">游戏类</param>
        /// <param name="sourceResolution">源分辨率</param>
        public static void Initialize(Game game,Point sourceResolution)
        {
            Point destinationResolution = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            Initialize(sourceResolution,destinationResolution);
        }

        /// <summary>
        /// 设置PreferredBackBuffer宽度和高度且窗口大小会改变（UWP），触控坐标与绘图坐标不一致时使用
        /// </summary>
        /// <param name="sourceResolution">源分辨率</param>
        /// <param name="destinationResolution">目标分辨率，通常设置设备实际分辨率</param>
        public static void Draw(Point sourceResolution,Point destinationResolution)
        {
            Initialize(sourceResolution, destinationResolution);
        }
        /// <summary>
        /// 设置PreferredBackBuffer宽度和高度且窗口大小会改变（UWP），触控坐标与绘图坐标不一致时使用，全屏放大
        /// </summary>
        /// <param name="game">游戏类</param>
        /// <param name="sourceResolution">源分辨率</param>
        public static void Draw(Game game, Point sourceResolution)
        {
            Point destinationResolution = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            Initialize(sourceResolution, destinationResolution);
        }

        public static Vector2 Position(Vector2 vector2)
        {
            return new Vector2(vector2.X * scalingPositionX, vector2.Y * scalingPositionY);
        }
        public static float X(float x)
        {
            return x * scalingPositionX;
        }
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
    }
}
