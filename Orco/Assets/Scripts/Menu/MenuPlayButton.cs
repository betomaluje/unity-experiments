using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuPlayButton : MonoBehaviour
{
    public void OnSubmit()
    {
        if (MenuPlayerSelected.numberOfPlayers == 0)
        {
            Debug.Log("Not enough players!");
            return;
        }
        
        MenuPlayerSettings.SaveNumberOfPlayers(MenuPlayerSelected.numberOfPlayers);

        int randomScene = Random.Range(1, SceneManager.sceneCount - 1);

        Debug.Log("playing with " + MenuPlayerSelected.numberOfPlayers + " players on Level " + randomScene);

        SceneManager.LoadScene(randomScene, LoadSceneMode.Single);
    }
}
