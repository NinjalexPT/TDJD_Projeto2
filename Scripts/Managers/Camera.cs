using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJD_Projeto2.Scripts.Managers
{
    /// <summary>
    /// Representa a câmera do jogo
    /// </summary>
    public class Camera
    {
        #region Campos e propriedades

        private Matrix transform;
        public Matrix Transform
        {
            get => transform;
        }

        private Viewport viewport;

        // centro do ecrã, independente quando se move para direita ou esquerda no mapa
        private Vector2 center;
        public Vector2 Center
        {
            get => center;
            set => center = value;
        }

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi a câmera do jogo para cada nível
        /// </summary>
        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza a câmera, quando o jogador se move
        /// </summary>
        public void Update(Vector2 position, int xOffset, int yOffset)
        {
            if (position.X < viewport.Width / 2)
            {
                center.X = viewport.Width / 2;
            }
            else if (position.X > xOffset - (viewport.Width / 2))
            {
                center.X = xOffset - (viewport.Width / 2);
            }
            else
            {
                center.X = position.X;
            }

            if (position.Y < viewport.Height / 2)
            {
                center.Y = viewport.Height / 2;
            }
            else if (position.Y > yOffset - (viewport.Height / 2))
            {
                center.Y = yOffset - (viewport.Height / 2);
            }
            else
            {
                center.Y = position.Y;
            }

            transform = Matrix.CreateTranslation(new Vector3(
                -Center.X + (viewport.Width / 2),
                -Center.Y + (viewport.Height / 2),
                0));
        }

        #endregion
    }
}