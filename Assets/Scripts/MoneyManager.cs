using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    string playerPrefsMoneyID = "MoneyCount";
    [ReadOnly]
    public int moneyCount;

    public MoneyVisual moneyDisplay;

    private void Start()
    {
        LoadMoney();
    }

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
            moneyDisplay.PlayAnimation();
        }
        SaveMoney();
    }

    public bool PurchaseItem(int amount)
    {
        if (CanPurchase(amount))
        {
            moneyCount -= amount;
            SaveMoney();
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

    [ContextMenu("Reset Money")]
    public void ResetMoney()
    {
        moneyCount = 0;
        SaveMoney();
    }

    public void ToggleDisplay(bool shouldEnable)
    {
        moneyDisplay.gameObject.SetActive(shouldEnable);
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


  

    #region Singleton

    public static MoneyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(this);
        }
    }

    #endregion
}
