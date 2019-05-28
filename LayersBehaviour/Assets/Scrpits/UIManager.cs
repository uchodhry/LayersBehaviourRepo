using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI result;
    // This script manages the UI elements under this group/canvas
    LayerManager layerManager;
    Transform layerStackParent;

    private void Start()
    {
        layerManager = FindObjectOfType<LayerManager>();
        layerStackParent = transform.Find("LayersGroup/LayerStack/Viewport/Content");
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            result.text = layerManager.CalculateLayerStack().ToString();
        }
    }
    public void addNewLayer(Transform layerUIObject)
    {
        LayersType layerTypeToAdd = layerUIObject.GetComponentInChildren<LayerElementProperty>().layerType;
        GameObject newLayer = layerManager.prepareNewLayer(layerTypeToAdd);
        newLayer.transform.SetParent(layerStackParent);
        newLayer.transform.SetSiblingIndex(0);
    }
}

