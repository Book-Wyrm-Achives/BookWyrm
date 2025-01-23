using System.Collections.Generic;
using Godot;

namespace BookWyrm.GodotExtensions
{
    public static class NodeExtensions
    {
        public static T[] GetChildrenOfType<T>(this Node node, bool includeInternal = false) where T : Node
        {
            List<T> tChildren = new List<T>();
            foreach (var child in node.GetChildren(includeInternal))
            {
                if (child is T tChild)
                {
                    tChildren.Add(tChild);
                }
            }

            return tChildren.ToArray();
        }
    }
}