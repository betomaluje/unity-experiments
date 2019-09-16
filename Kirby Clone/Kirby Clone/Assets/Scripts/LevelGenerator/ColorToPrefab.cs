using UnityEngine;

[CreateAssetMenu(fileName = "New Color Mapping", menuName = "Color Mapping")]
public class ColorToPrefab : ScriptableObject
{
    public Color color;
    public GameObject prefab;
    public int orderInLayer = 0;
}
