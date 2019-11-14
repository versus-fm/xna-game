using System.Collections.Generic;
using System.Linq;
using Game5.Data.Attributes.UI;

namespace Game5.UI.Components
{
    [UIDocElement("genericlist")]
    public class ListView<T> : Panel
    {
        private readonly Dictionary<T, Text> controls;
        private IEnumerable<T> items;
        private IEnumerable<T> lastItems;

        public ListView()
        {
            controls = new Dictionary<T, Text>();
        }

        public ListView(IEnumerable<T> items)
        {
            this.items = items;
        }

        public T SelectedItem => controls.Where(x => x.Value == children.Find(y => y.Active)).First().Key;

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

                if (added.Count() != 0 || removed != null && removed.Count() != 0)
                {
                    children = controls.Values.ToList<UserControl>();
                    for (var i = 0; i < children.Count; i++)
                        if (i != 0)
                        {
                            children[i].Name = Name + "_item" + i;
                            children[i].Aligned = children[i - 1].Name;
                            children[i].Anchor = AnchorPoint.BottomCenter;
                            children[i].MarkDirty();
                        }
                        else
                        {
                            children[i].Name = Name + "_item" + i;
                            children[i].Anchor = AnchorPoint.TopCenter;
                        }
                }

                lastItems = items.ToArray().AsEnumerable();
            }

            base.Update();
        }

        protected virtual void OnItemAdded(T item)
        {
            var v = (Text) UserInterface.CreateEmpty("text", GetFreeIndexName());
            v.Content = item.ToString();
            v.DynamicHeight = "1em";
            v.DynamicWidth = "95%";
            v.SetParent(this);
            v.Anchor = AnchorPoint.Center;
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