using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSystem : MonoBehaviour
{
    [SerializeField] private List<Animator> smokes;
    
    public void Spray()
    {
        foreach (var anim in smokes)
        {
            anim.Play("SmokeGrow");
        }
    }
}
