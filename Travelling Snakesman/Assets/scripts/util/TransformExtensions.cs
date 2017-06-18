using UnityEngine;

namespace util
{
    public static class TransformExtenstions
    {
        /// <summary>
        /// Gets or add a component. Usage example:
        /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            return child.GetComponent<T>() ?? child.gameObject.AddComponent<T>();
        }

        public static T AddNewComponent<T>(this Component child) where T : Component
        {
            var component = child.GetComponent<T>();
            if (component != null)
            {
                Component.Destroy(component);
            }
            return child.gameObject.AddComponent<T>();
        }
    }
}