using Unity.Netcode;

namespace Leaderboard
{
    public class LeaderboardNetworkManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                GetComponent<LeaderboardManager>().enabled = false;
            }
        }
    }
}