using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [HideInInspector]
    public Transform parentToReturnTo = null;
    [HideInInspector]
    public Transform placeHolderParent = null;

    private GameObject placeHolder = null;

    private int dragBeginIndex = -1;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // creat a new place holder game object to move the list item.
        int LayerIndex = this.transform.GetSiblingIndex();
        placeHolder = new GameObject("--PlaceHolder--");
        placeHolder.transform.SetParent( this.transform.parent );
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        placeHolder.GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;

        placeHolder.transform.SetSiblingIndex(LayerIndex);

        parentToReturnTo = this.transform.parent; // set the orignal parent
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent); // move the selected layer out of the content

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;

        dragBeginIndex = LayerIndex;

        
        //thisLayer.valueUpdated();
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position; // move the layer with the pointer.

        if (placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }
        
        int newSiblingIndex = placeHolderParent.childCount; // start the new layer from the last pos.
        int siblingToClipwith = -1;

        for (int i = 0; i < placeHolderParent.childCount; i++)
        {
            //compair the current position with other layers and move the place holder.
            if (this.transform.position.y > placeHolderParent.GetChild(i).position.y)
            {
                newSiblingIndex = i;

                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                // clip to the layer below by moving to most left. -80 in X axis from pivot of layer below.
                if (this.transform.position.x < placeHolderParent.GetChild(newSiblingIndex).position.x-80)
                {
                    if (newSiblingIndex + 1 != placeHolderParent.childCount)
                    {
                        siblingToClipwith = newSiblingIndex;
                        Debug.Log("Clip With " + placeHolderParent.GetChild(newSiblingIndex + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                    }
                }
                break;
            }
            
        }
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int layerIndex = placeHolder.transform.GetSiblingIndex();
        if (validateMove(layerIndex))
            MoveLayer(layerIndex);
        else
            ResetMove();
    }
    bool validateMove(int moveToIndex)
    {
        LinkedListNode<Layer> thisLayerNode = transform.GetComponentInChildren<LayerElementBehaviour>().ListNode;
        LinkedListNode<Layer> layerNodeBelow = null;
        LinkedListNode<Layer> layerNodeAbove = null;
        if (moveToIndex + 1 < parentToReturnTo.childCount)
        {
            // gets the layer node for layer below if its index is not greater the last object.
            layerNodeBelow = parentToReturnTo.GetChild(moveToIndex + 1).GetComponentInChildren<LayerElementBehaviour>().ListNode;
        }
        if (moveToIndex - 1 >= 0)
        {
            // gets the layer node for layer above if its index is not less the last zero.
            layerNodeAbove = parentToReturnTo.GetChild(moveToIndex - 1).GetComponentInChildren<LayerElementBehaviour>().ListNode;
        }
        return GameObject.FindObjectOfType<Manager>().ValidateMove(
            thisLayerNode,
            layerNodeAbove,
            layerNodeBelow
            );
        
    }
    void MoveLayer(int moveToIndex)
    {
        DestroyImmediate(placeHolder);
        this.transform.SetParent(parentToReturnTo); // move the layer to orignal parent.
        this.transform.SetSiblingIndex(moveToIndex); // set same index of the selected layer as placeholder.
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;// re-enable the raycast hit.
        UpdateLayerStack(moveToIndex);
    }

    void ResetMove()
    {
        DestroyImmediate(placeHolder);
        this.transform.SetParent(parentToReturnTo); // move the layer to orignal parent.
        this.transform.SetSiblingIndex(dragBeginIndex); // set same index of the selected layer as placeholder.
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;// re-enable the raycast hit.
    }

    void UpdateLayerStack(int LayerIndex)
    {
        // this layer node.
        LinkedListNode<Layer> thisLayerNode = transform.GetComponentInChildren<LayerElementBehaviour>().ListNode;
        // node of the layer below this layer.
        LinkedListNode<Layer> layerNodeBelowBeforeMove = null;
        // node of the layer below this layer.
        LinkedListNode<Layer> layerNodeBelow = null;
        // node of the layer above this layer.
        LinkedListNode<Layer> layerNodeAbove = null;

        if (LayerIndex + 1 < parentToReturnTo.childCount)
        {
            // gets the layer node for layer below if its index is not greater the last object.
            layerNodeBelow = parentToReturnTo.GetChild(LayerIndex + 1).GetComponentInChildren<LayerElementBehaviour>().ListNode;
        }
        if (dragBeginIndex + 1 < parentToReturnTo.childCount)
        {
            // gets the layer node for layer below if its index is not greater the last object.
            layerNodeBelowBeforeMove = parentToReturnTo.GetChild(dragBeginIndex + 1).GetComponentInChildren<LayerElementBehaviour>().ListNode;
        }
        if (LayerIndex - 1 >= 0)
        {
            // gets the layer node for layer above if its index is not less the last zero.
            layerNodeAbove = parentToReturnTo.GetChild(LayerIndex - 1).GetComponentInChildren<LayerElementBehaviour>().ListNode;
        }

        GameObject.FindObjectOfType<Manager>().LayerMovedFrom(
            thisLayerNode,
            layerNodeBelowBeforeMove
        );

        thisLayerNode.Value.state = LayerStates.Unchained;
        if (Input.GetKey(KeyCode.LeftAlt)) // checks if user is holding left alt key.
        {
            if (layerNodeBelow != null) // There is a valid layer below.
            {
                thisLayerNode.Value.state = LayerStates.Chained;
                switch (layerNodeBelow.Value.state)
                {
                    case LayerStates.ChainTarget:
                        break;
                    case LayerStates.Chained:
                        break;
                    case LayerStates.Unchained:
                        layerNodeBelow.Value.state = LayerStates.ChainTarget;
                        break;
                    default:
                        break;
                }
            }
        }
        if (layerNodeAbove != null) // There is a valid layer above.
        {
            switch (layerNodeAbove.Value.state)
            {
                case LayerStates.ChainTarget:
                    break;
                case LayerStates.Chained:
                    thisLayerNode.Value.state = LayerStates.Chained;
                    break;
                case LayerStates.Unchained:
                    break;
                default:
                    break;
            }
        }
        
        

        GameObject.FindObjectOfType<Manager>().LayerMovedTo(
            thisLayerNode,
            layerNodeBelow
            );
    }
}
