using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public GameObject healthfull;
    public GameObject healthbar;
    public GameObject shieldsprite;
    public GameObject buffsprite;
    public GameObject sprite;
    public Text healthtext;
    public Text shieldtext;
    public Text bufftext;

    public int maxhealth, health;

    public int shield, buff;

    public delegate void DeathDelegate(GameObject body);
    public static DeathDelegate delegateDeath;
    public static DeathDelegate warnDeath;

    public void Damage(int x = 0)
    {
        if (x > 0)
        {
            x = ReduceShield(x);
            if (x > 0)
            {
                ReduceHealth(x);
            }
            else
            {
                sprite.GetComponent<Animator>().SetTrigger("Shield");
            }
        }
    }

    protected void ReduceHealth(int x)
    {
        if (x >= health)
        {
            health = 0;
            healthbar.SetActive(false);
            StartCoroutine("Death");
        }
        else
        {
            health -= x;
            healthbar.GetComponent<SpriteRenderer>().size = new Vector2(0.3f * health / maxhealth, 0.03f);
            healthbar.transform.position -= new Vector3(x * 0.9f / maxhealth, 0, 0);
            sprite.GetComponent<Animator>().SetTrigger("Damage");
        }
        healthtext.text = health.ToString() + '/' + healthtext.text.Split('/')[1];

    }

    protected IEnumerator Death()
    {
        warnDeath(gameObject);
        sprite.GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(0.5f);
        Instantiate(GameController.particleDeath, transform.position, Quaternion.Euler(new Vector3(45, 45, 0)));
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        delegateDeath(gameObject);
    }

    public void RaiseShield(int x)
    {
        if (shield == 0)
        {
            shieldsprite.SetActive(true);
            shieldtext.gameObject.SetActive(true);
            shield = x;
        }
        else shield += x;
        shieldtext.text = shield.ToString();
        sprite.GetComponent<Animator>().SetTrigger("Shield");
        Instantiate(GameController.particleBuff, transform.position, Quaternion.Euler(90 * Vector3.right));
    }

    public void UseBuff(int x)
    {
        if (x > 0) Instantiate(GameController.particleBuff, transform.position, Quaternion.Euler(90 * Vector3.right));
        else if (x < 0) Instantiate(GameController.particleDebuff, transform.position + Vector3.up * 3, Quaternion.Euler(90 * Vector3.left));
        if (buff == 0 && x != 0)
        {
            buffsprite.SetActive(true);
            bufftext.gameObject.SetActive(true);
        }
        buff += x;
        if (buff < 0)
            buffsprite.GetComponent<SwitchBuff>().Switch(-1);
        else if (buff == 0)
        {
            buffsprite.SetActive(false);
            bufftext.gameObject.SetActive(false);
        }
        else buffsprite.GetComponent<SwitchBuff>().Switch(1);
        bufftext.text = buff.ToString();
    }

    public void IncreaseHealth(int x)
    {
        Instantiate(GameController.particleBuff, transform.position, Quaternion.Euler(90 * Vector3.right));
        if (x + health > maxhealth)
        {
            x = maxhealth - health;
            health = maxhealth;
        }
        else
            health += x;
        healthbar.GetComponent<SpriteRenderer>().size = new Vector2(0.3f * health / maxhealth, 0.03f);
        healthbar.transform.position += new Vector3(x * 0.9f / maxhealth, 0, 0);
        healthtext.text = health.ToString() + '/' + healthtext.text.Split('/')[1];
    }

    protected int ReduceShield(int x)
    {
        if (shield <= 0)
        {
            shieldsprite.SetActive(false);
            shieldtext.gameObject.SetActive(false);
            return x;
        }
        else if (shield >= x)
        {
            shield -= x;
            shieldtext.text = shield.ToString();
            return 0;
        }
        else
        {
            shieldsprite.SetActive(false);
            shieldtext.gameObject.SetActive(false);
            int k = shield;
            shield = 0;
            return x - k;
        }
    }
}
