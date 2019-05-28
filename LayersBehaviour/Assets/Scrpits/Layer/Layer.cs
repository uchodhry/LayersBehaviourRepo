using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    public float value;
    public LayersType layerType;
    public virtual float Compute(float previousValue)
    {
        Debug.LogError("Base Class Reached Leyer Type Missing in setType function.");
        return previousValue;
    }

    public static Layer getLayerOfType(LayersType type)
    {
        switch (type)
        {
            case LayersType.Addition:
                return new AdditionLayer();
            case LayersType.Subtraction:
                return new SubtractionLayer();
            case LayersType.Multiplication:
                return new SubtractionLayer();
            case LayersType.Division:
                return new Layer();
            case LayersType.None:
                return new Layer();
            default:
                return new Layer();
        }
    }
    
}
