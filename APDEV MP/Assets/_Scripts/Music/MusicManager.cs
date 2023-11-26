using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private AudioSource BGM;

    [SerializeField]
    private AudioClip AreaBgm;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    public void ChangeBGM(AudioClip music)
    {
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
    public void RevertBGM()
    {
        BGM.Stop();
        BGM.clip = this.AreaBgm;
        BGM.Play();

    }
}
