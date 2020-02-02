using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] breakEffects;
    public AudioClip[] fixEffects;
    void Start()
    {
        //musics = new AudioClip[4] [Audio1, Audio2, Audio3, Audio4];
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public AudioClip GetBreakingSound() {
        AudioClip RandomSound = breakEffects[Random.Range(0, breakEffects.Length)];
        return RandomSound;
    }
    public AudioClip GetFixingSound() {
        AudioClip RandomSound = fixEffects[Random.Range(0, fixEffects.Length)];
        return RandomSound;
    }
}
