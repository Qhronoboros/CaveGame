using UnityEngine;

public static class LayerHelper
{
    // Returns whether the given layer is included in the layerMask
    public static bool IsInLayerMask(LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
