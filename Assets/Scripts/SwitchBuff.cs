using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuff : MonoBehaviour
{
    public Sprite buff;
    public Sprite debuff;

    public void Switch(int x = 0)
    {
        if (x >= 0) GetComponent<SpriteRenderer>().sprite = buff;
        else GetComponent<SpriteRenderer>().sprite = debuff;
    }
}
