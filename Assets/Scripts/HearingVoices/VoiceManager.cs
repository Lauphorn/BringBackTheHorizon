using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float frequency;

    [Range(0.0f, 1.0f)]
    public float frequencyRandomizer;

    public float timer;
    float randomValue;

    public GameObject VoiceSubtitlePrefab;
    public Transform VoiceSubtitleParent;
    GameObject instantiatedSubtitle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (frequency > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                launchVoice();

                randomValue = Random.Range(-frequency*randomValue, frequency * randomValue);
                timer = frequency-randomValue;
            }
        }

    }

    void launchVoice()
    {
        float anchorX = Random.Range(0.0f, 1.0f);
        float anchorY = Random.Range(0.0f, 1.0f);
        instantiatedSubtitle = Instantiate(VoiceSubtitlePrefab, VoiceSubtitleParent);
        instantiatedSubtitle.GetComponent<RectTransform>().anchorMin = new Vector2(anchorX,anchorY);
        instantiatedSubtitle.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX, anchorY);
    }
}
