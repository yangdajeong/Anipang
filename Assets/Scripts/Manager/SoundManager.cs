using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ui;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null ) 
        {
            instance = this;    
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SoundPlay()
    {
        ui.Play();
    }
}
