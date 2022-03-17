using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TypeText : MonoBehaviour
{
    string descr;
    Text txt;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
        descr = txt.text;
        txt.text = "";
        StartCoroutine("Type");
    }

    // Update is called once per frame
    IEnumerator Type()
    {
        foreach (char c in descr)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        if (obj != null)
        {
            yield return new WaitForSeconds(0.5f);
            obj.SetActive(true);
        }
    }
}
