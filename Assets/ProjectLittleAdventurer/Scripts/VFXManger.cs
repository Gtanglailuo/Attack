using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManger : MonoBehaviour
{
    public VisualEffect footStep;

    public VisualEffect Slash;
    public ParticleSystem Blade01;
    public ParticleSystem Blade02;
    public ParticleSystem Blade03;
    public VisualEffect Heal;




    public void Update_FootStep(bool state)
    {
        if (state)
        {
            footStep.Play();
        }
        else
        {
            footStep.Stop();
        }
    
    }

    public void PlayerBlad01()
    {
        Blade01.Play();
    }
    public void PlayBlade02()
    {
        Blade02.Play();
    }
    public void PlayBlade03()
    {
        Blade03.Play();
    }

    public void StopBlade()
    {
        Blade01.Simulate(0);
        Blade01.Stop();

        Blade02.Simulate(0);
        Blade02.Stop();

        Blade03.Simulate(0);
        Blade03.Stop();

    }


    public void PlaySlash(Vector3 pos)
    {
        Slash.transform.position = pos;

        Slash.Play();

    }

    public void PlayHealVFX()
    {

        Heal.Play();

    }


}
