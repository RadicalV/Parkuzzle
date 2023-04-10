using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Transform destination;
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<PlayerManager>(out PlayerManager player);

        if (player)
        {
            player.SetRespawnData(destination.position, destination.localRotation);

            Vector3 currentVelocity = player.moveData.velocity;
            player.moveData.origin = destination.transform.position;
            player.SetPlayerRotation(destination.localRotation.eulerAngles.y);

            float angle = destination.rotation.eulerAngles.y * Mathf.Deg2Rad;
            Vector3 newVelocity = new Vector3(Mathf.Sin(angle), 0f,
            Mathf.Cos(angle)) * Mathf.Sqrt(player.moveData.velocity.x * player.moveData.velocity.x + player.moveData.velocity.z * player.moveData.velocity.z);

            player.moveData.velocity = newVelocity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(destination.position, 0.4f);
        var direction = destination.TransformDirection(Vector3.forward);
        Gizmos.DrawRay(destination.position, direction);
    }
}
