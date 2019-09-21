using UnityEngine;
using Cinemachine;

public class PlayerFirstPosition : MonoBehaviour
{
    public GameObject player;
    public CinemachineVirtualCamera vcam;

    public void SetPlayerInitialPosition(Vector3 position)
    {
        GameObject p = Instantiate(player, position, Quaternion.identity);
        vcam.Follow = p.gameObject.transform;
    }
}
