using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager
{
    LinkedList<Layer> layerList;
    public StackManager()
    {
        layerList = new LinkedList<Layer>();
    }

    public float CalculateStack()
    {
        LinkedListNode<Layer> current = layerList.First;
        float output = current.Value.value;
        current = current.Next;
        while (current != null)
        {
            //Debug.Log(output);
            output = current.Value.Compute(output);
            current = current.Next;
        }
        return output;
    }
    public LinkedListNode<Layer> AddNewLayer(Layer layer)
    {
        layerList.AddLast(layer);
        return layerList.Last;
    }
    public void AddNewLayer(LinkedListNode<Layer> layerNode)
    {
        layerList.AddFirst(layerNode);
    }
    public void MoveLayer(LinkedListNode<Layer> nodeMoved, LinkedListNode<Layer> nodeMovedAfter)
    {

        nodeMoved.List.Remove(nodeMoved);
        if (nodeMoved.Value.state == LayerStates.Chained)
        {
            if (nodeMovedAfter.Value.state == LayerStates.Chained)
            {
                if (nodeMovedAfter != null)
                    nodeMovedAfter.List.AddAfter(nodeMovedAfter, nodeMoved);
                else
                    nodeMovedAfter.List.AddFirst(nodeMoved);
            }
            if (nodeMovedAfter.Value.state == LayerStates.ChainTarget)
            {
                nodeMovedAfter.Value.chainStackManager.AddNewLayer(nodeMoved);
            }
        }
        else if (nodeMoved.Value.state == LayerStates.Unchained)
        {
            if (nodeMovedAfter != null)
                layerList.AddAfter(nodeMovedAfter, nodeMoved);
            else
                layerList.AddFirst(nodeMoved);
        }
    }

    }
