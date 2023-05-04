using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieController : MonoBehaviour
{
    public GameObject gameManager;
    private int disparos = 0;
    private int maximo = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bala")
        {
            Debug.Log("Colision de bala1");
            disparos++;
            Debug.Log(disparos);
            if (disparos >= maximo)
            {
                ZombiePuntos();
                Destroy(this.gameObject);
            }
        }

    }   
    private void ZombiePuntos()
    {
        gameManager = GameObject.Find("GameManager");
        var gm = gameManager.GetComponent<GameManager>();
        var uim = gameManager.GetComponent<UiManager>();

        gm.ZombiePuntos();
        uim.PrintZombie(gm.GetZombie());

        /*int cambio = gm.GetZombie();
        if(cambio > 1)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("Scena3");
        } */
    }
}
