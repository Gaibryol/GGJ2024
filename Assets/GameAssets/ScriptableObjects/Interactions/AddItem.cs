using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ingredient" && collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
        {
            GameObject collisionGO = collision.gameObject;
            eventBrokerComponent.Publish(this, new GameSystemEvents.ItemDroppedInMixer(collisionGO.GetComponent<Item>().GetItem()));
            Destroy(collisionGO);
        }
    }
}
