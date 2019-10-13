using UnityEngine;

public class MenuClickMapper : MonoBehaviour
{
    [SerializeField] private GameEvent playClicked;
    [SerializeField] private GameEvent optionsClicked;

    public void OnPlayClicked() {
        playClicked.Raise();
    }

    public void OnOptionsClicked() {
        optionsClicked.Raise();
    }
}
