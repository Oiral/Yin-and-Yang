using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopSelection : MonoBehaviour
{
    public GameObject shopButtonPrefab;
    public GameObject fillerPrefab;
    public GameObject modelButtonPrefab;

    public Transform currentlySelectedColourTransform;
    public GameObject currentlySelectedColourObject;

    public Transform currentlySelectedModelTransform;
    public GameObject currentlySelectedModelObject;

    public float moveSpeed = 5f;

    private void Start()
    {
        CostumeManager costumes = CostumeManager.instance;

        int count = 0;

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
            count ++;
        }

        count += 3;
        Instantiate(fillerPrefab, transform);
        Instantiate(fillerPrefab, transform);
        Instantiate(fillerPrefab, transform);

        foreach (var costumeModel in costumes.playerModels)
        {
            GameObject spawnedButton = Instantiate(modelButtonPrefab, transform);

            ShopSelectionButton button = spawnedButton.GetComponent<ShopSelectionButton>();

            button.itemName.text = costumeModel.Key;
            button.visualImage.color = new Color(0, 0, 0, 0);

            button.shop = this;

            //If this is currently selected colour
            if (button.itemName.text == costumes.selectedModel)
            {
                currentlySelectedModelTransform = spawnedButton.transform;
                currentlySelectedModelObject.transform.position = spawnedButton.transform.position;
            }

            Instantiate(fillerPrefab, transform);
            count ++;
        }

        ChangeHeight(count);

    }

    private void Update()
    {
        currentlySelectedColourObject.transform.position = Vector3.Lerp(
                                    currentlySelectedColourObject.transform.position,
                                    currentlySelectedColourTransform.position,
                                    moveSpeed * Time.deltaTime);

        currentlySelectedModelObject.transform.position = Vector3.Lerp(
                                    currentlySelectedModelObject.transform.position,
                                    currentlySelectedModelTransform.position,
                                    moveSpeed * Time.deltaTime);


    }

    void ChangeHeight(int cellcount)
    {
        //Lets adjust the size of the area
        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();

        Vector2 size = GetComponent<RectTransform>().sizeDelta;

        Vector2 cellSize = layoutGroup.cellSize;

        Vector2 spacing = layoutGroup.spacing;


        float count = cellcount;

        count = (count / 3) * 2f;

        Mathf.Ceil(count);

        size.y = (cellSize.y + spacing.y) * count;

        size.y += layoutGroup.padding.bottom;

        size.y += layoutGroup.padding.top;

        GetComponent<RectTransform>().sizeDelta = size;
    }
}
