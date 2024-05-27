using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDJD_Projeto2.Scripts.Effects;
using TDJD_Projeto2.Scripts.Managers;
using System;
using TDJD_Projeto2.Scripts.Utils;

namespace TDJD_Projeto2.Scripts.Sprites
{
    /// <summary>
    /// Representa um inimigo animado
    /// </summary>
    public class Enemy
    {
        #region Campos e propriedades

        // nível atual
        private Level level;
        public Level Level
        {
            get => level;
        }
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        // movimento do jogador
        private SpriteEffects flip = SpriteEffects.None;
        public float speed = 6000f;
        
        private bool isOnGround;
        public bool IsOnGround
        {
            get => isOnGround;
        }
        private float previousBottom;
        // animações
        private Animation idleAnimation;
        private Animator animator;
        
        // posição do inimigo para o centro inferior
        private Vector2 position;
        public Vector2 Position
        {
            get => position;
        }

        // limites (bordas) da textura
        private Rectangle textureBounds;
        // obtém o retângulo colisor do inimigo através dos limites da textura
        public Rectangle Collider
        {
            get
            {
                int left = (int)Math.Round(Position.X - animator.Origin.X) + textureBounds.X;
                int top = (int)Math.Round(Position.Y - animator.Origin.Y) + textureBounds.Y;
                int right = textureBounds.Width;
                int bottom = textureBounds.Height;

                return new Rectangle(left, top, right, bottom);
            }
        }

        public void ApplyPhysics()
        {
            float elapsedTime = (float)Game1._gameTime.ElapsedGameTime.TotalSeconds;

            // aplica o movimento para a direita ou esquerda
            velocity.X = speed * elapsedTime;

           

            // atualiza a posição do inimigo
            position += velocity * elapsedTime;
            position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
        }

        private void HandleTilemapCollisions()
        {
            // obtém o colisor na posição atual do inimigo
            Rectangle bounds = Collider;

            // obtém os tiles que se encontram na vizinhança da posição atual do inimigo
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
                                // se o inimigo está no topo do tile, está no chão
                                if (previousBottom <= tileBounds.Top)
                                {
                                    isOnGround = true;
                                }

                                // se é o tile é colisível e o inimigo está no chão
                                if (type == CollisionType.block || isOnGround)
                                {
                                    // resolve a colisão ao longo do eixo Y (atualiza a posição do inimigo)
                                    position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // atualiza o colisor do inimigo com os novos limites
                                    bounds = Collider;
                                }
                            }
                            else if (type == CollisionType.block)
                            {
                                // resolve a colisão ao longo do eixo X (atualiza a posição do inimigo)
                                position = new Vector2(Position.X + depth.X, Position.Y);

                                // atualiza o colisor do inimigo com os novos limites
                                bounds = Collider;
                            }
                        }
                    }
                }
            }

            // atualiza a posição inferior que o inimigo está no momento
            previousBottom = bounds.Bottom;
        }

        /// <summary>
        /// Pára de correr quando colide
        /// </summary>
        private void ResetVelocityIfCollide(Vector2 previousPosition)
        {
            if (Position.X == previousPosition.X)
            {
                speed = -speed;
            }
            
        }


        #endregion


        #region Carregar

        /// <summary>
        /// Constroi um novo inimigo
        /// </summary>
        public Enemy(Level level, Vector2 position, string spriteFolder)
        {
            this.level = level;
            this.position = position;

            LoadContent(spriteFolder);

            // inicia animação
            animator = new Animator();
            animator.PlayAnimation(idleAnimation);

            // calcula os limites da textura
            SetTextureBounds();
        }

        /// <summary>
        /// Carrega o conteúdo para o inimigo
        /// </summary>
        public void LoadContent(string spriteFolder)
        {
            // pasta das sprite sheets com as animações
            spriteFolder = "Sprites/" + spriteFolder + "/";

            // animações
            idleAnimation = new Animation(Game1._content.Load<Texture2D>(spriteFolder + "Idle"), 0.15f, true);
        }

        /// <summary>
        /// Calcula os limites (bordas) da textura
        /// </summary>
        private void SetTextureBounds()
        {
            int width = (int)(idleAnimation.FrameWidth * 0.35);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.7);
            int top = idleAnimation.FrameHeight - height;
            textureBounds = new Rectangle(left, top, width, height);
        }



        #endregion


        public void Update()
        {
            

            Vector2 previousPosition = Position;

            ApplyPhysics();

            HandleTilemapCollisions();

            ResetVelocityIfCollide(previousPosition);

            
        }


        #region Desenhar

        /// <summary>
        /// Desenha o inimigo animado
        /// </summary>
        /// 
        public void FlipEnemy()
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
        public void Draw()
        {
            FlipEnemy();
            animator.Draw(Position, flip);
        }

        #endregion
    }
}