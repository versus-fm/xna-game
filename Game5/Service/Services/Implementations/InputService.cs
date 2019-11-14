// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputService.cs" company="">
//   
// </copyright>
// <summary>
//   Extension class for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Game5.Data.Attributes.Service;
using Game5.Input;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game5.Service.Services.Implementations
{
    /// <summary>
    ///     InputService class
    /// </summary>
    [Service(typeof(IInput))]
    public class InputService : IInput
    {
        #region Constructors

        #endregion

        #region Member Variables

        /// <summary>
        ///     <see cref="KeyboardState" /> representing the KeyboardState for the current frame
        /// </summary>
        private KeyboardState currentKeyState;

        /// <summary>
        ///     <see cref="KeyboardState" /> representing the KeyboardState for the current frame
        /// </summary>
        private KeyboardState previousKeyState;

        /// <summary>
        ///     <see cref="MouseState" /> representing the MouseState for the previous frame
        /// </summary>
        private MouseState previousMouseState;

        /// <summary>
        ///     <see cref="MouseState" /> representing the MouseState for the current frame
        /// </summary>
        private MouseState currentMouseState;

        #endregion

        #region Properties

        #endregion

        #region Functions

        /// <inheritdoc />
        public bool IsClicked(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.LeftButton:
                    return currentMouseState.LeftButton == ButtonState.Pressed
                           && previousMouseState.LeftButton == ButtonState.Released;
                case MouseButton.RightButton:
                    return currentMouseState.RightButton == ButtonState.Pressed
                           && previousMouseState.RightButton == ButtonState.Released;
                case MouseButton.MiddleButton:
                    return currentMouseState.MiddleButton == ButtonState.Pressed
                           && previousMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.X1:
                    return currentMouseState.XButton1 == ButtonState.Pressed
                           && previousMouseState.XButton1 == ButtonState.Released;
                case MouseButton.X2:
                    return currentMouseState.XButton2 == ButtonState.Pressed
                           && previousMouseState.XButton2 == ButtonState.Released;
                default: return false;
            }
        }

        /// <inheritdoc />
        public bool IsDown(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.LeftButton: return currentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.RightButton: return currentMouseState.RightButton == ButtonState.Pressed;
                case MouseButton.MiddleButton: return currentMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButton.X1: return currentMouseState.XButton1 == ButtonState.Pressed;
                case MouseButton.X2: return currentMouseState.XButton2 == ButtonState.Pressed;
                default: return false;
            }
        }

        /// <inheritdoc />
        public bool IsUp(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.LeftButton: return currentMouseState.LeftButton == ButtonState.Released;
                case MouseButton.RightButton: return currentMouseState.RightButton == ButtonState.Released;
                case MouseButton.MiddleButton: return currentMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.X1: return currentMouseState.XButton1 == ButtonState.Released;
                case MouseButton.X2: return currentMouseState.XButton2 == ButtonState.Released;
                default: return false;
            }
        }

        /// <inheritdoc />
        public bool IsClicked(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }

        /// <inheritdoc />
        public bool IsDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        /// <inheritdoc />
        public bool IsUp(Keys key)
        {
            return currentKeyState.IsKeyUp(key);
        }

        /// <inheritdoc />
        public (int x, int y, Vector2 vector2) GetMousePos()
        {
            return (currentMouseState.X, currentMouseState.Y, new Vector2(currentMouseState.X, currentMouseState.Y));
        }

        /// <inheritdoc />
        public void PreUpdate()
        {
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        /// <inheritdoc />
        public void PostUpdate()
        {
            previousMouseState = currentMouseState;
            previousKeyState = currentKeyState;
        }

        /// <inheritdoc />
        public KeyboardState GetCurrentKeyState()
        {
            return currentKeyState;
        }

        /// <inheritdoc />
        public KeyboardState GetPreviousKeyState()
        {
            return previousKeyState;
        }

        /// <inheritdoc />
        public MouseState GetCurrentMouseState()
        {
            return currentMouseState;
        }

        /// <inheritdoc />
        public MouseState GetPreviousMouseState()
        {
            return previousMouseState;
        }

        public float GetScrollDelta()
        {
            return previousMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;
        }

        #endregion

        #region Enums

        #endregion
    }
}