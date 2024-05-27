using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TDJD_Projeto2.Scripts.Effects;
using TDJD_Projeto2.Scripts.Managers;
using TDJD_Projeto2.Scripts.Scenes;
using TDJD_Projeto2.Scripts.Utils;
using System;
using System.Collections.Generic;


namespace TDJD_Projeto2.Scripts.Sprites
{
    public class Bullet
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public bool IsActive;
        private Animator animator;
        public Bullet(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
            IsActive = true;
        }
        // limites (bordas) da textura
        private Rectangle textureBounds;
        // obtém o retângulo colisor do jogador através dos limites da textura
        public Rectangle Collider
        {
            get
            {
                int left = (int)(Position.X - animator.Origin.X) + textureBounds.X;
                int top = (int)(Position.Y - animator.Origin.Y) + textureBounds.Y;
                int right = textureBounds.Width;
                int bottom = textureBounds.Height;

                return new Rectangle(left, top, right, bottom);
            }
        }
        public void Update(float elapsedTime)
        {
            Position += Velocity * elapsedTime;
            
            // Desativar o projétil se sair da tela (ajuste conforme necessário)
            /*if (Position.X < 0 || Position.X > Game1.graphics.PreferredBackBufferWidth ||
            Position.Y < 0 || Position.Y > Game1.graphics.PreferredBackBufferHeight)
            {
                IsActive = false;
            }*/
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (IsActive)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }

    }
}
