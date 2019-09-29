using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private bool doesIncludePlayer;

    [SerializeField] private Texture2D map;
    //public CinemachineVirtualCamera vcam;

    [SerializeField] private float displacement = 3.5f;

    [SerializeField] private ColorToPrefab[] colorMappings;

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
            if (colorMapping != null && colorMapping.color.Equals(pixelColor))
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
                
                if (doesIncludePlayer && theNewObject.CompareTag("Player")) {
                    PlayerFirstPosition script = GetComponent<PlayerFirstPosition>();
                    script.SetFollowCamera(theNewObject.transform);

                    PlayerStats playerStats = theNewObject.GetComponent<PlayerStats>();
                    playerStats.SetPlayersPosition(theNewObject.transform.position);
                }
                
                break;
            }
        }
    }
}
