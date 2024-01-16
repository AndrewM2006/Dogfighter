using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Dogfighter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D planeTexture, shotTexture, ammoTexture, missleTexture, enemyPlaneTexture, coinTexture;
        List<Texture2D> achievementButton = new List<Texture2D>();
        List<Texture2D> storeButton = new List<Texture2D>();
        List<Texture2D> playButton = new List<Texture2D>();
        List<Texture2D> h2pButton = new List<Texture2D>();
        List<Texture2D> bgFrames = new List<Texture2D>();
        MouseState mouseState, previousMousestate;
        private Texture2D circleTexture;
        KeyboardState keyboardState, previousState;
        List<Texture2D> planeFrames = new List<Texture2D>();
        int planeFrame=0; int logoFrame=0; int bgFrame = 0;
        List<Texture2D> logoFrames = new List<Texture2D>();
        List<Shot> shots, supershots;
        List<Ammo> ammolist;
        List<EnemyPlane> enemyPlanes;
        Random generator = new Random();
        Plane plane;
        SpriteFont ammoamount;
        int ammorate, enemyrate, superammoRate, startAmmo, startSuper, kills;
        private FrameCounter frameCounter = new FrameCounter();
        float planeSpeed, enemySpeed, enemyFiringRate, enemyShotSpeed;
        float seconds, coins, gameseconds;
        float startTime;
        bool planeAnimation;
        int store, play, h2p, achievments;
        Rectangle storeRect, playRect, h2pRect, achievmentsRect;
        Point mouseLocation;
        enum Screen
        {
            Start,
            Shop,
            Play,
            Achievements,
            H2P
        }
        Screen screen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.ApplyChanges();
            this.Window.Title = "Aerial Assault";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            screen = Screen.Start;
            planeSpeed = 3; enemySpeed = 2; enemyFiringRate = 10; enemyShotSpeed = 4f; startAmmo=1; startSuper = 0;
            plane = new Plane(planeTexture, planeSpeed, circleTexture, startAmmo, startSuper);
            shots = new List<Shot>(); supershots = new List<Shot>();
            ammolist = new List<Ammo>();
            enemyPlanes = new List<EnemyPlane>();
            ammorate = 10; enemyrate = 12; superammoRate = 30;
            planeAnimation = false;
            store=0; play = 0; h2p = 0; achievments = 0;
            storeRect = new Rectangle(630, 400, 150, 30); playRect = new Rectangle(430, 400, 150, 30); h2pRect = new Rectangle(230, 400, 150, 30); achievmentsRect = new Rectangle(30, 400, 150, 30);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");
            enemyPlaneTexture = Content.Load<Texture2D>("EnemyPlane");
            circleTexture = Content.Load<Texture2D>("circle");
            shotTexture = Content.Load<Texture2D>("LaserShotRed");
            ammoTexture = Content.Load<Texture2D>("Ammo");
            ammoamount = Content.Load<SpriteFont>("AmmoAmount");
            missleTexture = Content.Load<Texture2D>("Missle");
            achievementButton.Add(Content.Load<Texture2D>("Achievements"));
            achievementButton.Add(Content.Load<Texture2D>("AchievmentsHover"));
            storeButton.Add(Content.Load<Texture2D>("Store"));
            storeButton.Add(Content.Load<Texture2D>("StoreHover"));
            playButton.Add(Content.Load<Texture2D>("PlayNow"));
            playButton.Add(Content.Load<Texture2D>("PlayNowHover"));
            h2pButton.Add(Content.Load<Texture2D>("H2P"));
            h2pButton.Add(Content.Load<Texture2D>("H2PHover"));
            coinTexture = Content.Load<Texture2D>("AACoin");
            for (int i = 0; i < 77; i++)
            {
                if (i < 10)
                {
                    planeFrames.Add(Content.Load<Texture2D>("Copy of frame_0" + i + "_delay-0.07s"));
                }
                else
                {
                    planeFrames.Add(Content.Load<Texture2D>("Copy of frame_" + i + "_delay-0.07s"));
                }
            }
            for (int i = 0; i < 179; i++)
            {
                bgFrames.Add(Content.Load<Texture2D>("Copy of frame_" + i + "_delay-0.06s"));
            }
            for (int i = 0; i < 15; i++)
            {
                logoFrames.Add(Content.Load<Texture2D>("frame_" + i + "_delay-0.08s"));
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (screen == Screen.Start)
            {
                seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
                if (seconds > 0.07 && logoFrame < logoFrames.Count-1)
                {
                    logoFrame++;
                    startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                }
                previousMousestate = mouseState;
                mouseState = Mouse.GetState();
                mouseLocation = new Point(mouseState.X, mouseState.Y);
                if (achievmentsRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released)
                {
                    screen = Screen.Achievements;
                    logoFrame = 0;
                }
                else if (achievmentsRect.Contains(mouseLocation))
                {
                    achievments = 1;
                }
                else
                {
                    achievments = 0;
                }
                if (playRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released)
                {
                    planeAnimation = true;
                    plane._ammo = startAmmo;
                    plane._superammo = startSuper;
                    plane._speed = planeSpeed;
                    gameseconds = 0;
                }
                else if ((playRect.Contains(mouseLocation)))
                {
                    play = 1;
                }
                else
                {
                    play = 0;
                }
                if (h2pRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released)
                {
                    screen = Screen.H2P;
                    logoFrame = 0;
                }
                else if (h2pRect.Contains(mouseLocation))
                {
                    h2p = 1;
                }
                else
                {
                    h2p = 0;
                }
                if (storeRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released)
                {
                    screen = Screen.Shop;
                    logoFrame = 0;
                }
                else if (storeRect.Contains(mouseLocation))
                {
                    store = 1;
                }
                else
                {
                    store = 0;
                }
                if (planeAnimation)
                {
                    seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
                    if (seconds > 0.047)
                    {
                        planeFrame++;
                        startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                        if (planeFrame == planeFrames.Count)
                        {
                            planeAnimation = false;
                            plane._dead = false;
                            screen = Screen.Play;
                            startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                        }
                    }
                }
            }
            else if (screen == Screen.Play)
            {
                gameseconds+= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (seconds > 0.045)
                {
                    bgFrame++;
                    startTime = (float)gameTime.TotalGameTime.TotalSeconds;
                    if (bgFrame == bgFrames.Count)
                    {
                        bgFrame = 0;
                    }
                }
                previousMousestate = mouseState;
                mouseState = Mouse.GetState();
                previousState = keyboardState;
                keyboardState = Keyboard.GetState();
                ammolist = plane.AmmoCollide(ammolist);
                plane.Update(_graphics, ammolist, enemyPlanes);
                seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
                if (keyboardState.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space) && plane._ammo > 0)
                {
                    shots.Add(new Shot(shotTexture, 10f, plane.Angle(), plane.Location(), plane.Movement(), circleTexture, Color.White));
                    plane._ammo--;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && plane._superammo > 0)
                {
                    supershots.Add(new Shot(shotTexture, 10f, plane.Angle(), plane.Location(), plane.Movement(), circleTexture, Color.Green));
                    plane._superammo--;
                }
                for (int i = 0; i < shots.Count; i++)
                {
                    shots[i].Update(_graphics);
                    if (shots[i].Offscreen)
                    {
                        shots.RemoveAt(i);
                    }
                }
                for (int i = 0; i < supershots.Count; i++)
                {
                    supershots[i].Update(_graphics);
                    if (supershots[i].Offscreen)
                    {
                        supershots.RemoveAt(i);
                    }
                }
                for (int i = 0; i < enemyPlanes.Count; i++)
                {
                    enemyPlanes[i].Update(plane, shots, (float)gameTime.TotalGameTime.TotalSeconds, shotTexture, _graphics);
                    shots = enemyPlanes[i].ShotCollide(shots);
                    supershots = enemyPlanes[i].SuperShotCollide(supershots);
                    enemyPlanes[i].SelfCollide(enemyPlanes, i);
                    for (int j = 0; j < enemyPlanes.Count; j++)
                    {
                        if (i != j)
                        {
                            enemyPlanes[i].ShotCollide(enemyPlanes[j].shots);
                        }
                    }
                    if (enemyPlanes[i].ShotDown)
                    {
                        enemyPlanes.RemoveAt(i);
                        kills++;
                    }
                }
                Spawn(gameTime);
                if (generator.Next(901) == 0)
                {
                    DifficultyIncrease();
                }
                if (plane._dead)
                {
                    screen = Screen.Start;
                    planeFrame = 0; logoFrame = 0;
                    ammolist.Clear();
                    enemyPlanes.Clear();
                    shots.Clear();
                    supershots.Clear();
                    enemyrate = 12; enemySpeed = 2; enemyFiringRate = 10; enemyShotSpeed = 4f;
                    coins += (float)(Math.Round(gameseconds/10) + kills);
                    kills = 0;
                }
            }
            else if (screen == Screen.Shop)
            {
                keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    screen = Screen.Start;
                }
            }
            else if (screen == Screen.Achievements)
            {
                keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    screen = Screen.Start;
                }
            }
            else if (screen == Screen.H2P)
            {
                keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    screen = Screen.Start;
                }
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (screen == Screen.Start)
            {
                _spriteBatch.Begin();
                if (!planeAnimation)
                {
                    _spriteBatch.Draw(planeFrames[planeFrame], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                    _spriteBatch.Draw(logoFrames[logoFrame], new Rectangle(85, 14, 650, 383), Color.White);
                    _spriteBatch.Draw(achievementButton[achievments],achievmentsRect, Color.White);
                    _spriteBatch.Draw(h2pButton[h2p], h2pRect, Color.White);
                    _spriteBatch.Draw(playButton[play], playRect, Color.White);
                    _spriteBatch.Draw(storeButton[store],storeRect, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(planeFrames[planeFrame], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                }
                _spriteBatch.End();
            }
            else if (screen == Screen.Play)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(bgFrames[bgFrame], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                foreach (EnemyPlane enemyPlane in enemyPlanes)
                {
                    enemyPlane.Draw(_spriteBatch);
                }
                plane.Draw(_spriteBatch);
                foreach (Shot shot in shots)
                {
                    shot.Draw(_spriteBatch);
                }
                foreach (Shot shot in supershots)
                {
                    shot.Draw(_spriteBatch);
                }
                foreach (Ammo ammo in ammolist)
                {
                    ammo.Draw(_spriteBatch);
                }
                _spriteBatch.DrawString(ammoamount, "Ammo: " + plane._ammo, new Vector2(725, 10), Color.White);
                _spriteBatch.DrawString(ammoamount, "Super Ammo: " + plane._superammo, new Vector2(679, 30), Color.White);
                _spriteBatch.DrawString(ammoamount, "Kills: " + kills, new Vector2(742, 50), Color.White);
                _spriteBatch.DrawString(ammoamount, "Seconds: " + Math.Round(gameseconds), new Vector2(708, 70), Color.White);
                _spriteBatch.End();
            }
            else if (screen == Screen.Shop)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(planeFrames[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                _spriteBatch.Draw(coinTexture, new Rectangle(10, 10, 45, 45), Color.White);
                _spriteBatch.DrawString(ammoamount, "X " + coins, new Vector2(60, 25), Color.White);
                _spriteBatch.End();
            }
            else if (screen == Screen.Achievements)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(planeFrames[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                _spriteBatch.End();
            }
            else if (screen == Screen.H2P)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(planeFrames[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }


        private void Spawn(GameTime gameTime)
        {
            frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (generator.Next(ammorate * (int)frameCounter.AverageFramesPerSecond + 1) == 1)
            {
                ammolist.Add(new Ammo(ammoTexture, Color.Red, circleTexture, _graphics));
            }
            if (generator.Next(superammoRate * (int)frameCounter.AverageFramesPerSecond + 1) == 1)
            {
                ammolist.Add(new Ammo(ammoTexture, Color.White, circleTexture, _graphics));
            }
            if (generator.Next(enemyrate * (int)frameCounter.AverageFramesPerSecond + 1) == 1)
            {
                enemyPlanes.Add(new EnemyPlane(enemyPlaneTexture, enemySpeed, circleTexture, enemyFiringRate, _graphics, enemyShotSpeed));
            }
            if (generator.Next(enemyrate * (int)frameCounter.AverageFramesPerSecond + 1) == 1)
            {
                enemyPlanes.Add(new EnemyPlane(missleTexture, enemyShotSpeed/2, circleTexture, float.PositiveInfinity, _graphics, enemyShotSpeed));
            }
        }

        private void DifficultyIncrease()
        {
            enemySpeed++;
            enemyShotSpeed++;
            if (enemyFiringRate > 1)
            {
                enemyFiringRate--;
            }
            if (enemyrate > 1)
            {
                enemyrate--;
            }
        }
    }
}