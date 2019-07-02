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
   
    public float CalculateStack(float baseValue = float.NaN)
    {
        LinkedListNode<Layer> current = layerList.First;
        float output = baseValue;
        output = current.Value.Compute(output);//current.Value.value;
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
    public void MoveList (LinkedListNode<Layer> nodeMoved, LinkedListNode<Layer> nodeMovedAfter)
    {
        LinkedList<Layer> baseList = nodeMoved.List;
        LinkedList<Layer> chainList = nodeMoved.Value.chainStack.layerList;

        Debug.Log(" baseList " + baseList.Count);
        Debug.Log(" nodeMovedAfter list " + chainList.Count);

        LinkedListNode<Layer> nodeToMoveAfter = nodeMovedAfter;
        LinkedListNode<Layer> current = chainList.First;
        
        while (chainList.Count > 0)
        {
            chainList.RemoveFirst();
            //Debug.Log(baseList == nodeMovedAfter.List);
            //Debug.Log(" baseList " + baseList.Count);
           // Debug.Log(" nodeMovedAfter list " + nodeMovedAfter.List.Count);
            baseList.AddAfter(nodeToMoveAfter, current);
            nodeToMoveAfter = current;
            current.Value.state = LayerStates.Unchained;
            current = chainList.First;
            //break;
        }
        nodeMoved.Value.chainStack.layerList = null;
    }
    public void MoveLayer(LinkedListNode<Layer> nodeMoved, LinkedListNode<Layer> nodeMovedAfter)
    {
        nodeMoved.List.Remove(nodeMoved);
        if (nodeMoved.Value.state == LayerStates.Chained)
        {
            if (nodeMovedAfter != null)
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
                    nodeMovedAfter.Value.chainStack.AddNewLayer(nodeMoved);
                }
            }
        }
        else if (nodeMoved.Value.state == LayerStates.Unchained)
        {
            if (nodeMovedAfter != null)
            {
                if (nodeMovedAfter.Value.state == LayerStates.Chained)
                {
                    
                    int LayerIndex = nodeMovedAfter.List.First.Value.layerTransform.parent.GetSiblingIndex();
                    Debug.Log(nodeMovedAfter.List.First.Value.layerTransform.parent.name);
                    
                    nodeMovedAfter = nodeMovedAfter.List.First.Value.layerTransform.parent.parent.GetChild(LayerIndex + 1).GetComponentInChildren<LayerElementBehaviour>().ListNode;
                }
                
                layerList.AddAfter(nodeMovedAfter, nodeMoved);
                
            }
            else
                layerList.AddFirst(nodeMoved);
        }
    }

}
