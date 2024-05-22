using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TDJD_Projeto2.Scripts.Scenes;
using TDJD_Projeto2.Scripts.Sprites;
using TDJD_Projeto2.Scripts.UI;
using TDJD_Projeto2.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace TDJD_Projeto2.Scripts.Managers
{
    /// <summary>
    /// Representa um nível do jogo
    /// </summary>
    public class Level
    {
        #region Campos e Propriedes

        // fundos
        private List<Image> backgrounds;

        // jogador principal (heroi)
        private Player player;
        public Player Player
        {
            get => player;
        }

        // inimigos e poções
        private List<Enemy> enemies = new List<Enemy>();
        private List<Potion> potions = new List<Potion>();

        // mapa do nível
        private Tilemap tilemap;
        public Tilemap Tilemap
        {
            get => tilemap;
        }

        // posições inicial e final
        private Vector2 startPosition;
        private Vector2 endPosition;

        // se o nível está completo ou não
        private bool completedLevel;
        public bool CompletedLevel
        {
            get => completedLevel;
        }

        // som quando completa o nível
        private SoundEffect completedLevelSound;

        // tempo máximo do nível
        private TimeSpan fullTime;

        // tempo atual
        private TimeSpan currentTime;
        public TimeSpan CurrentTime
        {
            get => currentTime;
            set => currentTime = value;
        }

        // pontuação do respetivo nível
        private int score;
        public int Score
        {
            get => score;
        }

        // se o nível está em pausa ou não
        private bool levelFreezed = false;
        public bool LevelFreezed
        {
            get => levelFreezed;
            set => levelFreezed = value;
        }

        #endregion


        #region Carregar nível

        /// <summary>
        /// Constroi o nível atual
        /// </summary>
        public Level(Stream fileStream, int levelIndex, int seconds, int currentScore)
        {
            SetInitialTime(seconds);
            SetCurrentScore(currentScore);
            LoadContent(fileStream, levelIndex);
        }

        /// <summary>
        /// Define o tempo (em segundos) para o atual nível
        /// </summary>
        private void SetInitialTime(int seconds)
        {
            fullTime = TimeSpan.FromSeconds(seconds);
            currentTime = fullTime;
        }

        /// <summary>
        /// Define o atual a pontuação para o nível atual
        /// </summary>
        private void SetCurrentScore(int currentScore)
        {
            score = currentScore;
        }

        /// <summary>
        /// Carrega o conteúdo para o nível
        /// </summary>
        private void LoadContent(Stream fileStream, int levelIndex)
        {
            LoadSounds();
            LoadTilemap(fileStream);
            LoadBackgrounds(levelIndex);
        }

        /// <summary>
        /// Carrega o som de completar o nível
        /// </summary>
        private void LoadSounds()
        {
            completedLevelSound = Game1._content.Load<SoundEffect>("Sounds/win");
        }

        /// <summary>
        /// Carrega o mapa do jogo
        /// </summary>
        private void LoadTilemap(Stream fileStream)
        {
            tilemap = new Tilemap(this, fileStream);
        }

        /// <summary>
        /// Carrega todos os fundos necessãrios
        /// </summary>
        private void LoadBackgrounds(int levelIndex)
        {
            int numberOfBackgrounds = tilemap.Width / Tile.WIDTH;
            backgrounds = new List<Image>();

            for (int i = 0; i < numberOfBackgrounds; i++)
            {
                backgrounds.Add(new Image(
                    Game1._content.Load<Texture2D>("Backgrounds/lvl" + levelIndex),
                    new Vector2(i * Game1._screenWidth, 0),
                    0.99f));
            }
        }

        /// <summary>
        /// Cria o jogador (heroi)
        /// </summary>
        public void CreatePlayer(Rectangle tileCollider)
        {
            startPosition = RectangleHelper.GetBottomCenter(tileCollider);
            player = new Player(this, startPosition);
        }

        /// <summary>
        /// Cria um inimigo
        /// </summary>
        public void CreateEnemy(Rectangle tileCollider, string spriteFolder)
        {
            Vector2 position = RectangleHelper.GetBottomCenter(tileCollider);
            enemies.Add(new Enemy(this, position, spriteFolder));
        }

        /// <summary>
        /// Cria uma poção
        /// </summary>
        public void CreatePotion(Rectangle tileCollider, string filename)
        {
            potions.Add(new Potion(this, tileCollider, filename));
        }

        /// <summary>
        /// Cria a meta do jogo
        /// </summary>
        public void CreateExit(Rectangle tileCollider)
        {
            endPosition = RectangleHelper.GetOrigin(tileCollider);
        }

        #endregion


        #region Atualizar nível

        /// <summary>
        /// Atualiza nível atual
        /// </summary>
        public void Update()
        {
            // serve para quando o nível estiver congelado, voltar a permitir jogar,
            // o nível fica congelado, caso o jogador esteja morto, ou perdeu por tempo ou completo o n+ivel
            // (por outras palavras o nível está congelado quando aparece uma popup) 
            if (Gameplay._keyboardManager.IsKeyPressed(Keys.Space))
            {
                levelFreezed = false;
            }

            if (!levelFreezed)
            {
                // se o jogador está vivo e ainda não completou o nível e ainda tem tempo
                if (player.IsAlive && !completedLevel && CurrentTime >= TimeSpan.Zero)
                {
                    DecrementTime();
                    UpdatePlayer();
                    UpdatePotions();
                    UpdateEnemies();

                    // se o jogador está a tocar no tile de fim do nível (no centro do limite inferior)
                    if (Player.IsOnGround && Player.Collider.Contains(endPosition))
                    {
                        CompleteLevel();
                    }
                }

                // se terminou o nível
                else if (completedLevel)
                {
                    CurrentTime = TimeSpan.Zero;
                    levelFreezed = true;
                }
                // se morreu ou ficou sem tempo
                else
                {
                    CurrentTime = TimeSpan.Zero;
                    levelFreezed = true;
                }
            }
        }

        /// <summary>
        /// Faz o tempo decrescer
        /// </summary>
        private void DecrementTime()
        {
            currentTime -= Game1._gameTime.ElapsedGameTime;
        }

        /// <summary>
        /// Atualiza o jogador (movimento, colisões e animações)
        /// </summary>
        private void UpdatePlayer()
        {
            Player.Update();
        }

        /// <summary>
        /// Atualiza os inimigos, verificar se o jogador tocou em algum inimigo
        /// </summary>
        private void UpdateEnemies()
        {
            foreach (Enemy enemy in enemies)
            {
                // se o jogador tocar num inimigo
                if (enemy.Collider.Intersects(Player.Collider))
                {
                    player.OnPlayerDied(enemy);
                }
            }
        }

        /// <summary>
        /// Atualiza as poções, para coletá-las
        /// </summary>
        private void UpdatePotions()
        {
            for (int i = 0; i < potions.Count; ++i)
            {
                Potion potion = potions[i];

                // se tocar numa poção
                if (potion.Collider.Intersects(Player.Collider))
                {
                    score += 1;
                    potion.OnPotionCollected();
                    potions.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// Quando o jogador completa o nível
        /// </summary>
        private void CompleteLevel()
        {
            completedLevel = true;
            completedLevelSound.Play();
            player.OnPlayerCompletedLevel();
        }

        /// <summary>
        /// Inicia um nova vida, para o utilizador tentar o nível novamente no ponto de partida
        /// </summary>
        public void StartNewLife()
        {
            currentTime = fullTime;
            Player.ResetPlayer(startPosition);
        }

        #endregion


        #region Desenhar nível

        /// <summary>
        /// Desenha os objetos do nível
        /// </summary>
        public void Draw()
        {
            DrawBackground();
            DrawTilemap();
            DrawPlayer();
            DrawPotions();
            DrawEnemies();
        }

        /// <summary>
        /// Desenha o fundo do nível
        /// </summary>
        private void DrawBackground()
        {
            foreach (Image background in backgrounds)
            {
                background.Draw();
            }
        }

        /// <summary>
        /// Desenha o mapa
        /// </summary>
        private void DrawTilemap()
        {
            tilemap.Draw();
        }

        /// <summary>
        /// Desenha o jogador
        /// </summary>
        private void DrawPlayer()
        {
            player.Draw();
        }

        /// <summary>
        /// Desenha as poções
        /// </summary>
        private void DrawPotions()
        {
            foreach (Potion potion in potions)
            {
                potion.Draw();
            }
        }

        /// <summary>
        /// Desenha os inimigos
        /// </summary>
        private void DrawEnemies()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }
        }

        #endregion
    }
}