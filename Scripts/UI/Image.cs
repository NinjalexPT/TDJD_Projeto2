using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJD_Projeto2.Scripts.UI
{
    /// <summary>
    /// Representa uma imagem
    /// </summary>
    public class Image
    {
        #region Campos e propriedades

        private Texture2D texture;
        private Vector2 position;
        private float layer;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi uma nova imagem, que é possível definir a camada em relação a outros objetos
        /// </summary>
        public Image(Texture2D texture, Vector2 position, float layer)
        {
            this.texture = texture;
            this.position = position;
            this.layer = layer;
        }

        /// <summary>
        /// Constroi uma nova imagem, que define a camada padrão de um objeto
        /// </summary>
        public Image(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            layer = 1f;
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha a imagem
        /// </summary>
        public void Draw()
        {
            Game1._spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }

        #endregion
    }
}