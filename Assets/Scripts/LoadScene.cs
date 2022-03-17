using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load(int id = 0)
    {
        SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
    }
}
