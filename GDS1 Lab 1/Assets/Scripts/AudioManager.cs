using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SoldierPickup;
    [SerializeField] AudioSource SoldierDropOff;
    [SerializeField] AudioSource Refuel;
    [SerializeField] AudioSource Crash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoldierPickup()
    {
        SoldierPickup.Play();
    }

    public void PlaySoldierDropOff()
    {
        SoldierDropOff.Play();
    }

    public void PlayRefuel()
    {
        Refuel.Play();
    }

    public void PlayCrash()
    {
        Crash.Play();
    }
}
