using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChange : MonoBehaviour
{
    public AudioClip regular, boss;
    // Start is called before the first frame update
    public void Change(bool f = false)
    {
        if (f) GetComponent<AudioSource>().clip = boss;
        else GetComponent<AudioSource>().clip = regular;
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
