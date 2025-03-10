using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;

    public int CurrentHealth;

    private Character _cc;

    public float currentHealthPercentage=1.0f;
    //public float CurrentHealthPercentage
    //{
    //    get
    //    {

    //        return currentHealthPercentage;
    //    }
    //    set
    //    {
    //        currentHealthPercentage = (float)CurrentHealth/(float)MaxHealth;

    //    }
    //}
    public float CurrentHealthPercentage
    {
        get
        {
            return (float)CurrentHealth / (float)MaxHealth;
        }
    }

    private void Awake()
    {
        _cc = GetComponent<Character>();
        
        CurrentHealth = MaxHealth;
    }

    public void ApplyDamage(int damage)
    {

        CurrentHealth -= damage;
        CheckHealth();


    }
    public void CheckHealth()
    {
        if (CurrentHealth<=0)
        {
            _cc.SwitchStateTo(Character.CharacterState.Dead);
        }
    
    }

    public void AddHealth(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth>MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    
    }
}
