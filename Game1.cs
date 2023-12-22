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
        Texture2D planeTexture, shotTexture, ammoTexture;
        MouseState mouseState;
        Vector2 location, origin;
        Rectangle sourceRectangle;
        private Texture2D circleTexture;
        Rectangle circleHitbox;
        KeyboardState keyboardState, previousState;
        List<Shot> shots;
        List<Ammo> ammolist;
        List<EnemyPlane> enemyPlanes;
        Random generator = new Random();
        Plane plane;
        SpriteFont ammoamount;
        int ammorate;
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
            plane = new Plane(planeTexture, 5f, circleTexture);
            shots = new List<Shot>();
            ammolist = new List<Ammo>();
            enemyPlanes = new List<EnemyPlane>();
            ammorate = 10;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");
            circleTexture = Content.Load<Texture2D>("circle");
            shotTexture = Content.Load<Texture2D>("LaserShotRed");
            ammoTexture = Content.Load<Texture2D>("Ammo");
            ammoamount = Content.Load<SpriteFont>("AmmoAmount");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            previousState=keyboardState;
            keyboardState = Keyboard.GetState();
            ammolist = plane.AmmoCollide(ammolist);
            plane.Update(_graphics, ammolist);
            if (keyboardState.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space) && plane._ammo>0)
            {
                shots.Add(new Shot(shotTexture, 10f, plane.Angle(), plane.Location(), plane.Movement(), circleTexture));
                plane._ammo--;
            }
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Update(_graphics);
                if (shots[i].Offscreen)
                {
                    shots.RemoveAt(i);
                }
            }
            for (int i =0;  i < enemyPlanes.Count; i++)
            {
                enemyPlanes[i].Update(plane, shots);
                shots = enemyPlanes[i].ShotCollide(shots);
                if (enemyPlanes[i].ShotDown)
                {
                    enemyPlanes.RemoveAt(i);
                }
            }
            if (generator.Next(ammorate*60+1) == ammorate*60)
            {
                ammolist.Add(new Ammo(ammoTexture, Color.Red, circleTexture, _graphics));
            }
            if (generator.Next(601) == 600)
            {
                enemyPlanes.Add(new EnemyPlane(planeTexture, 3f, circleTexture, 10f, _graphics));
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
            foreach (Ammo ammo in ammolist)
            {
                ammo.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(ammoamount, "Ammo: " + plane._ammo, new Vector2(725, 10), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}