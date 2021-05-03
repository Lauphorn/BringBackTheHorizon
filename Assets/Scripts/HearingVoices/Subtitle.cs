using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour
{
    Text texte;
    public float fadeSpeed;

    public float fadeStrength;

    public float length;
    float Timer;
    bool Done;



    // Start is called before the first frame update
    void Start()
    {
        texte = GetComponent<Text>();
        texte.fontSize = Random.Range(50, 200);
        texte.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-20, 20))); 
        fadeSpeed = Random.Range(1f, 2f);
        fadeStrength = Random.Range(0.9f, 1f);
        length = Random.Range(3f, 4f);
        StartCoroutine(FadeTextToFullAlpha(fadeSpeed, texte));
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer>= length && !Done)
        {
            StartCoroutine(FadeTextToZeroAlpha(fadeSpeed, texte));
            Done = true;
        }
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i.color.a < fadeStrength)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        Destroy(gameObject);
    }
}
