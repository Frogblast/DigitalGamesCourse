using UnityEngine;
using System;

// Manages sound through the inspector. Sounds that normaly don't live on an object, can easily be played through this
public class AudioManager : MonoBehaviour
{

    public Sound[] sounds; // list of all added sounds

    public static AudioManager Instance; // Only one instance of AudioManager should be present

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes sure the gameobject stays between levels
        }
        else
        {
            Destroy(gameObject);
            return; // Prevent any more code from running in awake
        }


        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>(); // Adds an audiosource to the gameobject and also connects the variable "source" with the added audiosource
            // Populates the new audiosource with the clip,volume,pitch,etc from the element s in sounds
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }


    public void Play(string name) // Finds the given name of audio object and plays it
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // Basically a for-loop and if-statement, but in one line. We are looking for an object in array where object.name == name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " does not exist!");
            return;
        }

        s.source.Play();
    }


    private void Start()
    {
       Play("ambianceSoundSource");
    }
}
