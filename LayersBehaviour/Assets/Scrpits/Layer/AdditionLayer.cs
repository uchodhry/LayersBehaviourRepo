﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionLayer : Layer
{
    // Start is called before the first frame update
    public override float Compute(float previousValue)
    {
        return base.value + previousValue;
    }
}
