using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Actor_DMGFloater : UIBase
{
    [SerializeField] private TextMeshProUGUI text;

    
    public void ShowDamage(int damage)
    {
        text.text = damage.ToString();
        Show();

    }
    
}
