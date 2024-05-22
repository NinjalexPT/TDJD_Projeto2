using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TDJD_Projeto2.Scripts.Scenes;
using TDJD_Projeto2.Scripts.Utils;

namespace TDJD_Projeto2
{
    /// <summary>
    /// Jogo inspirado no jogo: https://github.com/MonoGame/MonoGame.Samples
    /// </summary>
    public class Game1 : Game
    {
        #region Campos e propriedades

        // recursos padrão do monogame para criar os gráficos do jogo
        private GraphicsDeviceManager graphics;

        // variáveis globais
        public static int _screenWidth = 1024;
        public static int _screenHeight = 640;
        public static Point _screenCenter = new Point(_screenWidth / 2, _screenHeight / 2);

        public static GraphicsDevice _graphicsDevice;
        public static ContentManager _content;
        public static SpriteBatch _spriteBatch;
        public static GameTime _gameTime;

        // estado da cena atual
        public static SceneType _currentSceneType;
        public static Scene _currentScene;

        #endregion


        #region Carregar jogo

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;

            _content = Content;
            _content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = _screenWidth;
            graphics.PreferredBackBufferHeight = _screenHeight;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // permite que sejam desenhadas texturas no ecrã
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _graphicsDevice = GraphicsDevice;

            // primeira cena aparecer
            _currentSceneType = SceneType.MainMenu;
            _currentScene = new MainMenu(this);
        }

        #endregion


        #region Atualizar jogo

        protected override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _currentScene.Update();

            base.Update(_gameTime);
        }

        #endregion


        #region Desenhar jogo

        protected override void Draw(GameTime gameTime)
        {
            // cor padrão do ecrã
            GraphicsDevice.Clear(Color.Black);

            _gameTime = gameTime;
            _currentScene.Draw();

            base.Draw(_gameTime);
        }

        #endregion
    }
}