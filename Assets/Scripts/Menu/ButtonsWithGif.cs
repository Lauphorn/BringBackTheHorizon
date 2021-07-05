using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonsWithGif : MonoBehaviour
{

    public List<Sprite> One;
    public List<Sprite> Two;

    public List<List<Sprite>> Typos = new List<List<Sprite>>();

    public Image Img;
    bool hover, HoverDone;
    int place, actualImg;
    float timer;
    public float Delay;


    // Start is called before the first frame update
    void Start()
    {
        Typos.Add(One);
        Typos.Add(Two);
        Debug.Log(Typos.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (hover)
        {
            timer += Time.deltaTime;
            Img.sprite = Typos[place][actualImg];
            if(timer> Delay)
            {
                actualImg += 1;
                timer = 0;
                if (actualImg > 4)
                {
                    actualImg = 0;
                }
            }
        }
        else if(!HoverDone)
        {
            HoverDone = true;
            Img.sprite = Typos[place][Random.Range(0,4)];
        }
    }

}
