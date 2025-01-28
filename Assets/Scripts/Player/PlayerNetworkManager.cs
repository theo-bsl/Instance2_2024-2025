using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                GetComponent<PlayerInput>().enabled = false;
                GetComponentInChildren<Camera>().gameObject.SetActive(false);
            }
        }
    }
}