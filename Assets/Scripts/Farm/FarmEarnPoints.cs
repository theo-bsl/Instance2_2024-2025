using Player;
using UnityEngine;

namespace Farm
{
    public class FarmEarnPoints : MonoBehaviour
    {
        [SerializeField] private PlayerManager _playerEarningPoints;
        [SerializeField] private float _earningDelay;
        [SerializeField] private int _pointsAmount;
        private float _timer;

        private void Update()
        {
            if (_playerEarningPoints)
            {
                _timer += Time.deltaTime;
                if (_timer > _earningDelay)
                {
                    _timer = 0;
                    _playerEarningPoints?.IncreaseScore(_pointsAmount);
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