using System;
using Microsoft.Xna.Framework.Input;

namespace Game5.UI.UIEventArgs
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(MouseState mouseState)
        {
            Mouse = mouseState;
        }

        public MouseState Mouse { get; set; }
    }
}