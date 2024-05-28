using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TDJD_Projeto2.Scripts.Effects;
using TDJD_Projeto2.Scripts.Managers;
using TDJD_Projeto2.Scripts.Scenes;
using TDJD_Projeto2.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TDJD_Projeto2.Scripts.Sprites
{
    /// <summary>
    /// Controla os movimentos e animações do jogador.
    /// </summary>
    public class Player
    {
        #region Campos e propriedes

        public List<Bullet> Bullets => bullets.Where(b => b.IsActive).ToList();
        public bool canshoot { get; set; } = true;
        private Texture2D bulletTexture;
        private List<Bullet> bullets;
        public MouseState currentMouseState;
        public MouseState previousMouseState;

        // o nível atual
        public Level Level
        {
            get => level;
        }
        private Level level;

        // animações
        private Animation idleAnimation;
        public Animation runAnimation;
        private Animation jumpAnimation;
        private Animation deadAnimation;

        // animar animações
        private Animator animator;
        private SpriteEffects flip = SpriteEffects.None;

        // sons
        private SoundEffect runSound;
        private SoundEffectInstance runSoundInstace;
        private SoundEffect jumpSound;
        private SoundEffect dieSound;
        private SoundEffect shootsound;

        // se o jogador está vivo ou não
        private bool isAlive;
        public bool IsAlive
        {
            get => isAlive;
        }

        
        // posição do jogador
        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        // velocidade do jogador para horizontar ou vertical
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        // movimento do jogador
        private float speed;
        public float Speed
        {
            get => speed;
        }

        // estado do salto
        private bool isJumping;
        private float jumpTime;

        // sobre movimento do salto
        private const float GRAVITY = 3400f;
        private const float MAX_JUMP_TIME = 0.15f;
        private const float MAX_JUMP_SPEED = 500f;

        // se o jogador está a tocar no chão ou não
        private bool isOnGround;
        public bool IsOnGround
        {
            get => isOnGround;
        }

        // limites (bordas) da textura
        private Rectangle textureBounds;
        // obtém o retângulo colisor do jogador através dos limites da textura
        public Rectangle Collider
        {
            get
            {
                int left = (int)(Position.X - animator.Origin.X) + textureBounds.X;
                int top = (int)(Position.Y - animator.Origin.Y) + textureBounds.Y;
                int right = textureBounds.Width;
                int bottom = textureBounds.Height;

                return new Rectangle(left, top, right, bottom);
            }
        }

        private float previousBottom;

        private float layer;
        public float Layer
        {
            get => layer;
            set => layer = value;
        }

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi o jogador (heroi) do nível
        /// </summary>
        public Player(Level level, Vector2 position)
        {
            this.level = level;

            LoadContent();
            SetTextureBounds();
            ResetPlayer(position);
        }

        /// <summary>
        /// Carrega o conteúdo do jogador
        /// </summary>
        public void LoadContent()
        {
            // sprite sheets animadas
            idleAnimation = new Animation(Game1._content.Load<Texture2D>("Sprites/Hero/idle"), 0.1f, true);
            runAnimation = new Animation(Game1._content.Load<Texture2D>("Sprites/Hero/run"), 0.1f, true);
            jumpAnimation = new Animation(Game1._content.Load<Texture2D>("Sprites/Hero/jump"), 0.1f, false);
            deadAnimation = new Animation(Game1._content.Load<Texture2D>("Sprites/Hero/dead"), 0.1f, false);

            // sons
            runSound = Game1._content.Load<SoundEffect>("Sounds/run");
            runSoundInstace = runSound.CreateInstance();
            jumpSound = Game1._content.Load<SoundEffect>("Sounds/jump");
            dieSound = Game1._content.Load<SoundEffect>("Sounds/lose");
            shootsound = Game1._content.Load<SoundEffect>("Sounds/shoot");

            // Carregar textura do projétil
            bulletTexture = Game1._content.Load<Texture2D>("Sprites/Bullet");
            bullets = new List<Bullet>();
        }

        /// <summary>
        /// Reseta o jogador para voltar a viver
        /// </summary>
        public void ResetPlayer(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;

            animator = new Animator();
            animator.PlayAnimation(idleAnimation);

            isAlive = true;
        }

        /// <summary>
        /// Calcula os limites (bordas) da textura
        /// </summary>
        private void SetTextureBounds()
        {
            int right = (int)(idleAnimation.FrameWidth * 0.55);
            int left = (idleAnimation.FrameWidth - right) / 2;
            int bottom = (int)(idleAnimation.FrameHeight * 0.8);
            int top = idleAnimation.FrameHeight - bottom;

            textureBounds = new Rectangle(left, top, right, bottom);
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza o jogador
        /// </summary>
        public async void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            PressKey();

            Vector2 previousPosition = Position;

            ApplyPhysics();

            HandleTilemapCollisions();

            ResetVelocityIfCollide(previousPosition);

            if (isAlive && isOnGround)
            {
                if (Velocity.X > 0 || Velocity.X < 0)
                {
                    runSoundInstace.Play();
                    animator.PlayAnimation(runAnimation);
                }
                else
                {
                    
                    runSoundInstace.Stop();
                    
                    animator.PlayAnimation(idleAnimation);
                }
            }

            ResetPhysicsApplied();

            // Verificar clique do rato e disparar
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && canshoot)
            {
                
                Shoot();
            }

            // Atualizar projéteis
            float elapsedTime = (float)Game1._gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var bullet in bullets)
            {
                bullet.Update(elapsedTime);
            }

            // Remover projéteis inativos
            bullets.RemoveAll(b => !b.IsActive);
        }

        private void Shoot()
        {
            // Define a posição inicial e a velocidade da bala
            Vector2 bulletPosition = new Vector2(Position.X + (flip == SpriteEffects.None ? 1 : -1) * 20, Position.Y-58);
            Vector2 bulletVelocity = new Vector2(flip == SpriteEffects.None ? 500 : -500, 0);
            Bullet newBullet = new Bullet(level,bulletPosition, bulletVelocity, bulletTexture);
            shootsound.Play();
            bullets.Add(newBullet);
        }




        /// <summary>
        /// Verifica se pressionou as teclas do movimento ou de salto
        /// </summary>
        private void PressKey()
        {
            // se foi para direita
            if (Gameplay._keyboardManager.isKeyHeld(Keys.Right) ||
                Gameplay._keyboardManager.isKeyHeld(Keys.D))
            {
                speed = 10000f;
            }
            // se foi para esquerda
            else if (Gameplay._keyboardManager.isKeyHeld(Keys.Left) ||
                Gameplay._keyboardManager.isKeyHeld(Keys.A))
            {
                speed = -10000f;
            }

            // se saltou
            if (Gameplay._keyboardManager.isKeyHeld(Keys.Space) ||
                Gameplay._keyboardManager.isKeyHeld(Keys.W))
            {
                isJumping = true;
            }
        }

        /// <summary>
        /// Atualiza a velocidade e posição do jogador,
        /// baseada no tempo e gravidade do salto
        /// </summary>
        public void ApplyPhysics()
        {
            float elapsedTime = (float)Game1._gameTime.ElapsedGameTime.TotalSeconds;

            // aplica o movimento para a direita ou esquerda
            velocity.X = speed * elapsedTime;

            // aplica o movimento para cima
            // e previne que o salto tenha uma velocidade máxima e mínima
            velocity.Y = MathHelper.Clamp(velocity.Y + GRAVITY * elapsedTime, -MAX_JUMP_SPEED, MAX_JUMP_SPEED);
            velocity.Y = Jump(elapsedTime, velocity.Y);

            // atualiza a posição do jogador
            Position += velocity * elapsedTime;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
        }

        /// <summary>
        /// Faz o jogador saltar
        /// </summary>
        private float Jump(float elapsedTime, float velocityY)
        {
            if (isJumping)
            {
                // começa ou continua a saltar
                if (isOnGround || jumpTime > 0f)
                {
                    if (jumpTime == 0f)
                    {
                        jumpSound.Play();
                    }

                    animator.PlayAnimation(jumpAnimation);
                    jumpTime += elapsedTime;
                }

                // se jogador está a subir
                if (jumpTime > 0f && jumpTime <= MAX_JUMP_TIME)
                {
                    velocityY = -700f;
                }
                // senão atingiu o limite de tempo do salto
                else
                {
                    jumpTime = 0f;
                }
            }
            else
            {
                jumpTime = 0f;
            }

            return velocityY;
        }

        /// <summary>
        /// Deteta e resolve todas as colisões com o tilemap,
        /// o jogador é empurrado para um eixo para evitar sobreposição com o tile
        /// </summary>
        private void HandleTilemapCollisions()
        {
            // obtém o colisor na posição atual do jogador
            Rectangle bounds = Collider;

            // obtém os tiles que se encontram na vizinhança da posição atual do jogador
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.WIDTH);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.WIDTH)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.HEIGHT);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.HEIGHT)) - 1;

            // reseta se está no chão para procurar colisões
            isOnGround = false;

            // para cada potencial colisão
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // recebe o tipo do tile atual
                    CollisionType type = Level.Tilemap.GetTileType(x, y);

                    // se o tile atual é colisível
                    if (type != CollisionType.transparent)
                    {
                        // determina a profundidade da colisão entre o colisor do jogador e a colisão do tile (atual que está a ser verificado pelo loop)
                        Rectangle tileBounds = Level.Tilemap.GetTileCollider(x, y);
                        Vector2 depth = RectangleHelper.GetIntersectionDepth(bounds, tileBounds);

                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // resolve a colisão ao longo do eixo
                            if (absDepthY < absDepthX)
                            {
                                // se o jogador está no topo do tile, está no chão
                                if (previousBottom <= tileBounds.Top)
                                {
                                    isOnGround = true;
                                }

                                // se é o tile é colisível e o jogador está no chão
                                if (type == CollisionType.block || isOnGround)
                                {
                                    // resolve a colisão ao longo do eixo Y (atualiza a posição do jogador)
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // atualiza o colisor do jogador com os novos limites
                                    bounds = Collider;
                                }
                            }
                            else if (type == CollisionType.block)
                            {
                                // resolve a colisão ao longo do eixo X (atualiza a posição do jogador)
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                // atualiza o colisor do jogador com os novos limites
                                bounds = Collider;
                            }
                        }
                    }
                }
            }

            // atualiza a posição inferior que o jogador está no momento
            previousBottom = bounds.Bottom;
        }

        /// <summary>
        /// Pára de correr quando colide
        /// </summary>
        private void ResetVelocityIfCollide(Vector2 previousPosition)
        {
            if (Position.X == previousPosition.X)
            {
                velocity.X = 0;
            }
            if (Position.Y == previousPosition.Y)
            {
                velocity.Y = 0;
            }
        }

        /// <summary>
        /// Reseta as físicas aplicadas
        /// </summary>
        private void ResetPhysicsApplied()
        {
            speed = 0f;
            isJumping = false;
        }

        #endregion


        #region Eventos do nível

        /// <summary>
        /// É chamda quando o jogador ficou sem tempo para concluir o nível
        /// </summary>
        public void OnPlayerWithoutTime()
        {
            isAlive = false;
            dieSound.Play();
            animator.PlayAnimation(deadAnimation);

            runSoundInstace.Stop();
        }

        /// <summary>
        /// É chamado quando o jogador morre
        /// </summary>
        public void OnPlayerDied(Enemy killedBy)
        {
            isAlive = false;

            if (killedBy != null)
            {
                dieSound.Play();
            }

            animator.PlayAnimation(deadAnimation);
            runSoundInstace.Stop();
        }

        /// <summary>
        /// É chamado quando o jogador chega à meta (completa o nível)
        /// </summary>
        public void OnPlayerCompletedLevel()
        {
            runSoundInstace.Stop();
        }

        #endregion


        #region Desenhar jogador

        /// <summary>
        /// Desenha o jogador
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            FlipPlayer();
            animator.Draw(Position, flip);

            //Desenhar projeteis
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch, bulletTexture);
            }

        }

        /// <summary>
        /// Vira o jogador de acordo para onde se está a mover (para a esquerda ou para a direita)
        /// </summary>
        public void FlipPlayer()
        {
            if (Velocity.X > 0)
            {
                flip = SpriteEffects.None;
            }
            else if (Velocity.X < 0)
            {
                flip = SpriteEffects.FlipHorizontally;
            }
        }

        #endregion
    }
}