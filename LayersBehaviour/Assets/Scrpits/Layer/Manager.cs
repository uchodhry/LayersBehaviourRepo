using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LayersType {Addition , Subtraction , Multiplication , Division ,None };
public enum LayerStates { ChainTarget , Chained , Unchained };
public class Manager : MonoBehaviour
{
    public GameObject layerContainerPrefab;
    public GameObject[] layersDefinations;
    StackManager mainStack;
    private void Start()
    {
        mainStack = new StackManager();
    }

    public float CalculateLayerStack()
    {
        return mainStack.CalculateStack();
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
        defination.GetComponent<LayerElementBehaviour>().listNode = mainStack.AddNewLayer(tempLayer);// add the selected Layer type object to the Stack;
        return container; // return the new Gameobject with selected type UI defination in a container.
    }

    public void LayerMovedTo(LinkedListNode<Layer> nodeMoved , LinkedListNode<Layer> nodeMovedAfter)
    {
        mainStack.MoveLayer(nodeMoved, nodeMovedAfter);
        Debug.Log( CalculateLayerStack());
    }

    public void LayerValueUpdated(LinkedListNode<Layer> nodeMoved)
    {
        Debug.Log(CalculateLayerStack());
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
