using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJD_Projeto2.Scripts.UI
{
    /// <summary>
    /// Representa uma label de texto
    /// </summary>
    public class Label
    {
        #region Campos e propriedades

        // caminho do ficheiro da fonte e texto da  fonte
        private string fontPath;
        private SpriteFont font;
        private string text;

        // posição e cor
        private Vector2 position;
        private Color color;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi uma nova label de texto
        /// </summary>
        public Label(string fontPath, string text, Vector2 position, Color color)
        {
            this.fontPath = fontPath;
            this.text = text;
            this.position = position;
            this.color = color;

            font = Game1._content.Load<SpriteFont>(fontPath);
        }

        /// <summary>
        /// Define a posição do texto no centro do ecrã
        /// </summary>
        public void SetCenterTextInScreen(Vector2 screenCenter)
        {
            Vector2 size = font.MeasureString(text);
            position = screenCenter - size / 2;
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha a label
        /// </summary>
        public void Draw()
        {
            Game1._spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        #endregion
    }
}