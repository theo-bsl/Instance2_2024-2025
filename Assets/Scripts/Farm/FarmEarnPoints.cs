using Player;
using UnityEngine;

namespace Farm
{
    public class FarmEarnPoints : MonoBehaviour
    {
        [SerializeField] private float _earningDelay;
        [SerializeField] private int _pointsAmount;
        private float _timer;
        private PlayerManager _playerEarningPoints;

        private void FixedUpdate()
        {
            if (_playerEarningPoints)
            {
                _timer += Time.deltaTime;
                if (_timer > _earningDelay)
                {
                    _timer = 0;
                    _playerEarningPoints?.IncreaseScoreRPC(_pointsAmount);
                }
            }
            else
            {
                _timer = 0;
            }
        }
        
        
        public PlayerManager PlayerEarningPoints { get => _playerEarningPoints; set => _playerEarningPoints = value; }
    }
}