using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TDJD_Projeto2.Scripts.Effects
{
    /// <summary>
    /// Representa o animador de uma animação (sprite sheet animado)
    /// </summary>
    public class Animator
    {
        #region Campos e Propriedes

        // a animação que está atualmente a ser reproduzida
        private Animation animation;
        public Animation Animation
        {
            get => animation;
        }

        // é o índice do frame atual da animação
        private int frameIndex;
        public int FrameIndex
        {
            get => frameIndex;
        }

        // a quantidade de tempo (em segundos) que a atual frame foi mostrada
        private float frameTime;

        // Gets a texture origin at the bottom center of each frame
        public Vector2 Origin
        {
            get => new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight);
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Inicia ou continua a reprodução de uma animação
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            // se animação ja estiver em execução, retorna para continuar a reproduzi-la
            if (Animation == animation)
            {
                return;
            }

            this.animation = animation;
            frameIndex = 0;
            frameTime = 0.0f;
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha a frame atual da animação
        /// </summary>
        public void Draw(Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
            {
                throw new NotSupportedException("Erro: Nenhuma animação encontrada.");
            }

            frameTime += (float)Game1._gameTime.ElapsedGameTime.TotalSeconds;

            while (frameTime > Animation.TimeBetweenEachFrame)
            {
                frameTime -= Animation.TimeBetweenEachFrame;

                // avança o índice do frame
                if (Animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.NumberOfFrames;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.NumberOfFrames - 1);
                }
            }

            // calcula o retângulo de origem do frame atual
            Rectangle source = new Rectangle(FrameIndex * Animation.SpriteSheet.Height, 0, Animation.SpriteSheet.Height, Animation.SpriteSheet.Height);

            Game1._spriteBatch.Draw(Animation.SpriteSheet, position, source, Color.White, 0f, Origin, 1f, spriteEffects, 1f);
        }

        #endregion
    }
}