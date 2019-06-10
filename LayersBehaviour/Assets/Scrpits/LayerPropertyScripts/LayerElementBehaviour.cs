using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayerElementBehaviour : MonoBehaviour
{
    public LayersType layerType;
    public LinkedListNode<Layer> listNode;

    public float getInputFieldValue()
    {
        TMP_InputField tMP_InputField = GetComponentInChildren<TMP_InputField>();
        return float.Parse(tMP_InputField.text);

    }
    public void valueUpdated()
    {
        TMP_InputField tMP_InputField = GetComponentInChildren<TMP_InputField>();
        listNode.Value.value = float.Parse(tMP_InputField.text);
        FindObjectOfType<Manager>().LayerValueUpdated(listNode);
    }
    public void LayerStateChanged(LayerStates state)
    {
        Debug.Log(state.ToString());
    }
}
