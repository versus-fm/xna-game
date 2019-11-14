using System;

namespace Game5.Data.DataStructures
{
    public class Array2D<T>
    {
        private T[] items;
        private int width, height;

        public Array2D(int width, int height)
        {
            items = new T[width * height];
            this.width = width;
            this.height = height;
        }

        public T this[int x, int y]
        {
            get
            {
                if (x >= width || y >= height || x < 0 || y < 0) return default(T);
                return items[x + width * y];
            }
            set
            {
                if (x >= width || y >= height || x < 0 || y < 0) return;
                items[x + width * y] = value;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index >= width * height || index < 0) return default(T);
                return items[index];
            }
            set
            {
                if (index >= width * height || index < 0) return;
                items[index] = value;
            }
        }

        public void ForEach(Action<T> action)
        {
            foreach (var item in items) action(item);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public static void Resize(ref Array2D<T> array, int newWidth, int newHeight)
        {
            array.width = newWidth;
            array.height = newHeight;
            Array.Resize(ref array.items, newWidth * newHeight);
        }
    }
}