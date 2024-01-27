using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
    [SerializeField] private GameObject itemsGO;
    private bool active = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnMouseDown()
    {
        active = !active;
        itemsGO.SetActive(active);
    }

    private void DayEnd(BrokerEvent<GameSystemEvents.EndDay> @event)
    {
        itemsGO.SetActive(false);
    }
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameSystemEvents.EndDay>(DayEnd);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.EndDay>(DayEnd);
    }

}
