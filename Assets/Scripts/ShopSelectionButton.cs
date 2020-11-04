﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSelectionButton : MonoBehaviour
{
    public Image visualImage;
    public Text itemName;

    public void SelectItem()
    {
        CostumeManager.instance.SetSelected(itemName.text);
    }
}