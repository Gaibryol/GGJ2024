using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseCabinet : MonoBehaviour
{
    [SerializeField] private GameObject closedCabinet;
    [SerializeField] private GameObject lowerCabinet;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnMouseDown()
    {
		if (EventSystem.current.IsPointerOverGameObject()) return;

        gameObject.SetActive(false);
        closedCabinet.SetActive(true);
        if (lowerCabinet != null)
        {
            lowerCabinet.layer = 0;
            foreach (Transform child in lowerCabinet.transform)
            {
                child.gameObject.layer = 0;
            }
        }
		eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Drawer));
	}

    private void DayEnd(BrokerEvent<GameSystemEvents.EndDay> @event)
    {
        gameObject.SetActive(false);
        closedCabinet.SetActive(true);
        if (lowerCabinet != null)
        {
            lowerCabinet.layer = 0;
            foreach (Transform child in lowerCabinet.transform)
            {
                child.gameObject.layer = 0;
            }
        }
    }

	private void RestartHandler(BrokerEvent<GameSystemEvents.Restart> inEvent)
	{
		gameObject.SetActive(false);
		closedCabinet.SetActive(true);
		if (lowerCabinet != null)
		{
			lowerCabinet.layer = 0;
			foreach (Transform child in lowerCabinet.transform)
			{
				child.gameObject.layer = 0;
			}
		}
	}

	private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameSystemEvents.EndDay>(DayEnd);
		eventBrokerComponent.Subscribe<GameSystemEvents.Restart>(RestartHandler);
    }

	private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.EndDay>(DayEnd);
		eventBrokerComponent.Unsubscribe<GameSystemEvents.Restart>(RestartHandler);
	}

}
