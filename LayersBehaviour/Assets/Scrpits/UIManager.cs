using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI result;
    // This script manages the UI elements under this group/canvas
    Manager layerManager;
    Transform layerStackParent;

    private void Start()
    {
        layerManager = FindObjectOfType<Manager>();
        layerStackParent = transform.Find("LayersGroup/LayerStack/Viewport/Content");
    }
  
    public void UpdateResult(string newValue)
    {
        result.text = newValue;
    }
    public void addNewLayer(Transform layerUIObject)
    {
        LayersType layerTypeToAdd = layerUIObject.GetComponentInChildren<LayerElementBehaviour>().layerType;
        GameObject newLayer = layerManager.prepareNewLayer(layerTypeToAdd);
        newLayer.transform.SetParent(layerStackParent);
        newLayer.transform.SetSiblingIndex(0);
        layerManager.StackUpdated();
    }
}

