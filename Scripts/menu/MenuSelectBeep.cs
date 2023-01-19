using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectBeep : MonoBehaviour
{
    private AudioSource beepSource;
    public AudioClip beepSound;
    //public AudioClip enterSound;
    // Start is called before the first frame update
    void Start()
    {
        beepSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            beepSource.PlayOneShot(beepSound, 1.0f);
        }
    }
}
