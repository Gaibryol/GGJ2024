using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateItem : MonoBehaviour
{
    public Constants.GameSystem.RecipeItems item;
    public Sprite itemSprite;
    public GameObject itemPrefab;


    private void OnMouseDown()
    {
        GameObject itemGO = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Item itemComponent = itemGO.GetComponent<Item>();
        itemComponent.SetItem(item);
        itemComponent.SetSprite();
    }
}
