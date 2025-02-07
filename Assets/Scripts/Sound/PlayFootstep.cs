using UnityEngine;

namespace Sound
{
    public class PlayFootstep : MonoBehaviour
    {
        public void PlaySound()
        {
            SoundManager.PlaySound(SoundType.Footstep);
        }
    }
}