using System.Collections.Generic;
using Godot;

namespace BookWyrm.GodotExtensions
{
    public static class NodeExtensions
    {
        public static bool TryGetChildOfType<T>(this Node node, out T child) where T : Node
        {
             foreach (var c in node.GetChildren(false))
            {
                if (c is T tChild)
                {
                    child = tChild;
                    return true;
                }
            }
            child = default;
            return false;
        }

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