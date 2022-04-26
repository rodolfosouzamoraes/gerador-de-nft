using UnityEngine;

/// <summary>
/// Classe para genrenciar o uso de mem�ria do Garbage Collection
/// </summary>
public class GarbageCollectionManager : MonoBehaviour
{
    [SerializeField] private float maxTimeBetweenGarbageCollections = 60f;
    private float _timeSinceLastGarbageCollection;

    private void Start()
    {
#if !UNITY_EDITOR
    GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
#endif
        // You might want to run this during loading times, screen fades and such.
        // Events.OnScreenFade += CollectGarbage;
    }

    private void Update()
    {
        _timeSinceLastGarbageCollection += Time.unscaledDeltaTime;
        //Debug.Log($"Time: {_timeSinceLastGarbageCollection}");
        if (_timeSinceLastGarbageCollection > maxTimeBetweenGarbageCollections)
        {
            CollectGarbage();
        }
    }

    private void CollectGarbage()
    {
        _timeSinceLastGarbageCollection = 0f;
        //Debug.Log("Collecting garbage"); // talking about garbage... 
#if !UNITY_EDITOR
    // Not supported on the editor
    GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
    GC.Collect();
    GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
#endif
    }
}
