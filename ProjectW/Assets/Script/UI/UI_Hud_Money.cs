using System;
using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hud_Money: MonoBehaviour
{
   private int moneyID = 0;
   [SerializeField] private TextMeshProUGUI amount;
   [SerializeField] private Image imgMoney;
   
   public void RegisterMoney(int id)
   {
      MoneyData.Instance.OnMoneyChanged += OnMoneyAmountChange;
   }

   private void OnMoneyAmountChange(int id, int amount)
   {
      if (id == moneyID)
      {
         SetMoney(amount);
      }
   }

   public void UnregisterMoney(int id)
   {
      MoneyData.Instance.OnMoneyChanged -= OnMoneyAmountChange;
   }

   private void SetMoney(int money)
   {
      amount.text = money.ToString();
   }
   
   public void SetMoneyImage(int moneyType)
   {
      var tableData = GameTableManager.Instance._moneyTable.Find(_=>_.money_id == moneyType);
      moneyID = moneyType;
      imgMoney.sprite = GameResourceManager.Instance.GetImage(tableData.money_img);
   }
}
