using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TDJD_Projeto2.Scripts.Managers;
using TDJD_Projeto2.Scripts.UI;
using TDJD_Projeto2.Scripts.Utils;

namespace TDJD_Projeto2.Scripts.Scenes
{
    /// <summary>
    /// Representa a cena dos créditos
    /// </summary>
    public class Credits : Scene
    {
        #region Campos e propriedades

        // texto
        private string text;
        private Label label;

        // teclado
        private KeyboardManager keyboardManager;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi a cena dos créditos
        /// </summary>
        public Credits(Game1 game, string text)
            : base(game)
        {
            this.text = text;
            LoadContent();
            LoadKeyboard();
        }

        /// <summary>
        /// Carrega o conteúdo para a cena
        /// </summary>
        public override void LoadContent()
        {
            LoadLabel();
        }

        /// <summary>
        /// Carrega o texto da cena
        /// </summary>
        private void LoadLabel()
        {
            Vector2 screenCenter = new Vector2(Game1._screenCenter.X, Game1._screenCenter.Y);

            label = new Label("Fonts/Hud", text, new Vector2(0, 0), Color.White);
            label.SetCenterTextInScreen(screenCenter);
        }

        /// <summary>
        /// Carrega o teclado
        /// </summary>
        private void LoadKeyboard()
        {
            keyboardManager = new KeyboardManager();
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza a cena dos créditos
        /// </summary>
        public override void Update()
        {
            UpdateKeyboard();

            if (keyboardManager.IsKeyPressed(Keys.Space))
            {
                GoToMenuScene();
            }
        }

        /// <summary>
        /// Atualiza o teclado
        /// </summary>
        private void UpdateKeyboard()
        {
            keyboardManager.Update();
        }

        /// <summary>
        /// Sai da cena dos créditos e vai para o menu principal
        /// </summary>
        private void GoToMenuScene()
        {
            Game1._currentSceneType = SceneType.MainMenu;
            Game1._currentScene = new MainMenu(game);
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha a cena dos créditos
        /// </summary>
        public override void Draw()
        {
            Game1._spriteBatch.Begin();

            label.Draw();

            Game1._spriteBatch.End();
        }

        #endregion
    }
}