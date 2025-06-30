using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("AudioClip")]
    public AudioClip background;
    public AudioClip Shoot;
    public AudioClip Explosion;


    private void Start() //La música de fondo sonara al iniciar la escena
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip) //Para acceder a este parametro desde otros scripts
    {
        SFXSource.PlayOneShot(clip);
    }
    

}
