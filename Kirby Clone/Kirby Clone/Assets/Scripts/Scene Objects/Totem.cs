using UnityEngine;

public class Totem : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private int sceneToLoad;    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            sceneLoader.LoadScene(sceneToLoad);
        }
    }
}
