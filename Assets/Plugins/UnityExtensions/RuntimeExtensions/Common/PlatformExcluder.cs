using UnityEngine;

namespace UnityExtensions
{
    [AddComponentMenu("Unity Extensions/Platform Excluder")]
    public class PlatformExcluder : ScriptableComponent
    {
        [SerializeField, Flags]
        PlatformMask _excludedPlatforms = default;


        void Awake()
        {
            if (_excludedPlatforms.Contains(Application.platform))
                Destroy(gameObject);
        }
    }
}