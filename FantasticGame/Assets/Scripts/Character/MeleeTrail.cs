using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem meleeParticleRenderer;


    private Player p1;
    void Awake()
    {
        p1 = FindObjectOfType<Player>();

        meleeParticleRenderer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (p1.Attacking)
        {
            meleeParticleRenderer.Play();
        }

        if (p1.trailCurrentPos < 0.3f)
        {
            meleeParticleRenderer.Stop();
        }
    }
}
