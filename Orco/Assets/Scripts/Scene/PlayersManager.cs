using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayersManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private bool isDarkLevel = true;

    [Space]
    [Header("Camera Settings")]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float cameraWeight;
    [SerializeField] private Camera miniMapCamera;

    [Space]
    [Header("UI")]
    [SerializeField] private GameObject[] playerStatuses;
    
    private List<CinemachineTargetGroup.Target> allTargets;
    private List<Transform> players;

    private void Start()
    {
        allTargets = new List<CinemachineTargetGroup.Target>();
        players = new List<Transform>();
    }

    private void LateUpdate()
    {
        foreach (var player in players)
        {
            Vector3 newPosition = player.position;
            newPosition.z = miniMapCamera.transform.position.z;
            miniMapCamera.transform.position = newPosition;
        }       
    }

    public void OnPlayerJoined(PlayerInput player)
    {        
        Debug.Log("player "+ player.playerIndex + " joined!");

        if (player.gameObject.CompareTag("Player")) 
        {
            AddTargetToMainCamera(player);
            AddPlayerToMiniMap(player);
            ConfigureLevelLights(player);

            // we show the player panel on the UI
            playerStatuses[player.playerIndex].SetActive(true);
        }
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        Debug.Log("player " + player.playerIndex + " left!");

        if (player.gameObject.CompareTag("Player")) 
        {
            RemoveTargetFromMainCamera(player);
            RemovePlayerToMiniMap(player);
        }
    }

    #region Mini Map
    private void AddPlayerToMiniMap(PlayerInput player)
    {
        players.Add(player.transform);
    }

    private void RemovePlayerToMiniMap(PlayerInput player)
    {
        players.Remove(player.transform);
    }
    #endregion

    #region Main Camera
    private void AddTargetToMainCamera(PlayerInput player)
    {
        CinemachineTargetGroup.Target target;
        target.target = player.gameObject.transform;
        target.weight = cameraWeight;
        target.radius = 0;

        // we add it to the list
        allTargets.Insert(player.playerIndex, target);

        targetGroup.m_Targets = allTargets.ToArray();
    }

    private void RemoveTargetFromMainCamera(PlayerInput player)
    {
        allTargets.RemoveAt(player.playerIndex);
        targetGroup.m_Targets = allTargets.ToArray();
    }
    #endregion

    #region Player UI
    public void AddPlayerScore(AttackEvent attackEvent)
    {
        int i = 0;
        foreach (var t in players)
        {
            if (attackEvent.target == t.gameObject)
            {
                PlayerScore playerScore = playerStatuses[i].GetComponent<PlayerScore>();

                if (playerScore != null)
                {
                    playerScore.AddScore();
                }

                return;
            }

            i++;
        }
    }

    public void ApplyPlayerDamage(AttackEvent attackEvent)
    {
        Debug.Log("damage searching for " + attackEvent.target.name);

        int i = 0;
        foreach (var t in players)
        {
            if (attackEvent.target == t.gameObject)
            {
                PlayerScore playerScore = playerStatuses[i].GetComponent<PlayerScore>();

                if (playerScore != null)
                {
                    Debug.Log(attackEvent.target.name + " found!");
                    playerScore.PlayerDamage();
                }

                return;
            }

            i++;
        }
    }

    public void ApplyPlayerHealth(AttackEvent healthEvent)
    {
        Debug.Log("health searching for " + healthEvent.target.name);

        int i = 0;
        foreach (var t in players)
        {
            if (healthEvent.target == t.gameObject)
            {
                PlayerScore playerScore = playerStatuses[i].GetComponent<PlayerScore>();

                if (playerScore != null)
                {
                    Debug.Log(healthEvent.target.name + " found!");
                    playerScore.PlayerHealth();
                }

                return;
            }

            i++;
        }
    }
    #endregion

    #region Level Settings
    private void ConfigureLevelLights(PlayerInput player) {
        Light2D lights = player.GetComponentInChildren<Light2D>();    
        if (lights != null) 
        {
            lights.enabled = isDarkLevel;
        }
    }
    #endregion
}
