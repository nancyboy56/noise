using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderX : MonoBehaviour
{
    public Slider mainSlider;

    public void Start()
    {
        mainSlider = GetComponent<Slider>();
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        NoiseManger.Instance.scale = (int)mainSlider.value;
        Debug.Log("Scale slider:"+ mainSlider.value);
    }
}
