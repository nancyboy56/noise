using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputX : MonoBehaviour
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
        int x = NoiseManger.Instance.xMax;
        int.TryParse(value, out x);
        NoiseManger.Instance.xMax = x;
        Debug.Log("X changed" + value);
    }
}
