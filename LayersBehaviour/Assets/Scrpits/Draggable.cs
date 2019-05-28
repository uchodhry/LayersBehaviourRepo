using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [HideInInspector]
    public Transform parentToReturnTo = null;
    [HideInInspector]
    public Transform placeHolderParent = null;

    private GameObject placeHolder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // creat a new place holder game object to move the list item.
        placeHolder = new GameObject("--PlaceHolder--");
        placeHolder.transform.SetParent( this.transform.parent );
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        placeHolder.GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;

        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex() );

        parentToReturnTo = this.transform.parent; // set the orignal parent
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent); // move the selected layer out of the content

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
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
        this.transform.SetParent(parentToReturnTo); // move the layer to orignal parent.
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex()); // set same index of the selected layer as placeholder.
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;// re-enable the raycast hit.
        Destroy(placeHolder);
    }
}
