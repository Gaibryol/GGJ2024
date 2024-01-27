using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class MixerTube : MonoBehaviour
{
    [SerializeField] private SplineRenderer spline;
    // Start is called before the first frame update
    float animationTime = 3f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StopAllCoroutines();

            StartCoroutine(Flow(true));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StopAllCoroutines();
            StartCoroutine(Flow(false));

        }
    }

    private IEnumerator Flow(bool forward)
    {
        double currentFlow = spline.clipTo;
        double targetValue = forward ? 1 : 0;

        for (float elapsedTime = 0f; elapsedTime < animationTime; elapsedTime += Time.deltaTime)
        {
            spline.clipTo = Mathf.Lerp((float)currentFlow, (float)targetValue, elapsedTime / animationTime);
            yield return null;
        }
        spline.clipTo = targetValue;
    }

}
