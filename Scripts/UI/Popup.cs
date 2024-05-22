using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJD_Projeto2.Scripts.UI
{
    /// <summary>
    /// Representa uma popup
    /// </summary>
    public class Popup
    {
        #region Campos e propriedades

        private string texturePath;
        private Texture2D texture;

        // centro do ecrã
        private Vector2 screenCenter;

        // centrar imagem
        public Vector2 Position
        {
            get
            {
                Vector2 textureSize = new Vector2(texture.Width, texture.Height);
                return screenCenter - textureSize / 2;
            }
        }

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi uma nova popup
        /// </summary>
        public Popup(string texturePath, Vector2 screenCenter)
        {
            this.texturePath = texturePath;
            this.screenCenter = screenCenter;

            texture = Game1._content.Load<Texture2D>(this.texturePath);
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenhar a popup
        /// </summary>
        public void Draw()
        {
            Game1._spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        #endregion
    }
}