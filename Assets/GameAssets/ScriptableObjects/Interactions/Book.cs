using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] private GameObject displayGO;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private Vector2 startPosition;
    private bool isDragging = false;
    private bool allow = true;


    void Update()
    {
        if (isDragging)
        {
            // Check if the mouse is being dragged
            Vector2 currentPosition = Input.mousePosition;

            if (Vector2.Distance(startPosition, currentPosition) > 5f)  // You can adjust this threshold
            {
                transform.position = GetMouseWorldPos();
            }
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        // Mouse button is pressed, start recording the initial position
        startPosition = Input.mousePosition;
        isDragging = true;
        allow = true;

    }

    private void OnMouseUp()
    {
        // Mouse button is released, stop recording and reset the flag
        isDragging = false;

        // Check if it was just a click
        if (Vector2.Distance(startPosition, Input.mousePosition) <= 5f && allow)  // You can adjust this threshold
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Invoke("OpenBook", 0.01f);
            allow = false;
        }
    }

    private void OpenBook()
	{
		displayGO.SetActive(true);
	}

    private void DayEnd(BrokerEvent<GameSystemEvents.EndDay> @event)
    {
        displayGO.SetActive(false);
    }
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<GameSystemEvents.EndDay>(DayEnd);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<GameSystemEvents.EndDay>(DayEnd);
    }

    private Vector2 GetMouseWorldPos()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; // Set the z-coordinate to the camera's position
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
