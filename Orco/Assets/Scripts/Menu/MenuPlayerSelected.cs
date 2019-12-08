using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPlayerSelected : MonoBehaviour
{   
    [SerializeField] private Button defaultButton;

    private void Awake()
    {
        defaultButton.Select();
    }

    public void SelectNumberOfPlayers(int players)
    {
        Debug.Log(players + " selected!");
        MenuPlayerSettings.SaveNumberOfPlayers(players);
        SceneManager.LoadScene(MenuConstants.SCENE_LEVEL_1, LoadSceneMode.Single);
    }
}
