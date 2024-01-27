using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    public ItemsManager IM;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ingredient")
        {
            GameObject collisionGO = collision.gameObject;
            IM.AddItem(collisionGO.GetComponent<Item>().GetItem());
            Destroy(collisionGO);
        }
    }
}
