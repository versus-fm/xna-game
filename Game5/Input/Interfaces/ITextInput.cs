// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextInput.cs" company="">
//   
// </copyright>
// <summary>
//   Extension class for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Xna.Framework;

namespace Game5.Input.Interfaces
{
    /// <summary>
    ///     ITextInput class
    /// </summary>
    public interface ITextInput
    {
        #region Functions

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args"></param>
        void OnTextInput(object sender, TextInputEventArgs args);

        #endregion

        #region Member Variables

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Enums

        #endregion
    }
}