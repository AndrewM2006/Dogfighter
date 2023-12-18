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
        Texture2D planeTexture, shotTexture;
        MouseState mouseState;
        Vector2 location, origin;
        Rectangle sourceRectangle;
        private Texture2D circleTexture;
        Rectangle circleHitbox;
        KeyboardState keyboardState, previousState;
        List<Shot> shots;
        Plane plane;
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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");
            circleTexture = Content.Load<Texture2D>("circle");
            shotTexture = Content.Load<Texture2D>("LaserShotRed");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            previousState=keyboardState;
            keyboardState = Keyboard.GetState();
            plane.Update(_graphics);
            if (keyboardState.IsKeyDown(Keys.Space) && previousState.IsKeyUp(Keys.Space))
            {
                shots.Add(new Shot(shotTexture, 10f, plane.Angle(), plane.Location(), plane.Movement(), circleTexture));
            }
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Update(_graphics);
                if (shots[i].Offscreen)
                {
                    shots.RemoveAt(i);
                }
            }
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            plane.Draw(_spriteBatch);
            foreach (Shot shot in shots)
            {
                shot.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}