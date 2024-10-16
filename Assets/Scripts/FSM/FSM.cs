using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private delegate void ActiveState();
    private ActiveState activeState;
    public void SetState(Action state)
    {
        activeState = new ActiveState(state);
    }
    public void Update()
    {
        activeState?.Invoke();
    }

}
