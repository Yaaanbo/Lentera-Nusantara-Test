using UnityEngine;

namespace Clicker.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = GetComponent<T>();
            else
                Destroy(Instance);
        }
    }
}
