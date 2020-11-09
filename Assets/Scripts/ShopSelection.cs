using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopSelection : MonoBehaviour
{
    public GameObject shopButtonPrefab;
    public GameObject fillerPrefab;

    public Transform currentlySelectedColourTransform;
    public GameObject currentlySelectedColourObject;

    public float moveSpeed = 5f;

    private void Start()
    {
        CostumeManager costumes = CostumeManager.instance;

        foreach (var costumeCol in costumes.playerColours)
        {
            GameObject spawnedButton = Instantiate(shopButtonPrefab, transform);

            ShopSelectionButton button = spawnedButton.GetComponent<ShopSelectionButton>();

            button.itemName.text = costumeCol.Key;
            button.visualImage.color = costumeCol.Value.material.color;

            button.shop = this;

            //If this is currently selected colour
            if (button.itemName.text == costumes.selectedMaterial)
            {
                currentlySelectedColourTransform = spawnedButton.transform;
                currentlySelectedColourObject.transform.position = spawnedButton.transform.position;
            }

            Instantiate(fillerPrefab, transform);
        }

        //Lets adjust the size of the area
        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();

        Vector2 size = GetComponent<RectTransform>().sizeDelta;

        Vector2 cellSize = layoutGroup.cellSize;

        Vector2 spacing = layoutGroup.spacing;


        float count = costumes.playerColours.Count;

        count = (count / 3) * 2f;

        Mathf.Ceil(count);

        size.y = (cellSize.y + spacing.y) * count;

        size.y += layoutGroup.padding.bottom;

        size.y += layoutGroup.padding.top;

        GetComponent<RectTransform>().sizeDelta = size;

    }

    private void Update()
    {
        currentlySelectedColourObject.transform.position = Vector3.Lerp(
                                    currentlySelectedColourObject.transform.position,
                                    currentlySelectedColourTransform.position,
                                    moveSpeed * Time.deltaTime);


    }
}
