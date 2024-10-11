using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    public int MaxHp() { return maxHp; }
    
    [SerializeField] private Slider hpSlider = null;
    
    private bool _isDead = false;
    public bool IsDead() { return _isDead; }
    
    private int _currentHp;
    public int CurrentHp() { return _currentHp;}
    public float GetHpPercentage() {return _currentHp/(float)maxHp; }

    public event Action OnDeath;
    public event Action<float,Health> OnHit;
    
    public void AddDamage(int amount)
    {
        _currentHp -= amount;
        if (_currentHp <= 0)
        {
            _isDead = true;
            _currentHp = 0;
            OnDeath?.Invoke();
        }
        else if(_currentHp > maxHp)
        {
            _currentHp = maxHp;
            OnHit?.Invoke(GetHpPercentage(),this);
        }
        else
        {
            OnHit?.Invoke(GetHpPercentage(),this);
        }
        
        if (hpSlider != null)
        {
            hpSlider.value = _currentHp;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentHp = maxHp;
        
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = _currentHp;
        }
    }
}
