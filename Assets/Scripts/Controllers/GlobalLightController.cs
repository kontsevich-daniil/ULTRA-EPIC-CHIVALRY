using UnityEngine;
using RenderSettings = UnityEditor.Experimental.RenderSettings;

namespace Controllers
{
    public class GlobalLightController: MonoBehaviour
    {
        private static GlobalLightController _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}