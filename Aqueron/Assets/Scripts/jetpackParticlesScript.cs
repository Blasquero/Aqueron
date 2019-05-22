using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpackParticlesScript : MonoBehaviour
{
    [SerializeField] private GameObject jetpack;
    private ParticleSystem jetpackParticles;
    private bool jetpackParticlesActivated;

    void Start()
    {
        jetpackParticles = gameObject.GetComponent<ParticleSystem>();
        jetpackParticles.Stop();
        jetpackParticlesActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jetpack.activeSelf == true && jetpackParticlesActivated == false) {
            jetpackParticles.Play();
            jetpackParticlesActivated = true;
        } else if (jetpack.activeSelf == false && jetpackParticlesActivated == true) {
            jetpackParticles.Stop();
            jetpackParticlesActivated = false;
        }
    }
}
