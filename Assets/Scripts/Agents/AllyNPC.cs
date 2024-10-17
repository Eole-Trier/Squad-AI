using FSMMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AllyNPC : MonoBehaviour 
{
    [SerializeField]
    int MaxHP = 100;
    [SerializeField]
    float BulletPower = 1000f;
    [SerializeField]
    GameObject BulletPrefab;

    [SerializeField]
    Slider HPSlider = null;

    [SerializeField] private bool isDefend = false;

    [SerializeField] private AIAgent m_AiAgentinst;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            isDefend = true;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isDefend)
        {
            //m_AiAgentinst.DefenseFormation();
        }
    }
}
