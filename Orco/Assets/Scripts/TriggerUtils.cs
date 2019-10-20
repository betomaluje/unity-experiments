using UnityEngine;

public class TriggerUtils
{
    public static bool CheckLayerMask(LayerMask layer, GameObject target)
    {
        return (layer & 1 << target.layer) == 1 << target.layer;
    }
}
