using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitchManager : MonoBehaviour
{
    public static ModeSwitchManager Instance;

    bool state = true;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }


    public void SwitchOn()
    {
        state = true;
        StartCoroutine(UIManagerPhotobooth.Instance.SwitchOnCoroutine(state));
    }

    public void SwitchOff()
    {
        state = false;
        StartCoroutine(UIManagerPhotobooth.Instance.SwitchOffCoroutine(state));
    }

    public bool GetState()
    {
        return state;
    }
}
