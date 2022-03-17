using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject buff_nonstatic;
    public GameObject debuff_nonstatic;
    public GameObject death_nonstatic;
    public static GameObject particleBuff;
    public static GameObject particleDebuff;
    public static GameObject particleDeath;

    public GameObject grid;
    public GameObject bossroom;
    public int gridpos;
    public GameObject cardfield;
    public GameObject[] cards;
    public int[] counts;
    public static List<GameObject> draw;
    public static List<GameObject> hand;
    public static List<GameObject> discard;
    public Text drawtext;
    public Text discardtext;
    public Text actiontext;
    public static int actioncount;
    public static int handcount;
    public GameObject turnbutton;

    public GameObject player_nonstatic;
    public static GameObject player;

    public GameObject music;
    public GameObject Darkscreen;

    public GameObject StartScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject PauseScreen;

    List<GameObject> enemies;

    public GameObject[] enemy_prefabs;
    public GameObject boss;

    public static bool live = true;

    // Start is called before the first frame update
    void Start()
    {
        Agent.warnDeath += WarnedDeath;
        Agent.delegateDeath += DeadBody;
        particleBuff = buff_nonstatic;
        particleDebuff = debuff_nonstatic;
        particleDeath = death_nonstatic;
        player = player_nonstatic;
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        draw = new List<GameObject>();
        hand = new List<GameObject>();
        discard = new List<GameObject>();
        for (int i = 0; i < cards.Length && i < counts.Length; i++)
            for (int j = 0; j < counts[i]; j++)
            {
                draw.Add(Instantiate(cards[i], cardfield.transform));
                draw.Last().SetActive(false);
            }
        drawtext.text = draw.Count.ToString();
        discardtext.text = "0";
        music.GetComponent<MusicChange>().Change();
        if (PlayerPrefs.HasKey("volume"))
            music.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
        if (PlayerPrefs.HasKey("health") && PlayerPrefs.HasKey("screen"))
        {
            LoadPrefs();
        }
        else
        {
            enemies.Add(Instantiate(enemy_prefabs[Random.Range(0, enemy_prefabs.Length)], new Vector3(4.5f, 0, 0), Quaternion.identity));
            StartScreen.SetActive(true);
            live = false;
        }
        
        MoveGrid();
        NewTurn();
    }

    void MoveGrid()
    {
        Vector3 newpos = Vector3.left;
        switch (gridpos)
        {
            case 0:
                newpos *= 42;
                break;
            case 1:
                newpos *= -4.5f;
                break;
            case 2:
                newpos *= 13;
                break;
            case 3:
                newpos *= 30;
                break;
            case 4:
                grid.SetActive(false);
                bossroom.SetActive(true);
                return;
            default:
                break;
        }
        grid.transform.position = newpos + 1.5f * Vector3.up;
    }

    // Update is called once per frame
    public void NewTurn(int x = 5, int a = 3)
    {
        actioncount = a;
        actiontext.text = actioncount.ToString();
        handcount = x;
        GameObject temp;
        if (draw.Count < x)
        {
            x -= draw.Count;
            int k = draw.Count;
            for (int i = 0; i < k; i++)
            {
                temp = draw[Random.Range(0, draw.Count)];
                draw.Remove(temp);
                hand.Add(temp);
                temp.SetActive(true);
            }
            draw.Clear();
            draw.AddRange(discard);
            discard.Clear();
        }
        for (int i = 0; i < x; i++)
        {
            temp = draw[Random.Range(0, draw.Count)];
            draw.Remove(temp);
            hand.Add(temp);
            temp.SetActive(true);
        }
        drawtext.text = draw.Count.ToString();
        discardtext.text = discard.Count.ToString();
        cardfield.GetComponent<CardsHand>().Shuffle();
        turnbutton.GetComponent<Button>().interactable = true;
    }

    public void WarnedDeath(GameObject body)
    {
        if (body.tag == "Enemy") enemies.Remove(body);
        else
        {
            live = false;
            turnbutton.GetComponent<Button>().interactable = false;
        }
    }

    public void DeadBody(GameObject body)
    {
        if (body.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine("GameOver");
            live = false;
            Darkscreen.SetActive(true);
            Darkscreen.GetComponent<Darkening>().f = false;
        }
        else if (body.tag == "Enemy")
        {
            enemies.Remove(body);
            if (gridpos > 3)
            {
                StopAllCoroutines();
                StartCoroutine("GameWon");
                live = false;
                Darkscreen.SetActive(true);
                Darkscreen.GetComponent<Darkening>().f = false;
            }
            else if (enemies.Count < 1)
            {
                live = true;
                StartCoroutine("ScreenChange");
                Darkscreen.SetActive(true);
                Darkscreen.GetComponent<Darkening>().f = true;
            }
            else
            {
                live = true;
                turnbutton.GetComponent<Button>().interactable = true;
            }
            Destroy(body);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.1f);
        player.GetComponent<Player>().health = player.GetComponent<Player>().maxhealth;
        gridpos = 0;
        PlayerPrefs.DeleteKey("health");
        PlayerPrefs.DeleteKey("screen");
        LoseScreen.SetActive(true);
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(1.1f);
        player.GetComponent<Player>().health = player.GetComponent<Player>().maxhealth;
        gridpos = 0;
        PlayerPrefs.DeleteKey("health");
        PlayerPrefs.DeleteKey("screen");
        WinScreen.SetActive(true);
    }

    IEnumerator ScreenChange()
    {
        yield return new WaitForSeconds(1.1f);
        EndTurn();
        if (gridpos < 3)
        {
            gridpos++;
            enemies.Add(Instantiate(enemy_prefabs[Random.Range(0, enemy_prefabs.Length)], new Vector3(4.5f, 0, 0), Quaternion.identity));
            if (gridpos == 2) enemies.Add(Instantiate(enemy_prefabs[Random.Range(0, enemy_prefabs.Length)], new Vector3(2.5f, 0, 0), Quaternion.identity));
        }
        else
        {
            gridpos = 4;
            enemies.Add(Instantiate(boss, new Vector3(3.8f, -0.3f, 0), Quaternion.identity));
            music.GetComponent<MusicChange>().Change(true);
        }
        MoveGrid();
        draw.AddRange(discard);
        discard.Clear();
        player.GetComponent<Player>().Reset();
        SavePrefs();
    }

    void LoadPrefs()
    {
        player.GetComponent<Player>().health = PlayerPrefs.GetInt("health");
        gridpos = PlayerPrefs.GetInt("screen");
        if (gridpos > 3)
        {
            enemies.Add(Instantiate(boss, new Vector3(4.5f, -0.3f, 0), Quaternion.identity));
            music.GetComponent<MusicChange>().Change(true);
        }
        else
        {
            enemies.Add(Instantiate(enemy_prefabs[Random.Range(0, enemy_prefabs.Length)], new Vector3(5.5f, 0, 0), Quaternion.identity));
            if (gridpos == 2) enemies.Add(Instantiate(enemy_prefabs[Random.Range(0, enemy_prefabs.Length)], new Vector3(2.5f, 0, 0), Quaternion.identity));
        }
    }

    void SavePrefs()
    {
        PlayerPrefs.SetInt("health", player.GetComponent<Player>().health);
        PlayerPrefs.SetInt("screen", gridpos);
        PlayerPrefs.Save();
    }

    public void EndTurn()
    {
        StartCoroutine("EnemiesTurn");
        foreach (GameObject card in hand)
        {
            card.GetComponent<Collider2D>().enabled = true;
            card.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            discard.Add(card);
            card.SetActive(false);
        }
        hand.Clear();
    }

    IEnumerator EnemiesTurn()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Activate();
            yield return new WaitForSeconds(1f);
            enemy.GetComponent<Enemy>().NewTurn();
        }
        yield return new WaitForSeconds(0.5f);
        NewTurn();
    }

    public void Pause(bool f = true)
    {
        live = f;
        if (f)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }
}
