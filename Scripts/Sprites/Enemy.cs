using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDJD_Projeto2.Scripts.Effects;
using TDJD_Projeto2.Scripts.Managers;
using System;

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


        #region Desenhar

        /// <summary>
        /// Desenha o inimigo animado
        /// </summary>
        public void Draw()
        {
            animator.Draw(Position, SpriteEffects.None);
        }

        #endregion
    }
}