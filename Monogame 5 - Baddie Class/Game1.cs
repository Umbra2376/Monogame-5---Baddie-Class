using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monogame_5___Baddie_Class
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<Texture2D> ghostTextures;
        List<Ghost> ghosts;
        Texture2D titleScreen, houseScreen, endScreen, mario;
        Random generator = new Random();
        MouseState mouseState = new MouseState();
        KeyboardState keyboardState = new KeyboardState();
        enum Screen
        {
            Title, House, End
        }
        Screen screen;
        Rectangle window;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false;
            ghostTextures = new List<Texture2D>();
            ghosts = new List<Ghost>();
            window = new Rectangle(0, 0, 800, 600);
            screen = Screen.Title;
            base.Initialize();
            mouseState = Mouse.GetState();
            for (int i = 0; i <= 20; i++)
            {
                Ghost temp = new Ghost(ghostTextures, new Rectangle(generator.Next(window.Width - 40), generator.Next(window.Height - 40), 40, 40));
                while (temp.Contains(mouseState.Position))
                    temp = new Ghost(ghostTextures, new Rectangle(generator.Next(window.Width - 40), generator.Next(window.Height - 40), 40, 40));

                ghosts.Add(temp);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ghostTextures.Add(Content.Load<Texture2D>("boo-stopped"));
            for (int i = 1;  i <= 8; i++)
            {
                ghostTextures.Add(Content.Load<Texture2D>("boo-move-" + i));
            }
            titleScreen = Content.Load<Texture2D>("haunted-title");
            houseScreen = Content.Load<Texture2D>("haunted-background");
            endScreen = Content.Load<Texture2D>("haunted-end-screen");
            mario = Content.Load<Texture2D>("mario");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            if (screen == Screen.Title)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                    screen = Screen.House;

            }
            else if (screen == Screen.House)
            {
                foreach (Ghost ghost in ghosts)
                {
                    ghost.Update(gameTime, mouseState);
                    if (ghost.Contains(mouseState.Position))
                        screen = Screen.End;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.Title)
                _spriteBatch.Draw(titleScreen, window, Color.White);
            else if (screen == Screen.House)
            {
                _spriteBatch.Draw(houseScreen, window, Color.White);
                _spriteBatch.Draw(mario, mouseState.Position.ToVector2(), Color.White);
                foreach (Ghost ghost in ghosts)
                    ghost.Draw(_spriteBatch);
            }
            else
                _spriteBatch.Draw(endScreen, window, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
