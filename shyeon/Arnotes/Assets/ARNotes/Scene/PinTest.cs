using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PinTest : MonoBehaviour
{
    private List<Pin> pins;
    // Start is called before the first frame update
    void Start()
    {
        this.pins = PinManager.instance.GetPins();
        foreach (Pin pin in pins)
        {
            Debug.Log("test: " + pin.GetPinName());
        }
    }
}
