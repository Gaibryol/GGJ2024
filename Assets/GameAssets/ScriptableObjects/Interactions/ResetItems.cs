using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetItems : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    // Start is called before the first frame update
    private void OnMouseDown()
    {
		if (EventSystem.current.IsPointerOverGameObject()) return;
		eventBrokerComponent.Publish(this, new GameSystemEvents.ResetMixer());
    }
}
