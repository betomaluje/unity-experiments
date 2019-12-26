using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonSelectable : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
