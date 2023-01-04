using RibynsModules;
using System;
using Unity.Netcode;
using UnityEngine;

public sealed class NetworkPosition : NetworkBehaviour
{
    //readonly NetworkVariable<Vector2> _networkPosition = new NetworkVariable<Vector2>(
    //    default,
    //    NetworkVariableReadPermission.Everyone,
    //    NetworkVariableWritePermission.Owner);

    //public override void OnNetworkSpawn() // having this public is, yet another reason to move it out of this class.
    //{
    //    _networkPosition.OnValueChanged += RefreshPosition;

    //    gameObject.name += NetworkManager.Singleton.IsHost ? "Host" : "Client";
    //    this.Log("OnNetworkSpawn");
    //}

    //void RefreshPosition(Vector2 previousValue, Vector2 newValue)
    //{
    //    if (NetworkObject.IsOwner()) // owner has the change already, before he sent it.
    //        return;
    //    Debug.Log($"Refreshing {name} position!");

    //    // Refresh on Server, and Clients 
    //    transform.position = newValue;
    //}

    //public override void OnNetworkDespawn()
    //{
    //    _networkPosition.OnValueChanged -= RefreshPosition;
    //}

    //public void Refresh(Vector2 position)
    //{
    //    if (IsSpawned)
    //    {
    //        _networkPosition.Value = position;
    //    }
    //}

    NetworkVariable<Vector2> _serverPosition = new NetworkVariable<Vector2>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        Debug.Log($"On Network spawn");
        if (IsOwner)
        {
            _serverPosition.Value = default;
            Debug.Log($"{nameof(_serverPosition)} initialized to: {_serverPosition.Value}");
        }
    }

    public void RefreshPosition(Vector2 newPosition)
    {
        if (IsOwner)
        {
            _serverPosition.Value = newPosition;
            Debug.Log($"{nameof(_serverPosition)} updated to: {_serverPosition.Value}");
        }
    }
}
