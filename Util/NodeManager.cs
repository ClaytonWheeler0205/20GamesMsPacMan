using Godot;

namespace Util.ExtensionMethods
{
    public static class NodeManager
    {
        public static bool IsValid<T>(this T node) where T : Godot.Object
        {
            return node != null
                && Godot.Object.IsInstanceValid(node)
                && !node.IsQueuedForDeletion();
        }

        public static void SafeQueueFree(this Node node)
        {
            if(node.IsValid())
            {
                node.QueueFree();
            }
        }
    }
}
