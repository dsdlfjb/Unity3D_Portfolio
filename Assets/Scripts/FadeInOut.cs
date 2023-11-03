// 포탈 이동할 때 화면이 까맣게 fade in & fade out 되게끔 변환하는 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] bool _fadeIn = false;
    [SerializeField] bool _fadeOut = false;

    public float _timeToFade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadeIn == true)
        {
            if (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += _timeToFade * Time.deltaTime;
                if (_canvasGroup.alpha >= 1)
                {
                    _fadeIn = false;
                }
            }
        }

        if (_fadeOut == true)
        {
            if (_canvasGroup.alpha >= 0)
            {
                _canvasGroup.alpha -= _timeToFade * Time.deltaTime;
                if (_canvasGroup.alpha == 0)
                {
                    _fadeOut = false;
                }
            }
        }
    }

    public void FadeIn() { _fadeIn = true; }
    public void FadeOut() { _fadeOut = true; }
}
