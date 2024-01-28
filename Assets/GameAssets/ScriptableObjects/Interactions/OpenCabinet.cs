using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCabinet : MonoBehaviour
{
    [SerializeField] private GameObject openedCabinet;
    [SerializeField] private GameObject lowerCabinet;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        openedCabinet.SetActive(true);
        if(lowerCabinet != null)
            lowerCabinet.layer = 2;
    }
    private void DayEnd(BrokerEvent<GameSystemEvents.EndDay> @event)
    {
        gameObject.SetActive(true);
        openedCabinet.SetActive(false);
        if(lowerCabinet != null)
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
