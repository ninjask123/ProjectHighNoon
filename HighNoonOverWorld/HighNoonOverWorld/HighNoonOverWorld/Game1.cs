using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HighNoonOverWorld
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D character;
        Texture2D background;
        Texture2D rock;
        Rectangle charPos;
        Rectangle mainFrame;
        Rectangle stationary;
        int speed = 5;
        private Camera2D camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            character = Content.Load<Texture2D>("placeholdersprite");
            rock = Content.Load<Texture2D>("placeholdersprite");
            background = Content.Load<Texture2D>("background");

            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width*2, GraphicsDevice.Viewport.Height*2);
            stationary = new Rectangle(500, 500, rock.Width, rock.Height);

            camera = new Camera2D(GraphicsDevice.Viewport);

            int screenheight = GraphicsDevice.Viewport.Height;
            int screenwidth = GraphicsDevice.Viewport.Width;
            charPos = new Rectangle((screenwidth - character.Width) / 2, (screenheight - character.Height) / 2, character.Width, character.Height);
        }

        protected override void UnloadContent()
        {
        
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            if (keystate.IsKeyDown(Keys.W) && keystate.IsKeyDown(Keys.A))
            {
                up();
                left();
            }
            else if (keystate.IsKeyDown(Keys.A) && keystate.IsKeyDown(Keys.S))
            {
                down();
                left();
            }
            else if (keystate.IsKeyDown(Keys.S) && keystate.IsKeyDown(Keys.D))
            {
                down();
                right();
            }
            else if (keystate.IsKeyDown(Keys.D) && keystate.IsKeyDown(Keys.W))
            {
                up();
                right();
            }
            else if (keystate.IsKeyDown(Keys.W))
            {
                up();
            }
            else if (keystate.IsKeyDown(Keys.A))
            {
                left();
            }
            else if (keystate.IsKeyDown(Keys.S))
            {
                down();
            }
            else if (keystate.IsKeyDown(Keys.D))
            {
                right();
            }

            camera.Input();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Tan);

            camera.Update();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            spriteBatch.Draw(rock, stationary, Color.Brown);
            spriteBatch.Draw(character, charPos, Color.Brown);
            spriteBatch.Draw(background, mainFrame, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void left()
        {
            if (!doesCollide(mainFrame, charPos, speed, 'l') && !doesCollide(stationary))
            {
                charPos.X -= speed;
                camera.moveCamera('l');
            }
            if (doesCollide(stationary))
            {
                stop(stationary, 'l');
                camera.moveCamera('r');
            }
        }
        public void right()
        {
            if (!doesCollide(mainFrame, charPos, -speed, 'r') && !doesCollide(stationary))
            {
                charPos.X += speed;
                camera.moveCamera('r');
            }
            if (doesCollide(stationary))
            {
                stop(stationary, 'r');
                camera.moveCamera('l');
            }
        }
        public void up()
        {
            if (!doesCollide(mainFrame, charPos, speed, 'u') && !doesCollide(stationary))
            {
                charPos.Y -= speed;
                camera.moveCamera('u');
            }
            if (doesCollide(stationary))
            {
                stop(stationary, 'u');
                camera.moveCamera('d');
            }
        }
        public void down()
        {
            if (!doesCollide(mainFrame, charPos, -speed, 'd') && !doesCollide(stationary))
            {
                charPos.Y += speed;
                camera.moveCamera('d');
            }
            if (doesCollide(stationary))
            {
                stop(stationary, 'd');
                camera.moveCamera('u');
            }
        }

        public Boolean doesCollide(Rectangle obj1, Rectangle obj2, int speed, char direction)
        {
            Boolean collides = false;

            if ((obj1.X + speed > obj2.X) && direction == 'l')
            {
                collides = true;
            }
            else if ((obj1.X + obj1.Width + speed < obj2.X + obj2.Width) && direction == 'r')
            {
                collides = true;
            }
            else if ((obj1.Y + speed > obj2.Y) && direction == 'u')
            {
                collides = true;
            }
            else if ((obj1.Y + obj1.Height + speed < obj2.Y + obj2.Height) && direction == 'd')
            {
                collides = true;
            }

            return collides;
        }

        public Boolean doesCollide(Rectangle obj1)
        {
            Boolean collides = false;

            if(charPos.Intersects(obj1))
            {
                collides = true;
            }

            return collides;
        }

        public void stop(Rectangle obj1, Char d)
        {
            switch (d)
            {
                case 'u': charPos.Y = obj1.Y + obj1.Height + 1; break;
                case 'd': charPos.Y = obj1.Y - charPos.Height - 1; break;
                case 'r': charPos.X = obj1.X - charPos.Width - 1; break;
                case 'l': charPos.X = obj1.X + obj1.Width + 1; break;
            }
        }

    }
}
