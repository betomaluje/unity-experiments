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

        Debug.Log("playing with " + MenuPlayerSelected.numberOfPlayers + " players");
        MenuPlayerSettings.SaveNumberOfPlayers(MenuPlayerSelected.numberOfPlayers);
        SceneManager.LoadScene(MenuConstants.SCENE_LEVEL_2, LoadSceneMode.Single);
    }
}
