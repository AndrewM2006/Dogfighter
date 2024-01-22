using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Dogfighter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D planeTexture, shotTexture, ammoTexture, missleTexture, enemyPlaneTexture, coinTexture, plane2Texture, plane3Texture, plane4Texture;
        List<Texture2D> achievementButton = new List<Texture2D>();
        List<Texture2D> storeButton = new List<Texture2D>();
        List<Texture2D> playButton = new List<Texture2D>();
        List<Texture2D> h2pButton = new List<Texture2D>();
        List<Texture2D> bgFrames = new List<Texture2D>();
        Texture2D speedTexture, rateTexture, amountTexture, rectangleTexture, UITexture, creditcardTexture, scopeTexture;
        List<bool> whatTexture;
        Vector2 mousesPoint;
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
        SpriteFont ammoamount, pressSpace, priceFont, icerbergFont;
        int ammorate, enemyrate, superammoRate, startAmmo, startSuper, kills;
        private FrameCounter frameCounter = new FrameCounter();
        float planeSpeed, enemySpeed, enemyFiringRate, enemyShotSpeed;
        float seconds, coins, gameseconds;
        float startTime;
        bool planeAnimation, creditcard;
        int store, play, h2p, achievments;
        List<string> speedUpgrades, amountUpgrades, rateUpgrades;
        Rectangle storeRect, playRect, h2pRect, achievmentsRect;
        Rectangle plane2, plane3, plane4;
        Rectangle speedRect, amountRect, rateRect, coinsRect;
        Point mouseLocation;
        string creditcardnum;
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
            whatTexture = new List<bool>();
            base.Initialize();
            screen = Screen.Start;
            planeSpeed = 3; enemySpeed = 2; enemyFiringRate =5; enemyShotSpeed = 4f; startAmmo=1; startSuper = 0;
            plane = new Plane(planeTexture, planeSpeed, circleTexture, startAmmo, startSuper);
            shots = new List<Shot>(); supershots = new List<Shot>();
            ammolist = new List<Ammo>();
            enemyPlanes = new List<EnemyPlane>();
            speedUpgrades = new List<String>(); amountUpgrades = new List<String>(); rateUpgrades = new List<String>();
            ammorate = 10; enemyrate = 10; superammoRate = 30;
            planeAnimation = false;
            store=0; play = 0; h2p = 0; achievments = 0;
            coins = 0; creditcard = false;
            creditcardnum = "";
            plane2 = new Rectangle(200, 310, 120, 60); plane3 = new Rectangle(360, 310, 120, 60); plane4 = new Rectangle(520, 310, 120, 60);
            storeRect = new Rectangle(630, 400, 150, 30); playRect = new Rectangle(430, 400, 150, 30); h2pRect = new Rectangle(230, 400, 150, 30); achievmentsRect = new Rectangle(30, 400, 150, 30);
            speedRect = new Rectangle(6, 97, 256, 171); amountRect = new Rectangle(272, 97, 256, 171); rateRect = new Rectangle(538, 97, 256, 171);
            coinsRect = new Rectangle(520, 10, 200, 50);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");
            plane2Texture = Content.Load<Texture2D>("airplane2");
            plane3Texture = Content.Load<Texture2D>("airplane3");
            plane4Texture = Content.Load<Texture2D>("airplane4");
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
            pressSpace = Content.Load<SpriteFont>("PressSpace");
            speedTexture = Content.Load<Texture2D>("SpeedUpgrade");
            amountTexture = Content.Load<Texture2D>("scrnli_1_17_2024_10-02-11_AM-removebg-preview");
            rateTexture = Content.Load<Texture2D>("AmmoRateUpgrade");
            rectangleTexture = Content.Load<Texture2D>("RectangleTexture");
            priceFont = Content.Load<SpriteFont>("PriceFont");
            UITexture = Content.Load<Texture2D>("GameScreenUI");
            icerbergFont = Content.Load<SpriteFont>("IcebergFont");
            creditcardTexture = Content.Load<Texture2D>("credit_card");
            scopeTexture = Content.Load<Texture2D>("Scope");
            for (int i=0; i<6; i++)
            {
                whatTexture.Add(false);
            }
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
                else if (playRect.Contains(mouseLocation))
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
                            if (whatTexture[3]==false && whatTexture[4]==false && whatTexture[5] == false)
                            {
                                plane._texture = planeTexture;
                            }
                            else if (whatTexture[3] == true)
                            {
                                plane._texture = plane2Texture;
                            }
                            else if (whatTexture[4] == true)
                            {
                                plane._texture = plane3Texture;
                            }
                            else
                            {
                                plane._texture = plane4Texture;
                            }
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
                mousesPoint = new Vector2(mouseState.X, mouseState.Y);
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
                if (generator.Next(1201) == 0)
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
                previousState = keyboardState;
                keyboardState = Keyboard.GetState();
                previousMousestate = mouseState;
                mouseState = Mouse.GetState();
                mouseLocation = new Point(mouseState.X, mouseState.Y);
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    screen = Screen.Start;
                }
                if (plane2.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && (coins>=100 || whatTexture[0]==true))
                {
                    if (whatTexture[0]== false)
                    {
                        coins -= 100;
                        whatTexture[0] = true;
                        whatTexture[3] = true;
                        whatTexture[4] = false;
                        whatTexture[5] = false;
                    }
                    else
                    {
                        if (whatTexture[3] == true)
                        {
                            whatTexture[3] = false;
                        }
                        else
                        {
                            whatTexture[3] = true;
                            whatTexture[4] = false;
                            whatTexture[5] = false;
                        }
                    }
                }
                if (plane3.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && (coins >= 200 || whatTexture[1] == true))
                {
                    if (whatTexture[1] == false)
                    {
                        coins -= 200;
                        whatTexture[1] = true;
                        whatTexture[4] = true;
                        whatTexture[3] = false;
                        whatTexture[5] = false;
                    }
                    else
                    {
                        if (whatTexture[4] == true)
                        {
                            whatTexture[4] = false;
                        }
                        else
                        {
                            whatTexture[4] = true;
                            whatTexture[5] = false;
                            whatTexture[3] = false;
                        }
                    }
                }
                if (plane4.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && (coins >= 300 || whatTexture[2] == true))
                {
                    if (whatTexture[2] == false)
                    {
                        coins -= 300;
                        whatTexture[2] = true;
                        whatTexture[5] = true;
                        whatTexture[4] = false;
                        whatTexture[3] = false;
                    }
                    else
                    {
                        if (whatTexture[5] == true)
                        {
                            whatTexture[5] = false;
                        }
                        else
                        {
                            whatTexture[5] = true;
                            whatTexture[4] = false;
                            whatTexture[3] = false;
                        }
                    }
                }
                if (speedRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && coins >= (speedUpgrades.Count+1)*10)
                {
                    coins-= (speedUpgrades.Count+1)*10;
                    speedUpgrades.Add("Upgraded");
                    planeSpeed++;
                }
                if (amountRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && coins >= (amountUpgrades.Count + 1) * 10)
                {
                    coins -= (amountUpgrades.Count + 1) * 10;
                    amountUpgrades.Add("Upgraded");
                    startAmmo++;
                    if(generator.Next(4)==0)
                    {
                        startSuper++;
                    }
                }
                if (rateRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed && previousMousestate.LeftButton == ButtonState.Released && coins >= (rateUpgrades.Count + 1) * 10)
                {
                    coins -= (rateUpgrades.Count + 1) * 10;
                    rateUpgrades.Add("Upgraded");
                    if (ammorate > 1)
                    {
                        ammorate -= 1;
                    }
                    if (superammoRate > 1)
                    {
                        superammoRate-= 1;
                    }
                }
                if (coinsRect.Contains(mouseLocation) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    creditcard = true;
                }
                if (creditcard && keyboardState.IsKeyDown(Keys.Enter))
                {
                    creditcard = false;
                    if (creditcardnum.Length == 14)
                    {
                        coins += 1000;
                    }
                    creditcardnum = "";
                }
                else if (creditcard && keyboardState.IsKeyDown(Keys.Back) && previousState.IsKeyUp(Keys.Back))
                {
                    if (creditcardnum.Length > 0)
                    {
                        creditcardnum = creditcardnum.Remove(creditcardnum.Length-1);
                    }
                }
                else if (creditcard && (keyboardState.IsKeyDown(Keys.D1) && previousState.IsKeyUp(Keys.D1)) || (keyboardState.IsKeyDown(Keys.D2) && previousState.IsKeyUp(Keys.D2)) || (keyboardState.IsKeyDown(Keys.D3) && previousState.IsKeyUp(Keys.D3)) || (keyboardState.IsKeyDown(Keys.D4) && previousState.IsKeyUp(Keys.D4)) || (keyboardState.IsKeyDown(Keys.D5) && previousState.IsKeyUp(Keys.D5)) || (keyboardState.IsKeyDown(Keys.D6) && previousState.IsKeyUp(Keys.D6)) || (keyboardState.IsKeyDown(Keys.D7) && previousState.IsKeyUp(Keys.D7)) || (keyboardState.IsKeyDown(Keys.D8) && previousState.IsKeyUp(Keys.D8)) || (keyboardState.IsKeyDown(Keys.D9) && previousState.IsKeyUp(Keys.D9)) || (keyboardState.IsKeyDown(Keys.D0) && previousState.IsKeyUp(Keys.D0)))
                {
                    if (creditcardnum.Length < 14)
                    {
                        creditcardnum += "*";
                    }
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
                    this.IsMouseVisible = true;
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
                this.IsMouseVisible = false;
                _spriteBatch.Draw(bgFrames[bgFrame], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
                _spriteBatch.Draw(UITexture, new Rectangle(_graphics.PreferredBackBufferWidth - 250, _graphics.PreferredBackBufferHeight - 150, 250, 150), Color.White);
                _spriteBatch.Draw(logoFrames[logoFrames.Count-1], new Rectangle(596, 355, 160, 110), Color.White * 0.5f);
                _spriteBatch.DrawString(icerbergFont, "Kills: " + kills, new Vector2(590, 364), Color.White);
                _spriteBatch.DrawString(icerbergFont, "Time: " + Math.Round(gameseconds), new Vector2(589, 410), Color.White);
                _spriteBatch.Draw(ammoTexture, new Rectangle(687, 373, 45, 15), Color.Red);
                _spriteBatch.DrawString(icerbergFont, ": " + plane._ammo, new Vector2(740, 362), Color.White);
                _spriteBatch.Draw(ammoTexture, new Rectangle(687, 419, 45, 15), Color.White);
                _spriteBatch.DrawString(icerbergFont, ": " + plane._superammo, new Vector2(740, 408), Color.White);
                _spriteBatch.Draw(scopeTexture, new Rectangle((int)mousesPoint.X - 5, (int)mousesPoint.Y - 5, 20, 20), Color.White);
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
                _spriteBatch.End();
            }
            else if (screen == Screen.Shop)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(planeFrames[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                _spriteBatch.Draw(coinTexture, new Rectangle(10, 10, 45, 45), Color.White);
                _spriteBatch.DrawString(ammoamount, "X " + coins, new Vector2(60, 25), Color.White);
                if (whatTexture[3] == false)
                {
                    _spriteBatch.Draw(plane2Texture, plane2, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(plane2Texture, plane2, Color.LightBlue);
                }
                if (whatTexture[0] == false)
                {
                    _spriteBatch.Draw(coinTexture, new Rectangle(220, 370, 20, 20), Color.White);
                    _spriteBatch.DrawString(ammoamount, "X 100", new Vector2(242, 372), Color.White);
                }
                if (whatTexture[4]  == false)
                {
                    _spriteBatch.Draw(plane3Texture, plane3, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(plane3Texture, plane3, Color.LightBlue);
                }
                if (whatTexture[1] == false)
                {
                    _spriteBatch.Draw(coinTexture, new Rectangle(380, 370, 20, 20), Color.White);
                    _spriteBatch.DrawString(ammoamount, "X 200", new Vector2(402, 372), Color.White);
                }
                if (whatTexture[5] == false)
                {
                    _spriteBatch.Draw(plane4Texture, plane4, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(plane4Texture, plane4, Color.LightBlue);
                }
                if (whatTexture[2] == false)
                {
                    _spriteBatch.Draw(coinTexture, new Rectangle(540, 370, 20, 20), Color.White);
                    _spriteBatch.DrawString(ammoamount, "X 300", new Vector2(562, 372), Color.White);
                }
                _spriteBatch.Draw(rectangleTexture, speedRect, Color.Orange);
                _spriteBatch.Draw(speedTexture, new Rectangle(9, 100, 250, 125), Color.White);
                _spriteBatch.Draw(coinTexture, new Rectangle(89, 230, 25, 25), Color.White);
                _spriteBatch.DrawString(priceFont, "X " + (speedUpgrades.Count+1)*10, new Vector2(124, 230), Color.Black);
                _spriteBatch.Draw(rectangleTexture, amountRect, Color.LightBlue);
                _spriteBatch.Draw(amountTexture, new Rectangle(275, 100, 250, 125), Color.White);
                _spriteBatch.Draw(coinTexture, new Rectangle(355, 230, 25, 25), Color.White);
                _spriteBatch.DrawString(priceFont, "X " + (amountUpgrades.Count + 1) * 10, new Vector2(390, 230), Color.Black);
                _spriteBatch.Draw(rectangleTexture, rateRect, Color.Magenta);
                _spriteBatch.Draw(rateTexture, new Rectangle(543, 100, 250, 125), Color.White);
                _spriteBatch.Draw(coinTexture, new Rectangle(621, 230, 25, 25), Color.White);
                _spriteBatch.DrawString(priceFont, "X " + (rateUpgrades.Count + 1) * 10, new Vector2(656, 230), Color.Black);
                _spriteBatch.DrawString(pressSpace, "PRESS SPACE TO RETURN", new Vector2(180, 400), Color.White);
                if (!creditcard)
                {
                    _spriteBatch.Draw(rectangleTexture, coinsRect, Color.Gold);
                    _spriteBatch.DrawString(pressSpace, "BUY", new Vector2(545, 10), Color.Black);
                    _spriteBatch.Draw(coinTexture, new Rectangle(643, 12, 45, 45), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(creditcardTexture, coinsRect, Color.White);
                    _spriteBatch.DrawString(ammoamount, creditcardnum, new Vector2(600, 46), Color.Black);
                }
                _spriteBatch.End();
            }
            else if (screen == Screen.Achievements)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(planeFrames[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                _spriteBatch.DrawString(pressSpace, "PRESS SPACE TO RETURN", new Vector2(180, 400), Color.White);
                _spriteBatch.End();
            }
            else if (screen == Screen.H2P)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(planeFrames[0], new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
                _spriteBatch.DrawString(pressSpace, "PRESS SPACE TO RETURN", new Vector2(180, 400), Color.White);
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
            if (generator.Next(1) == 0)
            {
                enemySpeed++;
            }
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