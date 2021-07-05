using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings _instance;
    public static Settings Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public enum Lang { FR, ENG };
    public Lang currentLang;

    public enum shadowquality {low,med,hig,ult};
    public shadowquality currentShadowQuality;
}
