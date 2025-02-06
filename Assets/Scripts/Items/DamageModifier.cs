using UnityEngine;

namespace Items
{
    public class DamageModifier : Item
    {
        [SerializeField, Range(-1f, 1f)] private float _damageModifier = 0.2f;
        [SerializeField] private float _duration = 5;
        
        public override void Do()
        {
            _onDo.Invoke((_damageModifier, _duration));

        }
    }
}