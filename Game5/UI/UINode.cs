using System.Xml;

namespace Game5.UI
{
    public struct UINode
    {
        public XmlNode Node { get; set; }
        public XmlNode Parent { get; set; }
    }
}