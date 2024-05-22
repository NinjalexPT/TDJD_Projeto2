using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TDJD_Projeto2.Scripts.UI
{
    /// <summary>
    /// Representa um botão
    /// </summary>
    public class Button
    {
        #region Campos e propriedades

        // sobre textura (imagem)
        private Texture2D texture;
        private int width;
        private int height;
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }
        }

        // sobre texto
        private SpriteFont font;
        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }
        public string Text { get; set; }

        // sobre eventos
        public event EventHandler Click;
        private MouseState currentMouse;
        private MouseState previousMouse;

        #endregion


        #region Carregar

        /// <summary>
        /// Constroi um novo botão, que é possível definir a largura e altura
        /// </summary>
        public Button(Texture2D texture, SpriteFont font, int width, int height)
        {
            this.texture = texture;
            this.font = font;
            this.width = width;
            this.height = height;

            TextColor = Color.White;
            BackgroundColor = Color.White;
        }

        /// <summary>
        /// Constroi um novo botão, onde a largura e altura é definida automaticamente pelo tamanho da padrão da textura
        /// </summary>
        public Button(Texture2D texture, SpriteFont font)
        {
            this.texture = texture;
            this.font = font;

            TextColor = Color.White;
            BackgroundColor = Color.White;
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza o botão
        /// </summary>
        public void Update()
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion


        #region Desenhar

        /// <summary>
        /// Desenha o botão
        /// </summary>
        public void Draw()
        {
            BackgroundColor = Color.White;

            // desenha a imagem definida
            Game1._spriteBatch.Draw(texture, Rectangle, BackgroundColor);

            // desenha o texto, se o texto foi definido
            if (!string.IsNullOrEmpty(Text))
            {
                float x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                float y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);

                Game1._spriteBatch.DrawString(font, Text, new Vector2(x, y), TextColor);
            }
        }

        #endregion
    }
}