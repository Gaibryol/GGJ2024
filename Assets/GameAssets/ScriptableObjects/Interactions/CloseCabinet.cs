using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCabinet : MonoBehaviour
{
    [SerializeField] private GameObject closedCabinet;
    [SerializeField] private GameObject lowerCabinet;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        closedCabinet.SetActive(true);
        if (lowerCabinet != null)
            lowerCabinet.layer = 0;
    }
    private void DayEnd(BrokerEvent<GameSystemEvents.EndDay> @event)
    {
        gameObject.SetActive(false);
        closedCabinet.SetActive(true);
        if (lowerCabinet != null)
            lowerCabinet.layer = 0;
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
