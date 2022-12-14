using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Sword Slash")]
    public List<ParticleSystem> swordSlash = new List<ParticleSystem>();

    public void PlayWeaponFX(int n)
    {
        swordSlash[n].Play();
    }
}
    