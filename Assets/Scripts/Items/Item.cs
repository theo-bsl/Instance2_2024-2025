using Unity.Netcode;
using UnityEngine.Events;

namespace Items
{
    public abstract class Item : NetworkBehaviour
    {
        protected readonly UnityEvent<object> _onDo = new();
        public abstract void Do();
        
        public UnityEvent<object> OnDo => _onDo;
    }
}

