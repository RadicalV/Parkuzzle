using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<PlayerManager>(out PlayerManager player);

        if (player != null)
            player.Respawn();
    }
}
