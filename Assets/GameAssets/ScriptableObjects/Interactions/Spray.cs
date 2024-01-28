using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spray : MonoBehaviour
{

    [SerializeField] private SplineRenderer spline;
    float animationTime = 1f;

    private bool canBePressed = false;
    private bool isButtonPressed = false;
    private bool sprayTriggered = false;
    private float timer = 0f;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private bool flowMutex = false;

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameSystemEvents.SpawnAnimal>(OnSpawnAnimal);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.SpawnAnimal>(OnSpawnAnimal);
    }

    private void OnSpawnAnimal(BrokerEvent<GameSystemEvents.SpawnAnimal> @event)
    {
        canBePressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isButtonPressed)
        {
            // Increment the timer while the button is pressed
            timer += Time.deltaTime;

            switch (Mathf.Floor(timer))
            {
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case 2:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                default:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
  
            }
        }
    }

    private void OnMouseDown()
    {
        if (!canBePressed) return;

        if (sprayTriggered) return;

        // Start the timer when the mouse button is pressed on the button
        isButtonPressed = true;
        sprayTriggered = true;
        timer = 0f;

        StartCoroutine(Flow(true));
    }

    private void OnMouseUp()
    {
        if (!canBePressed) return;
        if (!sprayTriggered) return;
        // Stop the timer and save the duration when the mouse button is released
        isButtonPressed = false;
        canBePressed = false;

        StartCoroutine(Flow(false, () =>
        {
            float buttonPressDuration = Mathf.Clamp(timer, 1f, 3f);

            switch (buttonPressDuration)
            {
                case 1:
                    eventBrokerComponent.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.Low));
                    break;
                case 2:
                    eventBrokerComponent.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.Medium));
                    break;
                case 3:
                    eventBrokerComponent.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.High));
                    break;
                default:
                    Debug.Log("Fail");
                    break;
            }

            sprayTriggered = false;
        }));

    }

    private IEnumerator Flow(bool forward, Action onFinish=null)
    {
        // If this Coroutine is called multiple times at once, only one can manipulate the flow
        if (!flowMutex)
        {
            flowMutex = true;
        } else
        {
            yield return new WaitUntil(() => !flowMutex);
            flowMutex = true;
        }

        double currentFlow = spline.clipTo;
        double targetValue = forward ? 1 : 0;

        for (float elapsedTime = 0f; elapsedTime < animationTime; elapsedTime += Time.deltaTime)
        {
            spline.clipTo = Mathf.Lerp((float)currentFlow, (float)targetValue, elapsedTime / animationTime);
            yield return null;
        }
        spline.clipTo = targetValue;
        onFinish?.Invoke();
        flowMutex = false;
    }
}
