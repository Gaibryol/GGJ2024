using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSwitch : MonoBehaviour
{
    [SerializeField] private GameObject itemsGO;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnMouseDown()
    {
		if (EventSystem.current.IsPointerOverGameObject()) return;
		Invoke("OpenBook", 0.01f);
	}

	private void OpenBook()
	{
		itemsGO.SetActive(true);
		eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BookFlip));
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
