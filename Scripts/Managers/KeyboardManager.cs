using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TDJD_Projeto2.Scripts.Managers
{
    public class KeyboardManager
    {
        #region Campos e propriedes

        private KeyboardManager keyboardManager;
        private Dictionary<Keys, KeyState> keysAndState;

        // evemtos do teclado
        private enum KeyState
        {
            PRESSED,
            HELD,
            UP,
            NONE
        }

        #endregion


        #region carregar

        /// <summary>
        /// Cria um teclado, permite criar apenas um teclado por cena
        /// </summary>
        public KeyboardManager()
        {
            if (keyboardManager == null)
            {
                keysAndState = new Dictionary<Keys, KeyState>();
                keyboardManager = this;
            }
            else
            {
                throw new Exception("Erro: Uma instância já foi criada!");
            }
        }

        #endregion


        #region Atualizar

        /// <summary>
        /// Atualiza o estado do teclado
        /// </summary>
        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressedKeys = state.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (!keysAndState.ContainsKey(key))
                {
                    keysAndState.Add(key, KeyState.PRESSED);
                }
                else
                {
                    if (keysAndState[key] == KeyState.PRESSED)
                    {
                        keysAndState[key] = KeyState.HELD;
                    }
                    else if (keysAndState[key] == KeyState.UP || keysAndState[key] == KeyState.NONE)
                    {
                        keysAndState[key] = KeyState.PRESSED;
                    }
                }
            }

            foreach (Keys key in keysAndState.Keys.ToArray())
            {
                if (!pressedKeys.Contains(key))
                {
                    if (keysAndState[key] == KeyState.UP)
                    {
                        keysAndState[key] = KeyState.NONE;
                    }
                    else if (keysAndState[key] == KeyState.PRESSED || keysAndState[key] == KeyState.HELD)
                    {
                        keysAndState[key] = KeyState.UP;
                    }
                }
            }
        }

        /// <summary>
        /// Quando o utilizador pressiona uma tecla
        /// </summary>
        public bool IsKeyPressed(Keys key) => keysAndState.ContainsKey(key) && keysAndState[key] == KeyState.PRESSED;

        /// <summary>
        /// Quando o utilizador pressiona uma tecla e deixa de pressionar
        /// </summary>
        public bool IsKeyUp(Keys key) => keysAndState.ContainsKey(key) && keysAndState[key] == KeyState.UP;

        /// <summary>
        /// Quando o utilizador pressiona uma tecla em um intervalo de tempo
        /// </summary>
        public bool isKeyHeld(Keys key) => keysAndState.ContainsKey(key) && keysAndState[key] == KeyState.HELD;

        #endregion
    }
}