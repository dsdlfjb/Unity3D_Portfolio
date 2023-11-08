using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static LoadingSceneController _instance;
    public static LoadingSceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                    _instance = obj;
                else
                    _instance = Create();
            }
            return _instance;
        }
    }

    static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Image _progressBar;

    string _loadSceneName;

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        _loadSceneName = sceneName;
        StartCoroutine(Coroutine_LoadSceneProgress());
    }

    IEnumerator Coroutine_LoadSceneProgress()
    {
        _progressBar.fillAmount = 0f;
        yield return StartCoroutine(Coroutine_Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(_loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                _progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if (_progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == _loadSceneName)
        {
            StartCoroutine(Coroutine_Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    IEnumerator Coroutine_Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            _canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
