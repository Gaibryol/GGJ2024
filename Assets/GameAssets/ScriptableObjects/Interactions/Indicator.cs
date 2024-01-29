using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private float growDuration = 3f;
    private float resetDuration = 0.5f;
    private float growTime = 0.0f;
    private float originalHeight ;
    private float maxHeight;
    private bool buttonHeld;

    private void Start()
    {
        originalHeight = 0f;
        maxHeight = 1f;

    }
    private void IsButtonHeld(BrokerEvent<IndicatorEvent.SprayPressed> @event)
    {
        buttonHeld = @event.Payload.ButtonHeld;
    }
    public IEnumerator ResetIndicatorLevel()
    {
        growTime = 0f;
        float elapsedTime = 0.0f;
        float currentHeight = transform.localScale.y;

        while (elapsedTime < resetDuration)
        {
            float newHeight = Mathf.Lerp(currentHeight, originalHeight, elapsedTime / resetDuration);

            Vector3 newScale = transform.localScale;
            newScale.y = newHeight;
            transform.localScale = newScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 finalScale = transform.localScale;
        finalScale.y = originalHeight;
        transform.localScale = finalScale;
    }
    private void StartReset(BrokerEvent<GameSystemEvents.AnimalSprayed> @event)
    {
        StartCoroutine(ResetIndicatorLevel());
    }
    private void Update()
    {
        if (buttonHeld)
        {
            growTime += Time.deltaTime;

            // Calculate the new height using lerp
            float newHeight = Mathf.Lerp(originalHeight, maxHeight, growTime / growDuration);

            // Set the new scale of the GameObject
            Vector3 newScale = transform.localScale;
            newScale.y = newHeight;
            transform.localScale = newScale;

            // Check if the scaling is complete
            if (growTime >= growDuration)
            {
                buttonHeld = false; // Reset the variable to stop scaling
                growTime = 0.0f;   // Reset the elapsed time
            }
        }
    }
    private void OnMouseDown()
    {
        StartCoroutine(ResetIndicatorLevel());
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<IndicatorEvent.SprayPressed>(IsButtonHeld);
        eventBrokerComponent.Subscribe<GameSystemEvents.AnimalSprayed>(StartReset);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<IndicatorEvent.SprayPressed>(IsButtonHeld);
        eventBrokerComponent.Subscribe<GameSystemEvents.AnimalSprayed>(StartReset);
    }
}
