using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class KeyCombo
{
    public new string name;
    public string[] buttons;    
    public float allowedTimeBetweenButtons = 0.3f; //tweak as needed

    [Header("Special attack")]
    public UnityEvent specialAttack;

    private int currentIndex = 0; //moves along the array as buttons are pressed
    private float timeLastButtonPressed;

    public KeyCombo(string[] b)
    {
        buttons = b;
    }

    //usage: call this once a frame. when the combo has been completed, it will return true
    public bool Check(Player player)
    {
        if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
        {
            if (currentIndex < buttons.Length)
            {
                if ((buttons[currentIndex] == "down" && GetVerticalAxis(player.verticalInput) == -1) ||
                (buttons[currentIndex] == "up" && GetVerticalAxis(player.verticalInput) == 1) ||
                (buttons[currentIndex] == "left" && GetHorizontalAxis(player.horizontalInput) == -1) ||
                (buttons[currentIndex] == "right" && GetHorizontalAxis(player.horizontalInput) == 1) ||
                (buttons[currentIndex] != "down" && 
                buttons[currentIndex] != "up" && 
                buttons[currentIndex] != "left" && 
                buttons[currentIndex] != "right" && 
                Input.GetButtonDown(buttons[currentIndex])))
                {
                    timeLastButtonPressed = Time.time;
                    currentIndex++;
                }

                if (currentIndex >= buttons.Length)
                {
                    currentIndex = 0;

                    if (specialAttack != null)
                    {
                        Debug.Log(player.name + " special combo " + name);
                        specialAttack.Invoke();
                    }

                    return true;
                }
                else return false;
            }
        }
        return false;
    }

    private float GetVerticalAxis(string verticalInput)
    {
        return Input.GetAxisRaw(verticalInput);
    }

    private float GetHorizontalAxis(string horizontalInput)
    {
        return Input.GetAxisRaw(horizontalInput);
    }
}