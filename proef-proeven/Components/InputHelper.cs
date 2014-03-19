using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components
{
    class InputHelper
    {
        private static InputHelper instance;
        public static InputHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputHelper();
                return instance;
            }
        }

        MouseState lastMouseState, curMouseState;
        KeyboardState lastKeyboardState, curKeyboardState;

        /// <summary>
        /// Update the states of the mouse and keyboard
        /// </summary>
        public void Update()
        {
            lastMouseState = curMouseState;
            curMouseState = Mouse.GetState();

            lastKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();
        }

        #region MOUSE STUFF

        /// <summary>
        /// Check if the left mouse buttons is released this frame
        /// </summary>
        /// <returns>True if lastState pressed and this state released</returns>
        public bool IsLeftMouseReleased()
        {
            return (lastMouseState.LeftButton == ButtonState.Pressed && curMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Check if the left mouse buttons is pressed this frame
        /// </summary>
        /// <returns>True if lastState released and this state pressed</returns>
        public bool IsLeftMousePressed()
        {
            return (lastMouseState.LeftButton == ButtonState.Released && curMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the Left mouse button is pressed
        /// </summary>
        /// <returns>True if curState = pressed</returns>
        public bool LeftMouseDown()
        {
            return curMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Check if the right mouse button is released
        /// </summary>
        /// <returns>True if curState = released</returns>
        public bool RightMouseUp()
        {
            return curMouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Check if the right mouse buttons is released this frame
        /// </summary>
        /// <returns>True if lastState pressed and this state released</returns>
        public bool IsRightMouseReleased()
        {
            return (lastMouseState.RightButton == ButtonState.Pressed && curMouseState.RightButton == ButtonState.Released);
        }

        /// <summary>
        /// Check if the right mouse buttons is pressed this frame
        /// </summary>
        /// <returns>True if lastState released and this state pressed</returns>
        public bool IsRightMousePressed()
        {
            return (lastMouseState.RightButton == ButtonState.Released && curMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Check if the right mouse button is pressed
        /// </summary>
        /// <returns>True if curState = pressed</returns>
        public bool RightMouseDown()
        {
            return curMouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Gets the position of the mouse
        /// </summary>
        /// <returns>The position of the mouse</returns>
        public Vector2 MousePos()
        {
            return new Vector2(curMouseState.X, curMouseState.Y);
        }

        #endregion

        #region KEYBOARD STUFF

        /// <summary>
        /// Check if an key is released this frame
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>True if the key is released this frame an pressed in the last</returns>
        public bool IsKeyReleased(Keys k)
        {
            return curKeyboardState.IsKeyUp(k) && lastKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Check if an key is pressed this frame
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>True if the key is pressed this frame an released in the last</returns>
        public bool IsKeyPressed(Keys k)
        {
            return curKeyboardState.IsKeyDown(k) && lastKeyboardState.IsKeyUp(k);
        }

        /// <summary>
        /// Check if a key is being down
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>True if the key is down</returns>
        public bool IsKeyDown(Keys k)
        {
            return curKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Check if a key isn't being hold down
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>True if the key is up</returns>
        public bool IsKeyUp(Keys k)
        {
            return curKeyboardState.IsKeyUp(k);
        }

        #endregion
    }
}
