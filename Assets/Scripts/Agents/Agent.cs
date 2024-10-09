using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected int MaxHP = 100;
    [SerializeField]
    protected float BulletPower = 1000f;
    [SerializeField]
    protected GameObject BulletPrefab;
    
    [SerializeField]
    Slider HPSlider = null;
    
    protected Rigidbody rb;
    protected GameObject TargetCursor = null;
    protected GameObject NPCTargetCursor = null;
    protected Transform GunTransform;
    protected bool IsDead = false;
    int CurrentHP;
    
    public void AddDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            IsDead = true;
            CurrentHP = 0;
        }
        if (HPSlider != null)
        {
            HPSlider.value = CurrentHP;
        }
    }

    protected void Start()
    {
        
        CurrentHP = MaxHP;
        GunTransform = transform.Find("Gun");
        rb = GetComponent<Rigidbody>();

        if (HPSlider != null)
        {
            HPSlider.maxValue = MaxHP;
            HPSlider.value = CurrentHP;
        }
    }
}
