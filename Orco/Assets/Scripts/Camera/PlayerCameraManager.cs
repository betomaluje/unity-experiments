using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float weight;
    
    private List<CinemachineTargetGroup.Target> allTargets;

    private void Start()
    {
        allTargets = new List<CinemachineTargetGroup.Target>();
    }

    public void OnPlayerJoined(PlayerInput player)
    {        
        CinemachineTargetGroup.Target target;
        target.target = player.transform;
        target.weight = weight;
        target.radius = 0;

        // we add it to the list
        allTargets.Insert(player.playerIndex, target);

        targetGroup.m_Targets = allTargets.ToArray();
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        Debug.Log("player left! " + player.playerIndex);

        allTargets.RemoveAt(player.playerIndex);
        targetGroup.m_Targets = allTargets.ToArray();
    }    
}
