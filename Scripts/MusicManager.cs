using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource musicPlayer;
    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicPlayer.volume = PlayerPrefs.GetFloat("musicVolume");
        } else
        {
            musicPlayer.volume = 0.15f;
        }
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleMusic()
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.Pause();
        } else
        {
            musicPlayer.Play();
        }
    }
}
