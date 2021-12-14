using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourDropdown : MonoBehaviour
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
            NoiseManger.Instance.currentColour = ColourType.Hex;
        } else if (dropdown.value == 1)
        {
            NoiseManger.Instance.currentColour = ColourType.Red;
        }
        else if (dropdown.value == 2)
        {
            NoiseManger.Instance.currentColour = ColourType.Monochrome;
        } else if(dropdown.value == 3)
        {
            NoiseManger.Instance.currentColour = ColourType.HSV;
        }
          
    }
}
