using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource walkingAudioSource;
    [SerializeField]
    private AudioSource jumpingAudioSource;

    private bool canPlayJumpSound = true;

    public void PlayWalkSound()
    {
        if (!walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }
        canPlayJumpSound = true;
    }

    public void PlayJumpSound()
    {
        if (!canPlayJumpSound) return;

        if (!jumpingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
            jumpingAudioSource.Play();
        }
        canPlayJumpSound=false;
    }
}
