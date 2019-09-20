using UnityEngine;
using Cinemachine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;
    //public CinemachineVirtualCamera vcam;

    public float displacement = 3.5f;

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
                Vector2 position = new Vector2(x - displacement, y - displacement);
                GameObject prefabObject = colorMapping.prefab;

                if (prefabObject == null)
                {
                    break;
                }

                GameObject theNewObject = Instantiate(prefabObject, transform.position, Quaternion.identity, transform);
                theNewObject.transform.localPosition = position;

                SpriteRenderer sr = theNewObject.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    sr.sortingOrder = colorMapping.orderInLayer;
                }

                if (theNewObject.CompareTag("Player"))
                {
                    // need to move the proper gameobjects already instantiated instead of instantiate them again                    
                    //vcam.Follow = theNewObject.gameObject.transform;

                    PlayerStats ps = theNewObject.GetComponent<PlayerStats>();
                    ps.SetPlayersPosition(position);
                }

                break;
            }
        }
    }
}
