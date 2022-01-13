using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModdingToolkit.AssetsExtractor
{
    public class Main : Game
    {
        private const int Width = 1920, Height = 1080;
        private Texture2D _texture;

        public Main()
        {
            Instance = this;

            Graphics = new(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new(GraphicsDevice);

            base.LoadContent();
        }
        
        public void Extract(string path, FileInfo target)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

            _texture = Content.Load<Texture2D>(filePath);
            _texture.Save(target.FullName, ImageFileFormat.Tga);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color background = new Color(0.5f, 0.5f, 0.5f);

            if (_texture != null)
            {
                Graphics.GraphicsDevice.Clear(background);

                SpriteBatch.Begin();
                SpriteBatch.Draw(_texture, new Vector2(GraphicsDevice.Viewport.Width / 2 - _texture.Width / 2, GraphicsDevice.Viewport.Height / 2 - _texture.Height / 2), Color.White);
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch SpriteBatch { get; private set; }

        public static Main Instance { get; private set; }
    }
}
