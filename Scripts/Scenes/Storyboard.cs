using Microsoft.Xna.Framework.Input;
using TDJD_Projeto2.Scripts.Managers;
using TDJD_Projeto2.Scripts.UI;
using TDJD_Projeto2.Scripts.Utils;
using System.Collections.Generic;

namespace TDJD_Projeto2.Scripts.Scenes
{
    /// <summary>
    /// Representa uma storyboard que aparece no início e fim do jogo
    /// </summary>
    public class Storyboard : Scene
    {
        #region Campos e propriedades

        // imagem
        private List<Image> storyboards;
        private int currentIndex;

        // teclado
        private KeyboardManager keyboardManager;

        // próxima cena (depois de completar todas as storyboards)
        private SceneType nextSceneType;
        private Scene nextScene;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi uma nova storyboard com uma ou várias imagens
        /// </summary>
        public Storyboard(Game1 game, List<Image> storyboards, SceneType nextSceneType, Scene nextScene)
            : base(game)
        {
            this.storyboards = storyboards;
            this.nextSceneType = nextSceneType;
            this.nextScene = nextScene;

            currentIndex = 0;
            keyboardManager = new KeyboardManager();
        }

        public override void LoadContent() { }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza a storyboard
        /// </summary>
        public override void Update()
        {
            UpdateKeyboard();

            if (keyboardManager.IsKeyPressed(Keys.Space))
            {
                // se última storyboard está a ser mostrada
                if (currentIndex == storyboards.Count - 1)
                {
                    SetCurrentScene();
                }
                else // passa para a próxima storyboard
                {
                    currentIndex++;
                }
            }
        }

        /// <summary>
        /// Atualiza o teclado, se houver algum input
        /// </summary>
        private void UpdateKeyboard()
        {
            keyboardManager.Update();
        }

        /// <summary>
        /// Passa para a próxima cena, caso tenha terminado todas as imagens da storyboard
        /// </summary>
        private void SetCurrentScene()
        {
            Game1._currentSceneType = nextSceneType;
            Game1._currentScene = nextScene;
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenhar a storyboard
        /// </summary>
        public override void Draw()
        {
            Game1._spriteBatch.Begin();

            storyboards[currentIndex].Draw();

            Game1._spriteBatch.End();
        }

        #endregion
    }
}