using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class AutoScaleUI : MonoBehaviour
{
    [SerializeField] private RectTransform TextComponent;
    [SerializeField] private RectTransform BackgroundTransform;
    private void Awake()
    {

        
    }

    private void Update()
    {
        
        // Adjust the button size / scale.
        BackgroundTransform.sizeDelta = new Vector2(BackgroundTransform.sizeDelta.x, TextComponent.sizeDelta.y);
    }
}
