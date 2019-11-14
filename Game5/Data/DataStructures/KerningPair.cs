using System.Runtime.InteropServices;

namespace Game5.Data.DataStructures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KerningPair
    {
        public ushort wFirst; // might be better off defined as char
        public ushort wSecond; // might be better off defined as char
        public int iKernAmount;

        public KerningPair(ushort wFirst, ushort wSecond, int iKernAmount)
        {
            this.wFirst = wFirst;
            this.wSecond = wSecond;
            this.iKernAmount = iKernAmount;
        }

        public override string ToString()
        {
            return string.Format("{{First={0}, Second={1}, Amount={2}}}", wFirst, wSecond, iKernAmount);
        }
    }
}