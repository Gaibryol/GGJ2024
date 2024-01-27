using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    private EventBrokerComponent eventBroker = new EventBrokerComponent();
    private List<Constants.GameSystem.RecipeItems> currentItems = new List<Constants.GameSystem.RecipeItems>();

    public void AddItem(Constants.GameSystem.RecipeItems ingredient)
    {
        currentItems.Add(ingredient);
        PublishItemChanged();
    }

    public void ResetItems()
    {
        currentItems = new List<Constants.GameSystem.RecipeItems>();
        PublishItemChanged();
    }

    private void PublishItemChanged()
    {
        eventBroker.Publish(this, new ItemEvents(currentItems));
    }
}
