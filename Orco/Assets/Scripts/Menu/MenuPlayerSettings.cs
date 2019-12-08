using UnityEngine;

public class MenuPlayerSettings
{    
    public static void SaveNumberOfPlayers(int players)
    {
        PlayerPrefs.SetInt(MenuConstants.KEY_NUM_PLAYERS, players);
    }

    public static int GetNumberOfPlayers()
    {
        return PlayerPrefs.GetInt(MenuConstants.KEY_NUM_PLAYERS, 1);
    }
}
