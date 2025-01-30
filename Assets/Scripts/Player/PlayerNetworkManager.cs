using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsOwner || IsServer)
            {
                GetComponent<PlayerInput>().enabled = false;
                GetComponentInChildren<Camera>().gameObject.SetActive(false);
            }

            if (!IsServer)
            {
                GetComponentInChildren<PlayerManager>().enabled = false;
                GetComponentInChildren<PlayerMovement>().enabled = false;
                GetComponentInChildren<PlayerRotation>().enabled = false;
                GetComponentInChildren<PlayerAttack>().enabled = false;
                
            }
        }
    }
}