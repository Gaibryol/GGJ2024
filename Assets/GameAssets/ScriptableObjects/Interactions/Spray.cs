using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    private bool isButtonPressed = false;
    private float timer = 0f;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

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
        // Start the timer when the mouse button is pressed on the button
        isButtonPressed = true;
        timer = 0f;
    }

    private void OnMouseUp()
    {
        // Stop the timer and save the duration when the mouse button is released
        isButtonPressed = false;
        float buttonPressDuration = Mathf.Clamp(timer, 1, 3);

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
    }
}
