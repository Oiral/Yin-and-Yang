using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSelectionButton : MonoBehaviour
{
    public Image visualImage;
    public Text itemName;

    public ShopSelection shop;

    public void SelectItem()
    {
        CostumeManager.instance.SetSelectedMaterial(itemName.text);
        shop.currentlySelectedColourTransform = transform;
    }

    public void SelectModel()
    {
        CostumeManager.instance.SetSelectedModel(itemName.text);

        shop.currentlySelectedModelTransform = transform;
    }
}
