using UnityEngine;

namespace Sound
{
    public class PlaySoundEnter : StateMachineBehaviour
    {
        [SerializeField] private SoundType sound;
        [SerializeField, Range(0, 1)] private float volume = 1.0f;
        [SerializeField] private bool playAtPoint;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            /*if (playAtPoint)
                SoundManager.Instance.PlaySoundAtPosition(sound, animator.transform.position, volume);
            else 
                SoundManager.Instance.PlaySound(sound, volume);*/
        }
    }
}