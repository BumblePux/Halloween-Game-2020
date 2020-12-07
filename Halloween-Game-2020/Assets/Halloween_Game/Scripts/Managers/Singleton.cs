using UnityEngine;

namespace BumblePux.Halloween2020
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
#if UNITY_EDITOR
                        Debug.LogError($"[{typeof(T).Name}] No instance found. Has it been added to the scene?");
#endif
                        instance = null;
                    }
                }

                return instance;
            }
        }

		//--------------------------------------------------------------------------------
        protected virtual void Awake()
        {
            if (instance != null && instance != this as T)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}