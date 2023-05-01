using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Restart : NetworkBehaviour
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

            player.RestartClientRpc(clientRpcParams);
        }
    }
}
