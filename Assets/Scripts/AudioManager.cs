using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource ambianceSoundSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (ambianceSoundSource == null)
        {
            ambianceSoundSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (!ambianceSoundSource.isPlaying)
        {
            ambianceSoundSource.Play();
        }
    }
}
