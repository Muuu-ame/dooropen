using UnityEngine;
using System.Runtime.InteropServices;

public class JsBrige : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        Debug.Log("Unity: OnBackButtonClick called");
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval("window.parent.postMessage({ type: 'BackButton' }, '*');");
#else
        Debug.Log("BackButton (Editor stub)");
#endif
    }
    public void OnNextButtonClick()
    {
        Debug.Log("Unity: OnNextButtonClick called");
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval("window.parent.postMessage({ type: 'NextButton' }, '*');");
#else
        Debug.Log("NextButton (Editor stub)");
#endif
    }
}