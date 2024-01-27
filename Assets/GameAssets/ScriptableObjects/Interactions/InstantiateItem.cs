using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateItem : MonoBehaviour
{
    [SerializeField] private Constants.GameSystem.RecipeItems item;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private GameObject itemPrefab;


    private void OnMouseDown()
    {
        GameObject itemGO = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Item itemComponent = itemGO.GetComponent<Item>();
        itemComponent.SetItem(item);
        itemComponent.SetSprite();
    }
}
