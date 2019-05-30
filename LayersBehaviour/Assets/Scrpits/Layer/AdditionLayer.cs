﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionLayer : Layer
{
    public override float Compute(float previousValue)
    {
        return previousValue + base.value;
    }
}
