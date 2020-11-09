using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    string playerPrefsMoneyID = "MoneyCount";
    public int moneyCount;

    public void GainMoney(int amount)
    {
        if (amount < 0)
        {
            //We should decrease rather than increase
            PurchaseItem(-amount);
        }
        else
        {
            moneyCount += amount;
        }
    }

    public bool PurchaseItem(int amount)
    {
        if (CanPurchase(amount))
        {
            moneyCount -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanPurchase(int amount)
    {
        //If the amount were trying to purchase is greater than the amount we have, We cannot purchase
        if (amount > moneyCount)
        {
            //We cannot purchase
            return false;
        }
        else
        {
            //We can purchase
            return true;
        }
    }



    #region Player Prefs
    void SaveMoney()
    {
        PlayerPrefs.SetInt(playerPrefsMoneyID, moneyCount);
    }

    void LoadMoney()
    {
        if (PlayerPrefs.HasKey(playerPrefsMoneyID))
        {
            //Lets load it into moneyCount
            moneyCount = PlayerPrefs.GetInt(playerPrefsMoneyID);
        }
        else
        {
            moneyCount = 0;
        }
    }
    #endregion
}
