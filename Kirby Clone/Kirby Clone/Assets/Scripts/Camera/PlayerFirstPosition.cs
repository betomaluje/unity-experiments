using UnityEngine;
using Cinemachine;

public class PlayerFirstPosition : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineVirtualCamera vcam;

    public void SetPlayerInitialPosition(Vector3 position)
    {
        GameObject p = Instantiate(player, position, Quaternion.identity);    
        SetFollowCamera(p.gameObject.transform);
    }

    public void SetFollowCamera(Transform t) {
        vcam.Follow = t;
    }
}
