using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class Driver : Game
    {
        #region Declarations

        private readonly GraphicsDeviceManager graphics;

        //Level Info
        private Level current,
                      next;

        //For pausing the game
        private bool pause;
        private SpriteBatch spriteBatch;

        #endregion

        public Driver()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1248;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Set up first level
            current = new Logo();

            //Set pause to false
            pause = false;

            CollisionHelper.Initialize();
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Initialize current level
            current.Initialize();

            Components.Add(new GamerServicesComponent(this));

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            current.LoadContent(Content);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Update current level to determine next level
            next = current.Update(gameTime);
            if (next == null)
            {
                Exit();
            }
            else if (next != current)
            {
                current = next;

                if (!current.HasInitialized())
                {
                    current.Initialize();
                }

                if(!current.HasLoaded())
                {
                    current.LoadContent(Content);
                }

                graphics.GraphicsDevice.Viewport =
                    new Viewport(new Rectangle(0, 0, (int) Level.DEFAUT_SCREEN_SIZE.X,
                                               (int) Level.DEFAUT_SCREEN_SIZE.Y));
            }

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            current.Draw(spriteBatch, graphics, true);

            base.Draw(gameTime);
        }
    }
}