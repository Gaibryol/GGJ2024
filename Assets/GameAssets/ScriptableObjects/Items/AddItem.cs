using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    private EventBrokerComponent eventBroker = new EventBrokerComponent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ingredient")
        {
            GameObject collisionGO = collision.gameObject;
            eventBroker.Publish(this, new GameSystemEvents.ItemDroppedInMixer(collisionGO.GetComponent<Item>().GetItem()));
            Destroy(collisionGO);
        }
    }
}
