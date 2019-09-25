using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator animator;    

    private int nextSceneIndex;

    public void LoadScene(int scene) {
        nextSceneIndex = scene;
        animator.SetTrigger("FadeOut");    
    }

    public void LoadNextScene() {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadOptions() {

    }

    public void OnFadeComplete() {
        SceneManager.LoadScene(nextSceneIndex);
    }
}
