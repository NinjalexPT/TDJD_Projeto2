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
        private Texture2D texture;
        private float lifeTime = 1.0f;
        public Bullet(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            Position = position;
            Velocity = velocity;
            IsActive = true;
            this.texture = texture;
        }

        // obtém o retângulo colisor do jogador através dos limites da textura
        public Rectangle Collider
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }
        public void Update(float elapsedTime)
        {
            Position += Velocity * elapsedTime;

            lifeTime -= elapsedTime;
            


            // Desativar a bala se o tempo de vida expirar
            if (lifeTime <= 0)
            {
                IsActive = false;
            }
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
