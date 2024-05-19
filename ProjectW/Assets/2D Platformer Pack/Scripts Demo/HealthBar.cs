using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider sliderHP;
    
    public void SetMaxHP(int health)
    {
        sliderHP.maxValue = health;
        sliderHP.value = health;

    }

    public void SetHP(int health)
    {
        sliderHP.value = health;
    }



}
