using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    public CinemachineConfiner confiner;

    // Start is called before the first frame update
    public void ActivateConfiner() {
        confiner.enabled = true;
    }

    public void DectivateConfiner() {
        confiner.enabled = false;
    }
}
