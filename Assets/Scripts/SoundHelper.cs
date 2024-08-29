using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHelper : MonoBehaviour
{
    public void soundPlay()
    {
        SoundManager.instance.SoundPlay();
    }
}
