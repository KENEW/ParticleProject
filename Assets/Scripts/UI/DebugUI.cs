using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle toggle;
    
    private Rarity currentLevel;
    public Rarity GetCurrentLevel
    {
        get
        {
            return this.currentLevel;
        }
    }
    
    private bool isRandomMode = false;
    public bool GetCurrentRandomMode
    {
        get
        {
            return this.isRandomMode;
        }
    }
    
    private void Start()
    {
    }

    
    private void Update()
    {
        currentLevel = (Rarity)slider.value;
        isRandomMode = toggle.isOn;
    }
}
