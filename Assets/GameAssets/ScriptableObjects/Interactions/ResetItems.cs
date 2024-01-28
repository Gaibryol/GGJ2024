using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetItems : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        eventBrokerComponent.Publish(this, new GameSystemEvents.ResetMixer());
    }
}
