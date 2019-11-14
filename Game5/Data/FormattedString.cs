using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Game5.Data.Helper;
using Microsoft.Xna.Framework;

namespace Game5.Data
{
    public class FormattedString : IEnumerable<StyledString>
    {
        private List<StyledString> text;

        private FormattedString()
        {
            text = new List<StyledString>();
        }

        public int Count => text.Count;

        public StyledString this[int i] => text[i];

        public IEnumerator<StyledString> GetEnumerator()
        {
            return ((IEnumerable<StyledString>) text).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<StyledString>) text).GetEnumerator();
        }

        public static FormattedString BuildString(string text, Color? defaultColor = null)
        {
            if (defaultColor == null) defaultColor = Color.White;
            var first = new List<StyledString>();
            var str = new FormattedString();
            var matches = Regex.Matches(text, "<color=([\\w\\d#]+)>([^<][^/]*)<\\/color>");
            var lastEnd = 0;
            foreach (var match in matches.Cast<Match>())
            {
                first.Add(new StyledString((Color) defaultColor, text.Substring(lastEnd, match.Index - lastEnd)));
                lastEnd = match.Index + match.Length;
                first.Add(new StyledString(ColorHelper.FromName(match.Groups[1].Value), match.Groups[2].Value));
            }

            first.Add(new StyledString((Color) defaultColor, text.Substring(lastEnd, text.Length - lastEnd)));

            str.text = first;

            //foreach(var t in first)
            //{
            //    var strings = t.text.Split(' ');
            //    foreach(var s in strings)
            //    {
            //        str.text.Add(new StyledString(t.color, s + " "));
            //    }

            //}
            //str.text[str.text.Count - 1].text = str.text[str.text.Count - 1].text.TrimEnd(' ');
            return str;
        }

        public string GetVisibleString()
        {
            return string.Join("", text.Select(x => x.text));
        }

        public void ForEach(Action<StyledString> action)
        {
            foreach (var t in text) action(t);
        }
    }
}