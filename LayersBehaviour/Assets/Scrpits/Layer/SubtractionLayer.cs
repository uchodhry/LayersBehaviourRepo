using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtractionLayer : Layer
{
    public override float Compute(float previousValue)
    {
        if (float.IsNaN(previousValue))
        {
            previousValue = 0;
        }
        if (base.state == LayerStates.ChainTarget)// if this node is a chain target it needs to calculate the stack first.
        {
            return previousValue - (base.value - base.chainStack.CalculateStack());
        }
        else
        {
            return previousValue - base.value;
        }
    }
 
}
