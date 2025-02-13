using UnityEngine;
using System.Runtime.InteropServices;

public class MobiileDetector : MonoBehaviour
{
    [SerializeField] private MobileHandling _mobilehandeling;
    public static bool isMobile = false;

    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool IsRunningOnMobile();
    #endif

    void Start()
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        isMobile = IsRunningOnMobile();
    #else
        isMobile = (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android);
    #endif

        Debug.Log("Is Mobile: " + isMobile);
        _mobilehandeling.enabled = true;
    }
}
