using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMap : MonoBehaviour
{
    private List<Transform> players;

    private void Start()
    {
        players = new List<Transform>();
    }

    private void LateUpdate()
    {
        foreach (var player in players)
        {
            Vector3 newPosition = player.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }

    public void AddPlayerToMiniMap(PlayerInput player)
    {
        players.Add(player.transform);
    }

    public void RemovePlayerToMiniMap(PlayerInput player)
    {
        players.Remove(player.transform);
    }
}
