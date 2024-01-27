using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isDragging = true;
    private bool inDropZone = false;
    private Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            // Update the object's position based on the mouse position
            transform.position = GetMouseWorldPos() + offset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPos(), Vector2.zero);

            if (inDropZone)
            {
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            else if(hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the other GameObject has the specified name
        if (collision.gameObject.tag == "DropZone")
        {
            inDropZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the other GameObject has the specified name
        if (collision.gameObject.tag == "DropZone")
        {
            inDropZone = false;
        }
    }


    private Vector2 GetMouseWorldPos()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; // Set the z-coordinate to the camera's position
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
