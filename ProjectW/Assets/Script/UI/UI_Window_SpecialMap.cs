using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Window_SpecialMap : UIBase
{
    private GameObject _objGoodsParent;
    private GameObject _objUIGoods;
    private UIDialogButton _btnDialogButton;
    
    public void SetUI(BUTTON_FUNCTION function, int shopId)
    {
        switch (function)
        {
            case BUTTON_FUNCTION.BUTTON_FUNCTION_REWARD:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_SELECT_REWARD:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_DELETE_SOURCE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_DELETE_SUPPORT_MODULE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_DELETE_RELIC:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_SHOP:
                
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_ENHANCE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_EXCHANGE:
                break;
            case BUTTON_FUNCTION.BUTTON_FUNCTION_EXIT:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(function), function, null);
        }

    }
    
}
