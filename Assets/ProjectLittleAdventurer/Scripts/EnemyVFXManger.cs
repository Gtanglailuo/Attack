using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManger : MonoBehaviour
{
    public VisualEffect FootStep;


    public VisualEffect AttackVFX;


    public ParticleSystem BeingHitVFX;

    public VisualEffect BeingHitSplashVFX;
    public void PlayerAttackVFX()
    {
        AttackVFX.Play();
    }
    public void BurstFootStep()
    {
        FootStep.SendEvent("OnPlay");
    }
    //����������Ч�ķ���
    public void PlayBeingHitVFX(Vector3 attackPos)
    {
        Vector3 forceForward = transform.position - attackPos;
        forceForward.Normalize();
        forceForward.y = 0;
        BeingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
        BeingHitVFX.Play();
        //Ѫ����Ч��
        Vector3 splashPos = transform.position;
        splashPos.y += 2f;
        VisualEffect newSplashVFX = Instantiate(BeingHitSplashVFX,splashPos,Quaternion.identity);
        newSplashVFX.SendEvent("OnPlay");
        Destroy(newSplashVFX.gameObject,1.0f);
    }


}
