using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Transform[] players;

    private void Start() {
        GameObject[] ps = GameObject.FindGameObjectsWithTag("Player");

        players = new Transform[ps.Length];

        int i = 0;
        foreach (var p in ps)
        {
            players[i] = p.transform;
            i++;
        }
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
}
