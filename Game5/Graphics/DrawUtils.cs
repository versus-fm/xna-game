using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.Graphics
{
    public class DrawUtils
    {
        private readonly Stack<RenderTarget2D> renderTargetStack;

        public DrawUtils()
        {
            renderTargetStack = new Stack<RenderTarget2D>();
        }

        public void Bind(RenderTarget2D renderTarget)
        {
            renderTargetStack.Push(renderTarget);
        }

        public RenderTarget2D Unbind()
        {
            if (renderTargetStack.Count == 0) return null;
            var rt = renderTargetStack.Pop();
            //using (FileStream fs = new FileStream(renderTargetStack.Count.ToString() + ".png", FileMode.Create))
            //{
            //    rt.SaveAsPng(fs, rt.Width, rt.Height);
            //}

            return rt;
        }

        public RenderTarget2D Top()
        {
            if (renderTargetStack.Count == 0) return null;
            return renderTargetStack.Peek();
        }

        public void Clear()
        {
            renderTargetStack.Clear();
        }
    }
}