using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIAgentData", menuName = "ScriptableObjects/AIAgentData", order = 1)]

public class AIAgentData : ScriptableObject
{
    public float SupportFirePercentage;
    public float CoveringFirePercentage;
    public float HealPlayerPercentage;
    public float ProtectPlayerPercentage;
}
