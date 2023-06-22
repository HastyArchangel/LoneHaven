using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public Sound [] sounds;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void PlayCardPickup()
    {
        //Array.Find(sounds, sound => sound.name == "CardPickup").source.Play();
        sounds[0].source.Play();
    }

    public void PlayCardPutDown()
    {
        //Array.Find(sounds, sound => sound.name == "CardPickup").source.Play();
        sounds[1].source.Play();
    }

    public void PlayError()
    {
        int random = (int)Random.Range(0, 2) + 2;
        sounds[random].source.Play();
    }
}
