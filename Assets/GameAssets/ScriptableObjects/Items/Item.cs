using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private Constants.GameSystem.RecipeItems item;

    private Material material;
    private float dissolveSeconds = 1f;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameSystemEvents.ClearTable>(OnClearTable);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.ClearTable>(OnClearTable);
    }

    private void OnClearTable(BrokerEvent<GameSystemEvents.ClearTable> @event)
    {
        StartCoroutine(DissolveSprite());
    }

    private IEnumerator DissolveSprite()
    {
        // Must be using DissolveMaterial for this to work.
        for (float dissolveTime = 0f; dissolveTime <= dissolveSeconds; dissolveTime += Time.deltaTime)
        {
            material.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, dissolveTime/dissolveSeconds));
            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetItem(Constants.GameSystem.RecipeItems recipeItem)
    {
        item = recipeItem;
        gameObject.name = item.ToString();
    }
    public Constants.GameSystem.RecipeItems GetItem()
    {
        return item;
    }
    public void SetSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
