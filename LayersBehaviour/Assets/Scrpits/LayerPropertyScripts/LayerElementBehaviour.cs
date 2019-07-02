using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayerElementBehaviour : MonoBehaviour
{
    public LayersType layerType;
    private LinkedListNode<Layer> listNode;


    public LinkedListNode<Layer> ListNode
    {
        get
        {
            return listNode;
        }
        set
        {
            listNode = value;
            if (value != null)
                listNode.Value.layerTransform = transform;
        }
    }

    public float getInputFieldValue()
    {
        TMP_InputField tMP_InputField = GetComponentInChildren<TMP_InputField>();
        return float.Parse(tMP_InputField.text);

    }
    public void valueUpdated()
    {
        TMP_InputField tMP_InputField = GetComponentInChildren<TMP_InputField>();
        ListNode.Value.value = float.Parse(tMP_InputField.text);
        FindObjectOfType<Manager>().LayerValueUpdated(ListNode);
    }
    public void LayerStateChanged()
    {
        switch (ListNode.Value.state)
        {
            case LayerStates.Chained:
                transform.Find("ChainGoup").gameObject.SetActive(true);
                transform.Find("TargetGoup").gameObject.SetActive(false);
                break;
            case LayerStates.ChainTarget:
                transform.Find("TargetGoup").gameObject.SetActive(true);
                break;
            case LayerStates.Unchained:
                transform.Find("ChainGoup").gameObject.SetActive(false);
                transform.Find("TargetGoup").gameObject.SetActive(false);
                break;
            default:
                break;
        }
        Debug.Log(ListNode.Value.state);
    }
}
