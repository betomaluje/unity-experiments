using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MenuPlayerSelected : MonoBehaviour
{
    [SerializeField] private TMP_Text playersText;

    public static int numberOfPlayers = 0;   

    private void LateUpdate()
    {
        playersText.text = numberOfPlayers + " Players"; 
    }

    public void OnPlayerJoined(PlayerInput player)
    {        
        numberOfPlayers++;
        Debug.Log("player " + player.playerIndex + " joined! total: " + numberOfPlayers);        
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        numberOfPlayers--;
        Debug.Log("player " + player.playerIndex + " left! total: " + numberOfPlayers);
    }   

}
