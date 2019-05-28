using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayerElementProperty : MonoBehaviour
{
    public LayersType layerType;
    public LinkedListNode<Layer> listNode;

    public float getInputFieldValue()
    {
        TMP_InputField tMP_InputField = GetComponentInChildren<TMP_InputField>();
        return float.Parse(tMP_InputField.text);
    }
}
