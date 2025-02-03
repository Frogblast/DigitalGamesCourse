using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource walkingAudioSource;
    [SerializeField]
    private AudioSource jumpingAudioSource;

    public void PlayWalkSound()
    {
        if(!walkingAudioSource.isPlaying)
            walkingAudioSource.Play();
    }

    public void PlayJumpSound()
    {
        if (!jumpingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
            jumpingAudioSource.Play();
        }
    }
}
