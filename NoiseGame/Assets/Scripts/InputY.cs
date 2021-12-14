using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputY : MonoBehaviour
{
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputField>();
        input.onEndEdit.AddListener(ValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueChange(string value)
    {
        int x = NoiseManger.Instance.yMax;
        int.TryParse(value, out x);
        NoiseManger.Instance.yMax = x;
        Debug.Log("Y changed" + value);
    }

}
