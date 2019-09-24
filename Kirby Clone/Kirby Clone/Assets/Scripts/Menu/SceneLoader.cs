using UnityEngine;
//using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameEvent playClicked;

    public void LoadScene(int scene) {
        //SceneManager.LoadScene(scene);
        playClicked.Raise();
    }

    public void LoadOptions() {

    }
}
