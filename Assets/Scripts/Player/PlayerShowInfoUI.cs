using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerShowInfoUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _playerNameInfo;
        [SerializeField] TextMeshProUGUI _playerDamageInfo;
        
        public void SetName(string name)
        {
            _playerNameInfo.SetText(name);
        }

        public void SetDamage(float damage)
        {
            _playerDamageInfo.SetText($"{damage.ToString()} %");
        }
    }
}