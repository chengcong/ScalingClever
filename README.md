# ScalingClever
MonoGame 适配多种设备分辨率运行库。
功能：1.根据设备分辨率等比例放大游戏界面，全屏适配。2.触控点根据设备分辨率等比例放大，全屏适配

使用方法：
1.初始化Initialize()方法中加入： ScalingClever.ResolutionScaling.Initialize(new Point(800, 480), new Point(this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));
或UWP平台 ScalingClever.ResolutionScaling.Initialize(new Point(800, 480), new Point(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));

2.放大游戏画面：Draw(GameTime gameTime)方法中修改 spriteBatch()为spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ScalingClever.ResolutionScaling.ScalingMatrix);

3.Update(GameTime gameTime)触控或点击坐标处理：

var touches = TouchPanel.GetState();

foreach (var touch in touches)

{

       if (touch.State != TouchLocationState.Pressed)
       {
       
           var postion = ScalingClever.ResolutionScaling.Position(touch.Position);
           
           var X = ScalingClever.ResolutionScaling.X(touch.Position.X);
           
           var Y = ScalingClever.ResolutionScaling.Y(touch.Position.Y);
           
                  
       }
       
}


