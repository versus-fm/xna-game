using System;
using System.Collections.Generic;
using System.Linq;
using Game5.Data.Attributes.UI;
using Microsoft.Xna.Framework;

namespace Game5.UI.Components
{
    [UIDocElement("genericgrid")]
    public class GridView<T> : Panel
    {
        private readonly Dictionary<T, Icon> controls;

        private IEnumerable<T> items, lastItems;
        private int maxElementsWidth, lastWidth;

        public GridView()
        {
            controls = new Dictionary<T, Icon>();
        }

        public GridView(Func<T, string> textureSelector)
        {
            TextureSelector = textureSelector;
            controls = new Dictionary<T, Icon>();
        }

        [UIDocField("elementWidth")] public string DynamicGridElementWidth { get; set; }

        [UIDocField("elementHeight")] public string DynamicGridElementHeight { get; set; }

        [UIDocField("elementPadding")] public int GridElementPadding { get; set; }

        public Func<T, string> TextureSelector { get; set; }

        public void SetData(IEnumerable<T> items)
        {
            this.items = items;
        }


        public override void Update()
        {
            if (items != null)
            {
                IEnumerable<T> added = null;
                IEnumerable<T> removed = null;
                if (lastItems == null)
                    added = items.ToArray().AsEnumerable();
                else
                    added = items.Except(lastItems);

                if (lastItems != null) removed = lastItems.Except(items);

                if (added.Count() != 0)
                    foreach (var item in added)
                        OnItemAdded(item);

                if (removed != null && removed.Count() != 0)
                    foreach (var item in removed)
                        OnItemRemoved(item);

                if (added.Count() != 0 || removed != null && removed.Count() != 0 || lastWidth != maxElementsWidth)
                {
                    children = controls.Values.ToList<UserControl>();
                    for (var i = 0; i < children.Count; i++)
                        if (i < maxElementsWidth)
                        {
                            if (i != 0)
                            {
                                children[i].Name = Name + "_item" + i;
                                children[i].Aligned = children[i - 1].Name;
                                children[i].Anchor = AnchorPoint.Right;
                                children[i].MarkDirty();
                            }
                            else
                            {
                                children[i].Name = Name + "_item" + i;
                                children[i].Anchor = AnchorPoint.TopLeft;
                                children[i].Aligned = "";
                                children[i].MarkDirty();
                            }
                        }
                        else
                        {
                            children[i].Name = Name + "_item" + i;
                            children[i].Aligned = children[i - maxElementsWidth].Name;
                            children[i].Anchor = AnchorPoint.BottomCenter;
                            children[i].MarkDirty();
                        }
                }

                lastItems = items.ToArray().AsEnumerable();
                lastWidth = maxElementsWidth;
            }

            base.Update();
        }

        protected virtual void OnItemAdded(T item)
        {
            var v = (Icon) UserInterface.CreateEmpty("icon", GetFreeIndexName());
            v.ForeColor = Color.White;
            v.TextureName = TextureSelector(item);
            v.DynamicHeight = DynamicGridElementHeight;
            v.DynamicWidth = DynamicGridElementWidth;
            v.SetParent(this);
            v.Anchor = AnchorPoint.Center;
            v.HorizontalPadding = GridElementPadding;
            v.VerticalPadding = GridElementPadding;
            maxElementsWidth = DestRect.Width / (v.Size.X + v.HorizontalPadding * 2);
            controls.Add(item, v);
        }

        protected virtual void OnItemRemoved(T item)
        {
            controls.Remove(item);
        }

        private string GetFreeIndexName()
        {
            var i = 0;
            while (children.Exists(x => x.Name == Name + "_item" + i)) i++;
            return Name + "_item" + i;
        }
    }
}