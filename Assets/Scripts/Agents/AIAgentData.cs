using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIAgentData", menuName = "ScriptableObjects/AIAgentData", order = 1)]

public class AIAgentData : ScriptableObject
{
    public struct RoleDataPercentages
    {
        public float Defend;
        public float Attack;
        public float Heal;
    }

    public RoleDataPercentages q;

}
