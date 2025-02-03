using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.Netcode;

namespace Leaderboard
{
    public class Leaderboard : NetworkBehaviour
    {
        private readonly List<PlayerManager> _playerInLeaderboard = new();
        private readonly List<PlayerManager> _topPlayer = new();
        public int _nbPlayerInLeaderboard = 5;
        
        public void LeaderBoardUpdate()
        {
            var r = _playerInLeaderboard.OrderBy(x => x.Score).Take(_nbPlayerInLeaderboard).ToList();
        }

        public void AddNewPlayer(PlayerManager manager, PlayerAttack attack)
        {
            manager.Score.OnValueChanged += (value, newValue) => LeaderBoardUpdate();
            attack.OnEnemyBursted.AddListener(LeaderBoardUpdate);
        }
        
        public List<PlayerManager> TopPlayer => _topPlayer;
    }
}