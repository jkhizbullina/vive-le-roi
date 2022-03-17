using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public float x, y;
    public bool f = true;
    public bool released = false;
    public int type = 0;
    public int mod = 0;
    public Collider2D target;

    void OnMouseDown()
    {
        if (GameController.live)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.25f, 0.25f);
            transform.position += Vector3.back;
        }
    }

    void OnMouseUp()
    {
        if (released && GameController.live)
        {
            if (target.tag == "Player" && f)
            {
                switch (type)
                {
                    case 0:
                        target.GetComponent<Player>().RaiseShield(mod);
                        break;
                    case 1:
                        target.GetComponent<Player>().UseBuff(mod);
                        break;
                    case 2:
                        target.GetComponent<Player>().IncreaseHealth(mod);
                        break;
                    default:
                        break;
                }
            }
            else if (target.tag == "Enemy" && !f)
            {
                switch (type)
                {
                    case 0:
                        target.GetComponent<Enemy>().Damage(mod + GameController.player.GetComponent<Player>().buff);
                        break;
                    case 1:
                        target.GetComponent<Enemy>().UseBuff(mod);
                        break;
                    default:
                        break;
                }
            }
            GameController.hand.Remove(gameObject);
            GameController.discard.Add(gameObject);
            GameController.handcount--;
            gameObject.SetActive(false);
            transform.GetComponentInParent<CardsHand>().UseCard();
            transform.GetComponentInParent<CardsHand>().Shuffle();
        }
        transform.position = new Vector3(x + transform.parent.position.x, y + transform.parent.position.y, transform.parent.position.z);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0,0);
    }

    // Start is called before the first frame update
    void OnMouseDrag()
    {
        if (GameController.live)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }
}
