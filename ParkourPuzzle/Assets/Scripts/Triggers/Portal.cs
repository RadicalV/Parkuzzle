using Unity.Netcode;
using UnityEngine;

public class Portal : NetworkBehaviour
{
    [SerializeField]
    private Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        other.TryGetComponent<PlayerManager>(out PlayerManager player);

        if (player != null)
        {
            ulong clientId = player.gameObject.GetComponent<NetworkObject>().OwnerClientId;
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            player.SetRespawnDataClientRpc(destination.position, destination.localRotation, clientRpcParams);
            player.SetPlayerRotationClientRpc(destination.localRotation.eulerAngles.y, clientRpcParams);

            float angle = destination.rotation.eulerAngles.y * Mathf.Deg2Rad;

            player.TeleportPlayerClientRpc(angle, clientRpcParams);
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