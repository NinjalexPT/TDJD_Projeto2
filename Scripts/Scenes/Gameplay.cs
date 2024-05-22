using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TDJD_Projeto2.Scripts.Managers;
using TDJD_Projeto2.Scripts.Sprites;
using TDJD_Projeto2.Scripts.UI;
using TDJD_Projeto2.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace TDJD_Projeto2.Scripts.Scenes
{
    /// <summary>
    /// Representa a cena do jogo em si
    /// </summary>
    public class Gameplay : Scene
    {
        #region Campos e propriedades

        // teclado do jogo
        public static KeyboardManager _keyboardManager;

        // estado do nível atual do jogo
        private Level level;
        private const int numberOfLevels = 3;
        private int levelIndex = -1;
        private bool wasPlaying;

        // camara do jogo
        private static Camera camera;

        // guardar a pontuação atual do nível atual (é progressiva, ou seja permanece de nível para nível)
        private int currentScore;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi a cena do jogo em si
        /// </summary>
        public Gameplay(Game1 game)
            : base(game)
        {
            LoadKeyboard();
            LoadContent();
        }

        /// <summary>
        /// Carrega o teclado
        /// </summary>
        private void LoadKeyboard()
        {
            _keyboardManager = new KeyboardManager();
        }

        /// <summary>
        /// Carrega o conteúdo da cena
        /// </summary>
        public override void LoadContent()
        {
            LoadCamera();
            LoadNextLevel();
        }

        /// <summary>
        /// Carrega a câmera
        /// </summary>
        private void LoadCamera()
        {
            camera = new Camera(Game1._graphicsDevice.Viewport);
        }

        /// <summary>
        /// Carrega o próximo nível
        /// </summary>
        private void LoadNextLevel()
        {
            // se concluiu todos os níveis
            if (levelIndex == numberOfLevels - 1)
            {
                List<Image> storyboards = new List<Image>()
                {
                    new Image(Game1._content.Load<Texture2D>("Storyboards/storyend1"), new Vector2(0, 0)),
                    new Image(Game1._content.Load<Texture2D>("Storyboards/storyend2"), new Vector2(0, 0)),
                    new Image(Game1._content.Load<Texture2D>("Storyboards/storyend3"), new Vector2(0, 0)),
                };

                SceneType nextSceneType = SceneType.MainMenu;
                Scene nextScene = new MainMenu(game);

                Game1._currentSceneType = SceneType.Storyboard;
                Game1._currentScene = new Storyboard(game, storyboards, nextSceneType, nextScene);

                levelIndex = -1;
                currentScore = 0;
            }

            // índice do próximo nível
            levelIndex += 1;
            string levelPath = "Content/Levels/lvl" + levelIndex + ".txt";

            if (level != null)
            {
                currentScore = level.Score;
            }

            // carrega o nivel
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
            {
                if (levelIndex == 0)
                {
                    level = new Level(fileStream, levelIndex, 120, currentScore);
                }
                else if (levelIndex == 1)
                {
                    level = new Level(fileStream, levelIndex, 180, currentScore);
                }
                else if (levelIndex == 2)
                {
                    level = new Level(fileStream, levelIndex, 240, currentScore);
                }
            }
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza o estado da cena do jogo
        /// </summary>
        public override void Update()
        {
            UpdateKeyboard();

            bool isPlaying = _keyboardManager.IsKeyPressed(Keys.Space);

            if (!wasPlaying && isPlaying)
            {
                if (!level.Player.IsAlive)
                {
                    level.StartNewLife();
                }
                else if (level.CurrentTime == TimeSpan.Zero)
                {
                    if (level.CompletedLevel)
                    {
                        LoadNextLevel();
                    }
                }
            }
            wasPlaying = isPlaying;

            UpdateLevel();
            UpdateCamera();

            if (levelIndex - 1 == numberOfLevels)
            {
                Game1._currentSceneType = SceneType.MainMenu;
                Game1._currentScene = new MainMenu(game);
            }
        }

        /// <summary>
        /// Atualiza o teclado
        /// </summary>
        private void UpdateKeyboard()
        {
            _keyboardManager.Update();
        }

        /// <summary>
        /// Atualiza os objetos do nível
        /// </summary>
        private void UpdateLevel()
        {
            level.Update();
        }

        /// <summary>
        /// Atualiza a posição da câmera através do movimento do jogador
        /// </summary>
        private void UpdateCamera()
        {
            camera.Update(level.Player.Position, (int)level.Player.Position.X * Tile.WIDTH, 640);
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha a cena do jogo
        /// </summary>
        public override void Draw()
        {
            Game1._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            DrawLevel();
            DrawLabels();
            DrawPopups();

            Game1._spriteBatch.End();
        }

        /// <summary>
        /// Desenha os objetos do nível
        /// </summary>
        private void DrawLevel()
        {
            level.Draw();
        }

        /// <summary>
        /// Desenha o texto da pontuação e tempo
        /// </summary>
        private void DrawLabels()
        {
            string timeText = "TIME: " +
                level.CurrentTime.Minutes.ToString("00") + ":" +
                level.CurrentTime.Seconds.ToString("00");

            string scoreText = "SCORE: " +
                level.Score.ToString();

            int xPosition = (int)camera.Center.X - 500;//1120 1024 464

            Label labelTime = new Label("Fonts/Hud", timeText, new Vector2(xPosition, 0f), Color.Yellow);
            labelTime.Draw();

            Label labelScore = new Label("Fonts/Hud", scoreText, new Vector2(xPosition, 25f), Color.Yellow);
            labelScore.Draw();
        }

        /// <summary>
        /// Desenha as popups que aparecem quando o jogador morre, ganha ou perde por tempo
        /// </summary>
        private void DrawPopups()
        {
            Popup currentPopup = null;
            Vector2 screenCenter = new Vector2((int)camera.Center.X, (int)camera.Center.Y);

            if (level.CompletedLevel)
            {
                if (level.CompletedLevel)
                {
                    currentPopup = new Popup("Popups/youwin", screenCenter);
                }
            }
            else if (!level.Player.IsAlive)
            {
                currentPopup = new Popup("Popups/youaredead", screenCenter);
            }
            else if (level.CurrentTime <= TimeSpan.Zero)
            {
                currentPopup = new Popup("Popups/youlose", screenCenter);
            }

            if (currentPopup != null)
            {
                currentPopup.Draw();
            }
        }

        #endregion
    }
}