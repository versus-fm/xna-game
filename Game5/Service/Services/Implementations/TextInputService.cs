// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextInputService.cs" company="">
//   
// </copyright>
// <summary>
//   Extension class for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Game5.Data.Attributes.Service;
using Game5.Input.Interfaces;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;

namespace Game5.Service.Services.Implementations
{
    /// <summary>
    ///     TextInputService class
    /// </summary>
    [Service(typeof(ITextInputService))]
    public class TextInputService : ITextInputService
    {
        #region Member Variables

        private readonly List<ITextInput> textInputs;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextInputService" /> class.
        ///     Default TextInputService Constructor
        /// </summary>
        public TextInputService()
        {
            textInputs = new List<ITextInput>();
        }

        #endregion

        #region Properties

        #endregion

        #region Functions

        public void HookTextInput(ITextInput input)
        {
            textInputs.Add(input);
        }

        public void RemoveTextInput(ITextInput input)
        {
            textInputs.Remove(input);
        }

        public void Execute(object sender, TextInputEventArgs args)
        {
            textInputs.ForEach(x => x?.OnTextInput(sender, args));
        }

        #endregion

        #region Enums

        #endregion
    }
}