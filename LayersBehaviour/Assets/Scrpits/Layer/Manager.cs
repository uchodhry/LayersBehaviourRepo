using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RulesEngine;
public enum LayersType {Addition , Subtraction , Multiplication , Division ,None };
public enum LayerStates { ChainTarget , Chained , Unchained };

public class Manager : MonoBehaviour
{
    public GameObject layerContainerPrefab;
    public GameObject[] layersDefinations;
    StackManager stackManager;
    UIManager uiManager;
    
    private void Start()
    {
        stackManager = new StackManager();
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    public void StackUpdated()
    {
        string result = CalculateLayerStack().ToString();
        uiManager.UpdateResult(result);
        Debug.Log(result);
    }
    
    public GameObject prepareNewLayer(LayersType type)
    {
        GameObject defination = getUIDefinationFromType(type); // get the UI defination prefab of current selected layer type.
        if (defination == null)
            return null;
        GameObject container = Instantiate(layerContainerPrefab); // instantiat the defination container prefab.
        defination.transform.SetParent(container.transform); // add the UI defination as chiled of container.
        
        Layer tempLayer = Layer.getLayerOfType(type); // get the type of class from factory class.
        tempLayer.value = defination.GetComponent<LayerElementBehaviour>().getInputFieldValue(); // get the default Value of the layer.
        tempLayer.layerType = type; // set the type of the layer this should not be used if every thing work in drived classes.
        defination.GetComponent<LayerElementBehaviour>().ListNode = stackManager.AddNewLayer(tempLayer);// add the selected Layer type object to the Stack;
        return container; // return the new Gameobject with selected type UI defination in a container.
    }
    public bool ValidateMove(LinkedListNode<Layer> nodeMoved, LinkedListNode<Layer> nodeMovedAfter, LinkedListNode<Layer> nodeMovedBefore)
    {
        if (nodeMovedBefore == null)
        {
            Debug.LogError("Node below is null");
        }
        return nodeMoved.Value.Validate(nodeMovedBefore.Value.layerType);
    }
    public void LayerMovedTo(LinkedListNode<Layer> nodeMoved , LinkedListNode<Layer> nodeMovedAfter)
    {
        stackManager.MoveLayer(nodeMoved, nodeMovedAfter);
        StackUpdated();
    }
    public void LayerMovedFrom(LinkedListNode<Layer> nodeMoved, LinkedListNode<Layer> nodeMovedAfter)
    {
        if (nodeMoved.Value.state == LayerStates.ChainTarget)
        {
            stackManager.MoveList(nodeMoved, nodeMovedAfter);
        }
        else if (nodeMoved.Value.state == LayerStates.Chained)
        {
            Debug.Log("reset the state");
            Debug.Log((nodeMoved == nodeMoved.List.First));
            Debug.Log((nodeMoved.Next == null));
           
            if ((nodeMoved == nodeMoved.List.First) && (nodeMoved.Next == null))
            {
                nodeMovedAfter.Value.state = LayerStates.Unchained;
            }
        }
        //StackUpdated();
    }

    public void LayerValueUpdated(LinkedListNode<Layer> nodeMoved)
    {
        StackUpdated();
    }

    private float CalculateLayerStack()
    {
        return stackManager.CalculateStack();
    }

    private GameObject getUIDefinationFromType(LayersType type)
    {
        foreach (GameObject defination in layersDefinations)
        {
            if (defination.GetComponent<LayerElementBehaviour>().layerType == type)
                return Instantiate(defination);
        }
        return null;
    }
}
