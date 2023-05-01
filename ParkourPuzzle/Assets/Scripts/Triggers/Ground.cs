using Unity.Netcode;
using UnityEngine;

public class Ground : NetworkBehaviour
{
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

            player.RespawnPlayerClientRpc(clientRpcParams);
        }
    }
}