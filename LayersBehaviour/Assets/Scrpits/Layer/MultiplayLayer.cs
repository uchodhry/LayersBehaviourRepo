using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RulesEngine.Rules;
using RulesEngine;

[RuleEngineType(RuleType = typeof(DefaultRuleEngine<MultiplayLayer>))]
public class MultiplayLayer : Layer
{
    
    public override LayersType layerType { get; set; }
    [CompareLayerType("layerBelow", "can't have layer of this type below", LayersType.Subtraction)]
    public LayersType layerBelow { get; set; }
    public override float Compute(float previousValue)
    {
        if (float.IsNaN(previousValue))
        {
            previousValue = 1;
        }
        if (base.state == LayerStates.ChainTarget)// if this node is a chain target it needs to calculate the stack first.
        {
            return previousValue * base.value * base.chainStack.CalculateStack();
        }
        else
        {
            return previousValue * base.value;
        }
    }
    public override bool Validate(LayersType below)
    {
        layerBelow = below;
        IRuleEngine <MultiplayLayer> ruleEngine = RuleEngineFactory<MultiplayLayer>.GetEngine();
        var results = ruleEngine.Validate(this);
        foreach (var r in results)
        {
            if (r.IsBroken)
            {
                Debug.LogError(r.Name + " rule is broken and the error is " + r.ErrorMessage);
            }
        }
        return results.Count == 0;
    }
}
