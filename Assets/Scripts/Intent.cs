using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intent : MonoBehaviour
{
    public Sprite attack, defend, debuff, buff;
    // Start is called before the first frame update
    public void Change(int x = 0)
    {
        if (x == 0) gameObject.GetComponent<SpriteRenderer>().sprite = attack;
        else if (x == 1) gameObject.GetComponent<SpriteRenderer>().sprite = defend;
        else if (x == 2) gameObject.GetComponent<SpriteRenderer>().sprite = buff;
        else if (x == 3) gameObject.GetComponent<SpriteRenderer>().sprite = debuff;
    }
}
