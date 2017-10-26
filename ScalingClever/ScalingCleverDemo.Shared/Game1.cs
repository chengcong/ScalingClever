using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace ScalingCleverDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D panda;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;

#if WINDOWS_UAP || WINDOWS

            IsMouseVisible = true;
            //this.Window.IsBorderless = false;

#endif

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            TouchPanel.EnableMouseTouchPoint = true;
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
  
            // TODO: Add your initialization logic here
            //ScalingClever.ResolutionScaling.Initialize(new Point(800, 480), new Point(this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));
            ScalingClever.ResolutionScaling.Initialize(this, new Point(800, 480));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ScalingClever.ResolutionScaling.LoadContent(this, new Point(800, 480));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            panda = Texture2D.FromStream(this.graphics.GraphicsDevice,TitleContainer.OpenStream("Content/panda.jpg"));
            // TODO: use this.Content to load your game content here
        }
        protected override bool BeginDraw()
        {
            ScalingClever.ResolutionScaling.BeginDraw(this);
            return base.BeginDraw();
        }
        protected override void EndDraw()
        {
            ScalingClever.ResolutionScaling.EndDraw(this, spriteBatch);
            base.EndDraw();
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            #region 鼠标点击，手指点击
            var touches = TouchPanel.GetState();
            foreach (var touch in touches)
            {
                if (touch.State != TouchLocationState.Pressed)
                {
                    var postion = ScalingClever.ResolutionScaling.Position(touch.Position);
                    System.Diagnostics.Debug.WriteLine(postion.X + "," + postion.Y);
                    var X = ScalingClever.ResolutionScaling.X(touch.Position.X);
                    var Y = ScalingClever.ResolutionScaling.Y(touch.Position.Y);
                    System.Diagnostics.Debug.WriteLine(X + "," + Y);
                    //var postion = touch.Position;
                    //System.Diagnostics.Debug.WriteLine(postion.X + "," + postion.Y);
                    //var X = touch.Position.X;
                    //var Y = touch.Position.Y;
                    //System.Diagnostics.Debug.WriteLine(X + "," + Y);
                }
            }
            #endregion
            #region 手势
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                if(gesture.GestureType==GestureType.Tap)
                {
                    //var postion = ScalingClever.ResolutionScaling.Position(gesture.Position);
                    //System.Diagnostics.Debug.WriteLine(postion.X + "," + postion.Y);
                    //var X = ScalingClever.ResolutionScaling.X(gesture.Position.X);
                    //var Y = ScalingClever.ResolutionScaling.Y(gesture.Position.Y);
                    //System.Diagnostics.Debug.WriteLine(X + "," + Y);
                    var postion = gesture.Position;
                    System.Diagnostics.Debug.WriteLine(postion.X + "," + postion.Y);
                    var X = gesture.Position.X;
                    var Y = gesture.Position.Y;
                    System.Diagnostics.Debug.WriteLine(X + "," + Y);
                }
            }
            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

           

            // TODO: Add your drawing code here
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ScalingClever.ResolutionScaling.ScalingMatrix);
            spriteBatch.Begin();

            //ScalingClever.ResolutionScaling.Draw(new Point(800, 480), new Point(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));

            spriteBatch.Draw(panda, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
