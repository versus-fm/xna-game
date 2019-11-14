using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Game5.Data;
using Game5.Data.Helper;
using Game5.Data.LuaAPI;
using Game5.Data.LuaAPI.AddonAPI;
using Game5.DependencyInjection;
using Game5.Env;
using Game5.Env.ECS.Component;
using Game5.Env.World;
using Game5.Graphics;
using Game5.Input;
using Game5.Network;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.StateBased;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;

namespace Game5
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class ExtendedGame : Game
    {
        private static Queue<int> messageQueue;

        /// <summary>
        ///     The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        ///     The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtendedGame" /> class.
        /// </summary>
        /// 

        private ICamera camera;
        private IInput input;
        private IGameTimeService gameTimeService;
        private IStateService stateService;
        private IClient client;

        public ExtendedGame()
        {
            SteamAPI.Init();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected sealed override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected sealed override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (!Directory.Exists("cfg")) Directory.CreateDirectory("cfg");
            if (!Directory.Exists("addons")) Directory.CreateDirectory("addons");


            ComponentCache.RegisterComponents();
            HeaderCache.RegisterHeaders();
            NetworkMethodCache.RegisterSendables();
            TileTypeCache.RegisterTileTypes();

            ServiceLocator.RegisterService(Content);
            ServiceLocator.RegisterService(spriteBatch);
            ServiceLocator.RegisterService(graphics);

            ServiceLocator.RegisterImplementations();
            DefineImplementations();
            ServiceLocator.RegisterServices();

            GameState.RegisterStates();
            ServiceLocator.Get<IPerformanceProfiler>().StartSession(false);
            for (var i = 0; i < 1000; i++)
            {
                //var world = new ObjectFactory<ThreeTierWorld>().Make(1, 1);
            }

            var time = ServiceLocator.Get<IPerformanceProfiler>().StopSession();
            Debug.WriteLine(time.TotalTime.Milliseconds);

            ServiceLocator.Get<IPropertyService>().Load("app");
            SteamCallback.Create();
            LuaContext.LoadAddons();

            Window.TextInput += ServiceLocator.Get<ITextInputService>().Execute;

            camera = ServiceLocator.Get<ICamera>();
            input = ServiceLocator.Get<IInput>();
            gameTimeService = ServiceLocator.Get<IGameTimeService>();
            stateService = ServiceLocator.Get<IStateService>();
            client = ServiceLocator.Get<IClient>();


            LoadGameContent();
        }

        protected virtual void LoadGameContent()
        {
        }

        protected virtual void DefineImplementations()
        {
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            SteamAPI.Shutdown();
            base.OnExiting(sender, args);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected sealed override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected sealed override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            gameTimeService.SupplyGameTime(gameTime);
            SteamAPI.RunCallbacks();
            client.Update(gameTime);
            input.PreUpdate();
            stateService.UpdateUI();
            camera.Update();
            stateService.Update();
            input.PostUpdate();

            if (messageQueue != null)
                while (messageQueue.Count > 0)
                    ProcessMessage(messageQueue.Dequeue());
            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected sealed override void Draw(GameTime gameTime)
        //{
        //    RenderTarget2D renderTarget2D = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
        //    this.GraphicsDevice.SetRenderTarget(renderTarget2D);

        //    DrawUI(gameTime);
        //    ServiceLocator.GetClippedRenderUnit().Draw(renderTarget2D);

        //    this.GraphicsDevice.Clear(Color.CornflowerBlue);
        //    DrawGame(gameTime);

        //    this.GraphicsDevice.SetRenderTarget(null);
        //    this.spriteBatch.Begin(SpriteSortMode.FrontToBack);
        //    this.spriteBatch.Draw(renderTarget2D, new Vector2(0, 0), Color.White);
        //    this.spriteBatch.End();
        //    renderTarget2D.Dispose();

        //    // TODO: Add your drawing code here
        //    base.Draw(gameTime);
        //}
        private void ProcessMessage(int message)
        {
            switch (message)
            {
                case 0:
                    Exit();
                    break;
            }
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            DrawUI();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawGame();
            FinalizeUI();
            base.Draw(gameTime);
        }

        protected void FinalizeUI()
        {
            stateService.FinalizeDrawUI();
        }

        protected void DrawUI()
        {
            stateService.DrawUI();
        }

        protected void DrawGame()
        {
            stateService.DrawWorld();
        }

        public static void SendMessage(int message)
        {
            if (messageQueue == null) messageQueue = new Queue<int>();
            messageQueue.Enqueue(message);
        }
    }
}