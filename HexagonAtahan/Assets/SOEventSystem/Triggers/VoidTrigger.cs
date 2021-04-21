using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mainly for use on buttons
public class VoidTrigger : MonoBehaviour
{
    public VoidEvent eventToRaise;

    public void Trigger() {
        if (eventToRaise != null) { eventToRaise.Raise(new Void()); }       
    }


}
