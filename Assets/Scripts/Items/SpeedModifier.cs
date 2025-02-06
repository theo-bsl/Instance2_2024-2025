using UnityEngine;

namespace Items
{
    public class SpeedModifier : Item
    {
        [SerializeField, Range(-1f, 1f)] private float _speedModifier = 0.5f;
        [SerializeField] private float _duration = 10;

        public override void Do()
        {
            _onDo.Invoke((_speedModifier, _duration));
        }
    }
}
