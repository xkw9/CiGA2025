using UnityEngine;

namespace Assets.Utils
{
    public static class UnityUtils
    {
        public static T MakeObject<T>(string name, Transform parent = null) 
            where T : Component
        {
            var obj = new GameObject(name);
            obj.AddComponent<T>();

            if (parent != null)
            {
                obj.transform.SetParent(parent);
            }

            return obj.GetComponent<T>();
        }
    }
}
