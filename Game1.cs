using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Dogfighter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D planeTexture, shotTexture, ammoTexture, missleTexture;
        MouseState mouseState, previousMousestate;
        private Texture2D circleTexture;
        KeyboardState keyboardState, previousState;
        List<Shot> shots, supershots;
        List<Ammo> ammolist, superammolist;
        List<EnemyPlane> enemyPlanes;
        Random generator = new Random();
        Plane plane;
        SpriteFont ammoamount;
        int ammorate, enemyrate, superammoRate;
        private FrameCounter frameCounter = new FrameCounter();
        float planeSpeed, enemySpeed, enemyFiringRate, enemyShotSpeed;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            planeSpeed = 3; enemySpeed = 2; enemyFiringRate = 10; enemyShotSpeed = 4f;
            plane = new Plane(planeTexture, planeSpeed, circleTexture);
            shots = new List<Shot>(); supershots = new List<Shot>();
            ammolist = new List<Ammo>();
            superammolist = new List<Ammo>();
            enemyPlanes = new List<EnemyPlane>();
            ammorate = 10; enemyrate = 12; superammoRate = 30;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");
            circleTexture = Content.Load<Texture2D>("circle");
            shotTexture = Content.Load<Texture2D>("LaserShotRed");
            ammoTexture = Content.Load<Texture2D>("Ammo");
            ammoamount = Content.Load<SpriteFont>("AmmoAmount");
            missleTexture = Content.Load<Texture2D>("Missle");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            previousMousestate = mouseState;
            mouseState = Mouse.GetState();
            previousState=keyboardState;
            keyboardState = Keyboard.GetState();
            ammolist = plane.AmmoCollide(ammolist);
            plane.Update(_graphics, ammolist, enemyPlanes);
            if (keyboardState.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space) && plane._ammo>0)
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
            for (int i =0;  i < enemyPlanes.Count; i++)
            {
                enemyPlanes[i].Update(plane, shots, (float)gameTime.TotalGameTime.TotalSeconds, shotTexture, _graphics);
                shots = enemyPlanes[i].ShotCollide(shots);
                supershots = enemyPlanes[i].SuperShotCollide(supershots);
                enemyPlanes[i].SelfCollide(enemyPlanes, i);
                for (int j=0; j<enemyPlanes.Count; j++)
                {
                    if (i != j)
                    {
                        enemyPlanes[i].ShotCollide(enemyPlanes[j].shots);
                    }
                }
                if (enemyPlanes[i].ShotDown)
                {
                    enemyPlanes.RemoveAt(i);
                }
            }
            Spawn(gameTime);
            if (generator.Next(901) == 0)
            {
                DifficultyIncrease();
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
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
            _spriteBatch.End();
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
                enemyPlanes.Add(new EnemyPlane(planeTexture, enemySpeed, circleTexture, enemyFiringRate, _graphics, enemyShotSpeed));
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