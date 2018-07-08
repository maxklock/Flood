using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWin : MonoBehaviour
{
    private ParticleSystem.EmissionModule em;
    private AudioSource _audio;

    public float Volume = 2f;
    void Start()
    {
        em = GetComponent<ParticleSystem>().emission;
        _audio = GetComponent<AudioSource>();
        _audio.volume = Volume;
        em.enabled = false;
    }
    public void StartParticle()
    {
        em.enabled = true;
        _audio.Play();
    }
    public void StopParticle()
    {
        em.enabled = false;
        _audio.Stop();
        
    }

}
