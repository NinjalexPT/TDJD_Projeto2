using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDJD_Projeto2.Scripts.Utils;

namespace TDJD_Projeto2.Scripts.Sprites
{
    /// <summary>
    /// Representa um tile
    /// </summary>
    public class Tile
    {
        #region Campos e propriedades

        // textura e tipo de tile
        public Texture2D texture;
        public CollisionType type;

        // largura e altura de cada tile 
        public const int WIDTH = 32;
        public const int HEIGHT = 32;

        public static Vector2 size = new Vector2(WIDTH, HEIGHT);

        #endregion


        #region Carregar tile

        /// <summary>
        /// Constroi um novo tile
        /// </summary>
        public Tile(Texture2D texture, CollisionType type)
        {
            this.texture = texture;
            this.type = type;
        }

        #endregion
    }
}