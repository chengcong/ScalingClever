# ScalingClever
MonoGame 适配多种设备分辨率运行库。
功能：1.根据设备分辨率等比例放大游戏界面，全屏适配。2.触控点根据设备分辨率等比例放大，全屏适配

使用方法：
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace ScalingCleverDemo
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D panda;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TouchPanel.EnableMouseTouchPoint = true;
        }


        protected override void Initialize()
        {
            // 初始化ScalingClever，设置800x480，游戏制作使用的虚拟分辨率，目标分辨率为设备分辨率
            ScalingClever.ResolutionScaling.Initialize(this, new Point(800, 480), new Point(this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));

            base.Initialize();
        }

        


        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            var touches = TouchPanel.GetState();
            foreach (var touch in touches)
            {
                if (touch.State != TouchLocationState.Pressed)
                {
                    // 缩放后，坐标点位置
                    
                    var postion = ScalingClever.ResolutionScaling.Position(touch.Position);
                    System.Diagnostics.Debug.WriteLine(postion.X + "," + postion.Y);
                    
                    // 缩放后，坐标点X值
                    
                    var X = ScalingClever.ResolutionScaling.X(touch.Position.X);
                    
                    // 缩放后，坐标点Y值
                    
                    var Y = ScalingClever.ResolutionScaling.Y(touch.Position.Y);
                    System.Diagnostics.Debug.WriteLine(X + "," + Y);
                }
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // 放大界面
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ScalingClever.ResolutionScaling.ScalingMatrix);
            
            // UWP平台放大缩小窗口比例，设置800x480，游戏制作使用的虚拟分辨率，目标分辨率为窗口分辨率
            
            ScalingClever.ResolutionScaling.Draw(new Point(800, 480), new Point(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));
            spriteBatch.Draw(panda, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

