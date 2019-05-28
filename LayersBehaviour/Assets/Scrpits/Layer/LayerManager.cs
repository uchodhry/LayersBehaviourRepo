using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LayersType {Addition , Subtraction , Multiplication , Division ,None };
public class LayerManager : MonoBehaviour
{
    public GameObject layerContainerPrefab;
    public GameObject[] layersDefinations;
    LinkedList<Layer> layerList;
    private void Start()
    {
        layerList = new LinkedList<Layer>();
    }

    public float CalculateLayerStack()
    {
        LinkedListNode<Layer> current = layerList.First;
        float output = current.Value.value;
        current = current.Next;
        while (current != null)
        {
            Debug.Log(output);
            output = current.Value.Compute(output);
            current = current.Next;
        }
        return output;
    }

    public GameObject prepareNewLayer(LayersType type)
    {
        GameObject defination = getUIDefinationFromType(type); // get the UI defination prefab of current selected layer type.
        if (defination == null)
            return null;
        GameObject container = Instantiate(layerContainerPrefab); // instantiat the defination container prefab.
        defination.transform.SetParent(container.transform); // add the UI defination as chiled of container.


        Layer tempLayer = Layer.getLayerOfType(type); // get the type of class from factory class.
        tempLayer.value = defination.GetComponent<LayerElementProperty>().getInputFieldValue(); // get the default Value of the layer.
        tempLayer.layerType = type; // set the type of the layer this should not be used if every thing work in drived classes.
        layerList.AddLast(tempLayer); // add the selected Layer type object to the Stack;
        defination.GetComponent<LayerElementProperty>().listNode = layerList.Last;
        return container; // return the new Gameobject with selected type UI defination in a container.
    }

    public void LayerMovedTo(LinkedListNode<Layer> nodeMoved , LinkedListNode<Layer> nodeMovedAfter , bool isChained = false)
    {
        layerList.Remove(nodeMoved);
        layerList.AddAfter(nodeMovedAfter, nodeMoved); 
    } 


    private GameObject getUIDefinationFromType(LayersType type)
    {
        foreach (GameObject defination in layersDefinations)
        {
            if (defination.GetComponent<LayerElementProperty>().layerType == type)
                return Instantiate(defination);
        }
        return null;
    }

}
