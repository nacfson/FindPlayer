using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ClockUI : MonoBehaviour {
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _clockImage;
    
    public void SetUI(float currentValue, float maxValue,bool result = true) {
        gameObject.SetActive(result); 
        if(currentValue >= 0) {
            _timerText.SetText(String.Format("{0:0.0#}",currentValue));
        }
        _clockImage.fillAmount = currentValue / maxValue;
    }
}