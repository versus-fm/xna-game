using System.Diagnostics.CodeAnalysis;

namespace Game5.Input
{
    /// <summary>
    ///     Enum representing the different mouse buttons
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum MouseButton
    {
        /// <summary>
        ///     The left button.
        /// </summary>
        LeftButton,

        /// <summary>
        ///     The right button.
        /// </summary>
        RightButton,

        /// <summary>
        ///     The x 1.
        /// </summary>
        X1,

        /// <summary>
        ///     The x 2.
        /// </summary>
        X2,

        /// <summary>
        ///     The middle button.
        /// </summary>
        MiddleButton
    }
}