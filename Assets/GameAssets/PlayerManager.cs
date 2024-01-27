using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void SendSprayLevel(Constants.GameSystem.SprayLevel sprayLevel)
    {
        eventBrokerComponent.Publish(this, new GameSystemEvents.AnimalSprayed(sprayLevel));
    }
}
