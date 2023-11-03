// ��Ż �̵��� �� ȭ���� ��İ� fade in & fade out �ǰԲ� ��ȯ�ϴ� ��ũ��Ʈ
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
