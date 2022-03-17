using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Agent
{
    public GameObject intent;
    public int intention;
    public int attack;
    public int d_shield;
    public int d_buff;
    public int d_debuff;
    public int chance_attack;
    public int chance_buff;
    public int chance_debuff;

    void Start()
    {
        healthtext.text = health.ToString() + '/' + maxhealth.ToString();
        intent.GetComponent<Intent>().Change(intention);
        if (health < maxhealth)
        {
            healthbar.GetComponent<SpriteRenderer>().size = new Vector2(0.3f * health / maxhealth, 0.03f);
            healthbar.transform.position -= new Vector3((maxhealth - health) * 0.9f / maxhealth, 0, 0);
        }
    }

    public void Activate()
    {
        switch (intention)
        {
            case 0:
                sprite.GetComponent<Animator>().SetTrigger("Attack");
                GameController.player.GetComponent<Player>().Damage(attack + buff);
                break;
            case 1:
                RaiseShield(d_shield);
                break;
            case 2:
                UseBuff(d_buff);
                break;
            case 3:
                GameController.player.GetComponent<Player>().UseBuff(d_debuff);
                break;
            default:
                break;
        }
    }

    public void NewTurn()
    {
        int chance = Random.Range(0, 100);
        int old = intention;
        if (chance < chance_attack && shield < 1) intention = 1;
        else if (chance < chance_buff || chance < chance_attack) intention = 0;
        else if (chance < chance_debuff) intention = 2;
        else intention = 3;
        if (intention != old)
            intent.GetComponent<Intent>().Change(intention);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Card" && !other.GetComponent<Card>().f)
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
