using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkening : MonoBehaviour
{
    public bool f;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Darken());
    }

    // Update is called once per frame
    IEnumerator Darken()
    {
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime * 2);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.5f);
        if (f) StartCoroutine("Lighten");
    }

    IEnumerator Lighten()
    {
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime * 2);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
    }
}
