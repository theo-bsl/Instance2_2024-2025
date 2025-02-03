using System.Collections.Generic;
using Player;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

namespace Lobby
{
    public class LobbyGame : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ManageNewPlayer;
        }

        private void ManageNewPlayer(ulong id)
        {
            var player = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            PlayerManager manager = TryGetComponentInChildren<PlayerManager>(player.transform);
            PlayerAttack attack = TryGetComponentInChildren<PlayerAttack>(player.transform);
            
            
        }

        private T TryGetComponentInChildren<T>(Transform playerTransform)
        {
            Transform[] transforms = playerTransform.GetComponentsInChildren<Transform>();
            foreach (Transform childTransform in transforms)
            {
                if (childTransform.TryGetComponent(out T component))
                    return component;
            }

            return default;
        }
    }
}