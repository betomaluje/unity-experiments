using UnityEngine;
using Cinemachine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;
    public CinemachineVirtualCamera vcam;

    public ColorToPrefab[] colorMappings;    

    // Use this for initialization
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            // The pixel is transparrent. Let's ignore it!
            return;
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x, y);
                GameObject theNewObject = colorMapping.prefab;

                if (theNewObject == null)
                {
                    break;
                }

                SpriteRenderer sr = Instantiate(theNewObject, position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = colorMapping.orderInLayer;
                }

                if (theNewObject.CompareTag("Player"))
                {
                    // need to move the proper gameobjects already instantiated instead of instantiate them again                    
                    vcam.Follow = sr.gameObject.transform;
                }

                break;
            }
        }
    }
}
