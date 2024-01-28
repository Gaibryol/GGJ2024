using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private float growDuration = 3f;
    private float resetDuration = 0.5f;
    private float elapsedTime = 0.0f;
    private float originalHeight;
    private float maxHeight;
    private bool buttonHeld;

    private void Start()
    {
        originalHeight = transform.localScale.y;
        maxHeight = 4 * originalHeight;

    }
    private void IsButtonHeld(BrokerEvent<IndicatorEvent.SprayPressed> @event)
    {
        buttonHeld = @event.Payload.ButtonHeld;
    }
    public IEnumerator ResetIndicatorLevel()
    {
        float elapsedTime = 0.0f;
        float currentHeight = transform.localScale.y;
        float scaleResetDuration = resetDuration * (currentHeight / maxHeight);

        while (elapsedTime < scaleResetDuration)
        {
            float newHeight = Mathf.Lerp(currentHeight, originalHeight, elapsedTime / scaleResetDuration);

            Vector3 newScale = transform.localScale;
            newScale.y = newHeight;
            transform.localScale = newScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set to the minimum height
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
            elapsedTime += Time.deltaTime;

            // Calculate the new height using lerp
            float newHeight = Mathf.Lerp(originalHeight, maxHeight, elapsedTime / resetDuration);

            // Set the new scale of the GameObject
            Vector3 newScale = transform.localScale;
            newScale.y = newHeight;
            transform.localScale = newScale;

            // Check if the scaling is complete
            if (elapsedTime >= resetDuration)
            {
                buttonHeld = false; // Reset the variable to stop scaling
                elapsedTime = 0.0f;   // Reset the elapsed time
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
