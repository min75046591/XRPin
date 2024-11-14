using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConCollisionDetector : MonoBehaviour
{
    public MainController mainController;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mainController.IsEnableGenarationMode()) return;
        meshRenderer.enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        meshRenderer.enabled = true;
    }

}
