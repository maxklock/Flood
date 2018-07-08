using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWin : MonoBehaviour
{
    private ParticleSystem.EmissionModule em;
    void Start()
    {
        em = GetComponent<ParticleSystem>().emission;
        em.enabled = false;
    }
    public void StartParticle()
    {
        em.enabled = true;
    }
    public void StopParticle()
    {
        em.enabled = false;
    }

}
