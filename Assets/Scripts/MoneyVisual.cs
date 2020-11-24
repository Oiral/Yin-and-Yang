using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MoneyVisual))]
public class MoneyVisual : MonoBehaviour
{
    public Text moneyCounter;

    private void Start()
    {
        UpdateVisualNumber();
    }

    public void PlayAnimation()
    {
        GetComponent<Animator>().SetTrigger("Collect");
    }

    public void UpdateVisualNumber()
    {
        moneyCounter.text = MoneyManager.instance.moneyCount.ToString();
    }
}
