// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextInputService.cs" company="">
//   
// </copyright>
// <summary>
//   Extension class for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Game5.Input.Interfaces;
using Microsoft.Xna.Framework;

namespace Game5.Service.Services.Interfaces
{
    /// <summary>
    ///     ITextInputService class
    /// </summary>
    public interface ITextInputService
    {
        #region Member Variables

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Functions

        void HookTextInput(ITextInput input);

        void RemoveTextInput(ITextInput input);

        void Execute(object sender, TextInputEventArgs args);

        #endregion

        #region Enums

        #endregion
    }
}