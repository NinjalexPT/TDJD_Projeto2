namespace TDJD_Projeto2.Scripts.Utils
{
    /// <summary>
    /// As diferentes colisões que existem para os tiles
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// Significa que o tile é um objeto transparente
        /// (outros objetos podem se sobrepor a ele)
        /// </summary>
        transparent = 0,

        /// <summary>
        /// Significa que o tile é um objeto físico
        /// (outros objetos não se podem sobrepor a ele)
        /// </summary>
        block = 1
    }
}