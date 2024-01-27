using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    private bool isButtonPressed = false;
    private float timer = 0f;

    private EventBrokerComponent eventBroker = new EventBrokerComponent();
    // Update is called once per frame
    void Update()
    {
        if (isButtonPressed)
        {
            // Increment the timer while the button is pressed
            timer += Time.deltaTime;
            if (Mathf.Floor(timer) == 0)
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            else if (Mathf.Floor(timer) == 1)
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            else if (Mathf.Floor(timer) == 2)
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            else if (Mathf.Floor(timer) == 3)
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        }
    }

    private void OnMouseDown()
    {
        // Start the timer when the mouse button is pressed on the button
        isButtonPressed = true;
        timer = 0f;
    }

    private void OnMouseUp()
    {
        // Stop the timer and save the duration when the mouse button is released
        isButtonPressed = false;
        float buttonPressDuration = timer;
        if (Mathf.Floor(buttonPressDuration) == 0)
            Debug.Log("Too Short");
        else if (Mathf.Floor(buttonPressDuration) == 1)
            eventBroker.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.Low));
        else if (Mathf.Floor(buttonPressDuration) == 2)
            eventBroker.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.Medium));
        else if (Mathf.Floor(buttonPressDuration) == 3)
            eventBroker.Publish(this, new GameSystemEvents.AnimalSprayed(Constants.GameSystem.SprayLevel.High));


    }
}
