using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetItems : MonoBehaviour
{
    public ItemsManager IM;

    private void OnMouseDown()
    {
        IM.ResetItem();
    }
}
