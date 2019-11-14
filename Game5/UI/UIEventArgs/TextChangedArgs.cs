using System;
using Game5.Data;

namespace Game5.UI.UIEventArgs
{
    public class TextChangedArgs : EventArgs
    {
        public TextChangedArgs(string last, string current, FormattedString lastFormat, FormattedString currentFormat)
        {
            Last = last;
            Current = current;
            CurrentFormat = currentFormat;
            LastFormat = lastFormat;
        }

        public string Last { get; set; }
        public string Current { get; set; }
        public FormattedString CurrentFormat { get; set; }
        public FormattedString LastFormat { get; set; }
    }
}