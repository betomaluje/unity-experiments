using UnityEngine;

public class SceneLoadSound : MonoBehaviour
{
    [SerializeField] private string bgMusic;

    private void Start()
    {
        SoundManager.instance.Play(bgMusic);
    }
}
