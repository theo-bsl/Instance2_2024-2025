using Unity.Netcode;

namespace Farm
{
    public class FarmNetworkManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                GetComponent<FarmEarnPoints>().enabled = false;
                GetComponent<FarmZone>().enabled = false;
            }
        }
    }
}