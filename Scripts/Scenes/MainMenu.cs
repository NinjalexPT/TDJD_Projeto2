using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TDJD_Projeto2.Scripts.UI;
using TDJD_Projeto2.Scripts.Utils;
using System;
using System.Collections.Generic;

namespace TDJD_Projeto2.Scripts.Scenes
{
    /// <summary>
    /// Representa a cena do menu principal
    /// </summary>
    public class MainMenu : Scene
    {
        #region Campos e propriedades

        // logótipo
        private Texture2D logo;
        
        // botões
        private List<Button> buttons;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi a cena do menu principal
        /// </summary>
        public MainMenu(Game1 game)
            : base(game)
        {
            LoadContent();
        }

        /// <summary>
        /// Carrega o conteúdo para a cena do menu principal
        /// </summary>
        public override void LoadContent()
        {
            PlayBackgroundSong();
            LoadLogo();
            LoadButtons();
        }

        /// <summary>
        /// Tocar música de fundo do jogo
        /// </summary>
        private void PlayBackgroundSong()
        {
            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Game1._content.Load<Song>("Sounds/theme"));
            }
            catch
            {
                throw new NotSupportedException("Erro: Impossível carregar música do jogo.");
            }
        }

        /// <summary>
        /// Carregar o logótipo
        /// </summary>
        private void LoadLogo()
        {
            logo = Game1._content.Load<Texture2D>("Logos/logomenu");
        }

        /// <summary>
        /// Carregar os botões do menu
        /// </summary>
        private void LoadButtons()
        {
            Texture2D creditsButton_texture = Game1._content.Load<Texture2D>("Buttons/credits");
            Texture2D playGameButton_texture = Game1._content.Load<Texture2D>("Buttons/play");
            Texture2D quitGameButton_texture = Game1._content.Load<Texture2D>("Buttons/exit");

            SpriteFont buttonFont_normal = Game1._content.Load<SpriteFont>("Fonts/charybdis_normal");
            SpriteFont buttonFont_big = Game1._content.Load<SpriteFont>("Fonts/charybdis_big");

            Button creditsButton = new Button(creditsButton_texture, null, 250, 150)
            {
                Position = new Vector2(Game1._screenCenter.X - playGameButton_texture.Width / 2 - 320, Game1._screenCenter.Y + 95),
            };
            creditsButton.Click += CreditsButton_Click;

            Button playGameButton = new Button(playGameButton_texture, null, 255, 155)
            {
                Position = new Vector2(Game1._screenCenter.X - playGameButton_texture.Width / 2 + 20, Game1._screenCenter.Y + 130),
            };
            playGameButton.Click += PlayGameButton_Click;

            Button quitGameButton = new Button(quitGameButton_texture, null, 235, 135)
            {
                Position = new Vector2(Game1._screenCenter.X - playGameButton_texture.Width / 2 + 340, Game1._screenCenter.Y + 95),
            };
            quitGameButton.Click += QuitGameButton_Click;

            buttons = new List<Button>()
            {
                playGameButton,
                creditsButton,
                quitGameButton,
            };
        }

        #endregion


        #region Eventos de input (mouse)

        /// <summary>
        /// Quando o utilizador clica no botão dos créditos
        /// </summary>
        private void CreditsButton_Click(object sender, EventArgs e)
        {
            string text = "OMG Zombies\n\n" +
                "Jogo produzido no âmbito da unidade curricular\n" +
                "'Técnicas de Desenvolvimento de Vídeojogos',\n" +
                "realizado no Instituto Politécnico do Cávado e do Ave.\n\n" +
                "Desenvolvedores:\n" +
                "- Ademar Valente\n" +
                "- Luís Pereira\n\n" +
                "Barcelos, Maio 2022\n\n" +
                "© Todos os direitos reservados.";

            Game1._currentSceneType = SceneType.Credits;
            Game1._currentScene = new Credits(game, text);
        }

        /// </summary>
        /// Quando o utilizador clica no botão de jogar
        /// </summary>
        private void PlayGameButton_Click(object sender, EventArgs e)
        {
            List<Image> storyboards = new List<Image>()
            {
                new Image(Game1._content.Load<Texture2D>("Storyboards/storystart1"), new Vector2(0, 0)),
                new Image(Game1._content.Load<Texture2D>("Storyboards/storystart2"), new Vector2(0, 0)),
                new Image(Game1._content.Load<Texture2D>("Storyboards/storystart3"), new Vector2(0, 0)),
                new Image(Game1._content.Load<Texture2D>("Storyboards/storystart4"), new Vector2(0, 0))
            };

            SceneType nextSceneType = SceneType.Gameplay;
            Scene nextScene = new Gameplay(game);

            Game1._currentSceneType = SceneType.Storyboard;
            Game1._currentScene = new Storyboard(game, storyboards, nextSceneType, nextScene);
        }

        /// </summary>
        /// Quando o utilizador clica no botão de sair
        /// </summary>
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza o menu principal, ao clicar num botão
        /// </summary>
        public override void Update()
        {
            foreach (Button button in buttons)
            {
                button.Update();
            }
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha o menu principal
        /// </summary>
        public override void Draw()
        {
            Game1._spriteBatch.Begin();

            DrawLogo();
            DrawButtons();

            Game1._spriteBatch.End();
        }

        /// <summary>
        /// Desenha o logótipo
        /// </summary>
        private void DrawLogo()
        {
            Game1._spriteBatch.Draw(logo, new Vector2(Game1._screenCenter.X - logo.Width / 2, Game1._screenCenter.Y - 285), Color.White);
        }

        /// <summary>
        /// Desenha os botões
        /// </summary>
        private void DrawButtons()
        {
            foreach (Button button in buttons)
            {
                button.Draw();
            }
        }

        #endregion
    }
}