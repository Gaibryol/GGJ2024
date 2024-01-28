using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.GetComponent<Item>().IsDissolving) return;

        if (collision.gameObject.tag == "Ingredient" && collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
        {
            GameObject collisionGO = collision.gameObject;
            StartCoroutine(collisionGO.GetComponent<Item>().DissolveSprite(.5f));
            eventBrokerComponent.Publish(this, new GameSystemEvents.ItemDroppedInMixer(collisionGO.GetComponent<Item>().GetItem()));
			eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Mixer));
        }
    }
}
