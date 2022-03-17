using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsHand : MonoBehaviour
{
    public GameController controller;
    // Start is called before the first frame update

    public void UseCard()
    {
        controller.actiontext.text = (--GameController.actioncount).ToString();
        if (GameController.actioncount == 0)
        {
            foreach (Transform child in transform)
                if (child.gameObject.activeSelf)
                {
                    child.GetComponent<Collider2D>().enabled = false;
                    child.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
                }
        }
    }
    public void Shuffle()
    {
        Vector3 move = new Vector3(1 - GameController.handcount, 0, 0);
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.localPosition = move;
                child.GetComponent<Card>().x = move.x;
                move += Vector3.right * 2;
            }
        }
    }
}
