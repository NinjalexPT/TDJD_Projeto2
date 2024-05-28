using Microsoft.Xna.Framework.Graphics;

namespace TDJD_Projeto2.Scripts.Effects
{
    /// <summary>
    /// Representa uma sprite sheet animada
    /// </summary>
    public class Animation
    {
        #region Campos e Propriedes

        // textura da animação completa
        private Texture2D spriteSheet;
        public Texture2D SpriteSheet
        {
            get => spriteSheet;
        }

        // tempo entre cada frame
        private float timeBetweenEachFrame;
        public float TimeBetweenEachFrame
        {
            get => timeBetweenEachFrame;
        }

        // indica se quando animação terminar, se deve continuar a reproduzi-la ou não
        private bool isLooping;
        public bool IsLooping
        {
            get => isLooping;
        }

        // número de frames
        public int NumberOfFrames
        {
            get => SpriteSheet.Width / FrameHeight;
        }

        // largura e altura de cada frame 
        public int FrameWidth
        {
            get => SpriteSheet.Height;
        }
        public int FrameHeight
        {
            get => SpriteSheet.Height;
        }

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi uma nova animação
        /// </summary>        
        public Animation(Texture2D spriteSheet, float timeBetweenEachFrame, bool isLooping)
        {
            this.spriteSheet = spriteSheet;
            this.timeBetweenEachFrame = timeBetweenEachFrame;
            this.isLooping = isLooping;
        }

        #endregion
    }
}