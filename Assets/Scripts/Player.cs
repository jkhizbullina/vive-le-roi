using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        healthtext.text = health.ToString() + '/' + maxhealth.ToString();
        if (health < maxhealth)
        {
            healthbar.GetComponent<SpriteRenderer>().size = new Vector2(0.3f * health / maxhealth, 0.03f);
            healthbar.transform.position -= new Vector3((maxhealth - health) * 0.9f / maxhealth, 0, 0);
        }
        Reset();
    }

    public void Reset()
    {
        shield = 0;
        buff = 0;
        shieldsprite.SetActive(false);
        shieldtext.gameObject.SetActive(false);
        buffsprite.SetActive(false);
        bufftext.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Card" && other.GetComponent<Card>().f)
        {
            other.GetComponent<Card>().target = GetComponent<Collider2D>();
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            other.GetComponent<Card>().released = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Card")
        {
            other.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.25f, 0.25f);
            other.GetComponent<Card>().released = false;
        }
    }
}
