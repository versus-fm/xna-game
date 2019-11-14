using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game5.Data.Helper
{
    public static class ByteStreamHelper
    {
        public static byte[] Append(this byte[] bytes, int position, byte[] bytesToAppend)
        {
            while (bytesToAppend.Length + position > bytes.Length) Array.Resize(ref bytes, bytes.Length * 2);
            for (var i = position; i < position + bytesToAppend.Length; i++) bytes[i] = bytesToAppend[i - position];
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, Vector2 v)
        {
            var x = BitConverter.GetBytes(v.X);
            var y = BitConverter.GetBytes(v.Y);
            bytes = bytes.Append(position, x);
            position += y.Length;
            bytes = bytes.Append(position, y);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, Point v)
        {
            var x = BitConverter.GetBytes(v.X);
            var y = BitConverter.GetBytes(v.Y);
            bytes = bytes.Append(position, x);
            position += x.Length;
            bytes = bytes.Append(position, y);
            position += y.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, Color v)
        {
            var p = BitConverter.GetBytes(v.PackedValue);
            bytes = bytes.Append(position, p);
            position += p.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, Rectangle rec)
        {
            var x = BitConverter.GetBytes(rec.X);
            var y = BitConverter.GetBytes(rec.Y);
            var w = BitConverter.GetBytes(rec.Width);
            var h = BitConverter.GetBytes(rec.Height);
            bytes = bytes.Append(position, x);
            position += x.Length;
            bytes = bytes.Append(position, y);
            position += y.Length;
            bytes = bytes.Append(position, w);
            position += w.Length;
            bytes = bytes.Append(position, h);
            position += h.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, int i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, bool i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, float i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, string i)
        {
            var x = Encoding.ASCII.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            bytes = bytes.Append(position, new byte[] {0});
            position++;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, long i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, double i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, byte i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static byte[] Write(this byte[] bytes, ref int position, short i)
        {
            var x = BitConverter.GetBytes(i);
            bytes = bytes.Append(position, x);
            position += x.Length;
            return bytes;
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public static Rectangle ReadRectangle(this BinaryReader reader)
        {
            return new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        }

        public static Point ReadPoint(this BinaryReader reader)
        {
            return new Point(reader.ReadInt32(), reader.ReadInt32());
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            return new Color(reader.ReadUInt32());
        }

        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var s = "";
            byte b = 0;
            while ((b = reader.ReadByte()) != 0) s += (char) b;
            return s;
        }

        public static byte[] TrimEnd(this byte[] bytes)
        {
            var j = 0;
            for (var i = bytes.Length - 1; i >= 0; i--)
                if (bytes[i] == 0)
                {
                    j++;
                }
                else
                {
                    j--;
                    break;
                }

            return bytes.Take(bytes.Length - j).ToArray();
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}