using Microsoft.Xna.Framework;
using System;

namespace TDJD_Projeto2.Scripts.Utils
{
    /// <summary>
    /// Conjunto de métodos para ajudar a trabalhar com retângulos
    /// </summary>
    public static class RectangleHelper
    {
        /// <summary>
        /// Obtém o ponto central de um rectângulo
        /// </summary>
        public static Vector2 GetOrigin(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2f, rectangle.Y + rectangle.Height / 2f);
        }

        /// <summary>
        /// Obtém a posição do centro da borda inferior de um rectângulo
        /// </summary>
        public static Vector2 GetBottomCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2.0f, rectangle.Bottom);
        }

        /// <summary>
        /// Calcula a profundidade da interseção de 2 retângulos
        /// </summary>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // calcula a metade do tamanho de cada retângulo
            float halfWidthA = rectA.Width / 2f;
            float halfHeightA = rectA.Height / 2f;
            float halfWidthB = rectB.Width / 2f;
            float halfHeightB = rectB.Height / 2f;

            // calcula os centros de cada retângulo
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // calcula as distâncias atuais e mínimas entre os retângulos
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // se não houve interseção entre os retângulos
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            // calcula a profundidade da interseção
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
    }
}