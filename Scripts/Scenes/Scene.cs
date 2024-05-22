namespace TDJD_Projeto2.Scripts.Scenes
{
    /// <summary>
    /// Representa a classe base para cada cena 
    /// </summary>
    public abstract class Scene
    {
        #region Campos e propriedades

        protected Game1 game;

        #endregion


        #region Métodos

        public Scene(Game1 game)
        {
            this.game = game;
        }

        public abstract void LoadContent();

        public abstract void Draw();

        public abstract void Update();

        #endregion
    }
}