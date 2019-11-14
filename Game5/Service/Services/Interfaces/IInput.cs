// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInput.cs" company="">
//   
// </copyright>
// <summary>
//   Extension class for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Game5.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game5.Service.Services.Interfaces
{
    /// <summary>
    ///     IInput interface
    /// </summary>
    public interface IInput
    {
        #region Member Variables

        #endregion

        #region Properties

        #endregion

        #region Functions

        /// <summary>
        ///     Gets a value indicating whether mouseButton was pressed
        /// </summary>
        /// <param name="mouseButton">The mouseButton</param>
        /// <returns>The value</returns>
        bool IsClicked(MouseButton mouseButton);

        /// <summary>
        ///     Gets a value indicating whether mouseButton is down
        /// </summary>
        /// <param name="mouseButton">The mouseButton</param>
        /// <returns>The value</returns>
        bool IsDown(MouseButton mouseButton);

        /// <summary>
        ///     Gets a value indicating whether mouseButton is up
        /// </summary>
        /// <param name="mouseButton">The mouseButton</param>
        /// <returns>The value</returns>
        bool IsUp(MouseButton mouseButton);

        /// <summary>
        ///     Gets a value indicating whether key was pressed
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        bool IsClicked(Keys key);

        /// <summary>
        ///     Gets a value indicating whether key is down
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        bool IsDown(Keys key);

        /// <summary>
        ///     Gets a value indicating whether key is up
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        bool IsUp(Keys key);

        /// <summary>
        ///     Gets the position of the mouse in a <see cref="ValueTuple" />
        /// </summary>
        /// <returns>The <see cref="ValueTuple" /></returns>
        (int x, int y, Vector2 vector2) GetMousePos();

        /// <summary>
        ///     PreUpdate function for this <see cref="IInput" />
        /// </summary>
        void PreUpdate();

        /// <summary>
        ///     PostUpdate function for this <see cref="IInput" />
        /// </summary>
        void PostUpdate();

        /// <summary>
        ///     Gets a <see cref="KeyboardState" /> representing the KeyboardState for this frame
        /// </summary>
        /// <returns>
        ///     The <see cref="KeyboardState" />.
        /// </returns>
        KeyboardState GetCurrentKeyState();

        /// <summary>
        ///     Gets a <see cref="KeyboardState" /> representing the KeyboardState for the previous frame
        /// </summary>
        /// <returns>
        ///     The <see cref="KeyboardState" />.
        /// </returns>
        KeyboardState GetPreviousKeyState();

        /// <summary>
        ///     Gets a <see cref="MouseState" /> representing the MouseState for this frame
        /// </summary>
        /// <returns>
        ///     The <see cref="MouseState" />.
        /// </returns>
        MouseState GetCurrentMouseState();

        /// <summary>
        ///     Gets a <see cref="MouseState" /> representing the MouseState for the previous frame
        /// </summary>
        /// <returns>
        ///     The <see cref="MouseState" />.
        /// </returns>
        MouseState GetPreviousMouseState();

        float GetScrollDelta();

        #endregion
    }
}