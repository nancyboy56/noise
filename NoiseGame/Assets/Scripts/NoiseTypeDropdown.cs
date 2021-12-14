using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoiseTypeDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;
  

    private void Start()
    {

        dropdown = GetComponent<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });

    }


    private void DropdownValueChanged()
    {
        if (dropdown.value == 0)
        {
            NoiseManger.Instance.currentNoise = NoiseType.Noise2D;
        }
        else if (dropdown.value == 1)
        {
            NoiseManger.Instance.currentNoise = NoiseType.Perlin;
        }
      

    }
}
