using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RulesEngine.Rules;
using RulesEngine;

[RuleEngineType(RuleType = typeof(DefaultRuleEngine<Layer>))]
public class Layer
{
    public float value;
    public virtual LayersType layerType { get; set; }
    public StackManager chainStack;
    public Transform layerTransform;
    private LayerStates _state;
    public LayerStates state
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            if(layerTransform!=null)
                layerTransform.GetComponent<LayerElementBehaviour>().LayerStateChanged();
        }
    }

    public Layer()
    {
        chainStack = new StackManager();
        state = LayerStates.Unchained;
    }

    public virtual float Compute(float previousValue)
    {
        Debug.LogError("Base Class Reached Leyer Type Missing in setType function.");
        return previousValue;
    }
    public virtual bool Validate(LayersType below)
    {
        Debug.LogError("Base Class Reached Leyer Type Missing in setType function.");
        return true;
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
                return new MultiplayLayer();
            case LayersType.Division:
                return new Layer();
            case LayersType.None:
                return new Layer();
            default:
                return new Layer();
        }
    }
    
}
