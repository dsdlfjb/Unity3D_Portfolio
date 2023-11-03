using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    FadeInOut _fade;

    // Start is called before the first frame update
    void Start()
    {
        _fade = FindObjectOfType<FadeInOut>();
        _fade.FadeOut();
    }
}
