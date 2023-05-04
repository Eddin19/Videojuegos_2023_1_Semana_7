using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //Salto
    bool isJump = false;
    bool canJump =  true;
    public float jumpForce = 1000f;
    public float waitJump = 0.85f;

    public event EventHandler muerteJugador;

    public GameObject gameOverText;

    public AudioClip[] audios;
    public GameObject gameManager;
    public GameObject bala;
    
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private Transform balaTransform;
    
    private int currentAnimation = 1;
    public float tiempoDeRetraso = 2.0f;
    public int vida = 2;
    public bool Muerte = false;
    private bool balaExiste = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        cambiar();
        animator.SetInteger("Estado", currentAnimation);
    }
    public void clickJump()
    {
        isJump = true;
    }
    public void Salto()
    {
        if(isJump && canJump)
        {
            audioSource.PlayOneShot(audios[0], 0.2f);
            isJump = false;
            rb.AddForce(transform.up * jumpForce);
            canJump = false;
            Invoke("waitToJump", waitJump);
        }
    }
    private void waitToJump()
    {
        canJump = true;
    }
    public void Disparar() {

      //if(Input.GetKeyUp(KeyCode.U)) {
            //GanarPuntos();

            var gm = gameManager.GetComponent<GameManager>();
            var uim = gameManager.GetComponent<UiManager>();

            int disparo = gm.GetPuntaje();
            if (disparo > 0 )
            {
                audioSource.PlayOneShot(audios[1]);
                balaExiste = true;
                var position = transform.position;
                var x = position.x + 1;
                var newPosition = new Vector3(x, position.y, position.z);

                GameObject balaGenerada = Instantiate(bala, newPosition, Quaternion.identity);
                balaTransform = balaGenerada.transform;
                gm.PerderPuntos();
                uim.PrintPuntaje(gm.GetPuntaje());

            }
        //}
    }

    public void DividirDisparo() {
      // solo se divide si la bala existe y presiono C
      //transform.position
      if(balaExiste) {

        var position = balaTransform.position;
        var positionBala2 = new Vector3(position.x + 1, position.y + 1, position.z); 
        var positionBala3 = new Vector3(position.x + 1, position.y - 1, position.z); 

        GameObject balaGenerada2 = Instantiate(bala, positionBala2, Quaternion.identity);

        (balaGenerada2.GetComponent<BalaController>()).velocityY = 1;

        GameObject balaGenerada3 = Instantiate(bala, positionBala3, Quaternion.identity);

        (balaGenerada3.GetComponent<BalaController>()).velocityY = -1;
      }
    }

    private void GanarPuntos() {
      var gm = gameManager.GetComponent<GameManager>();
      var uim = gameManager.GetComponent<UiManager>();

      gm.GanarPuntos();
      uim.PrintPuntaje(gm.GetPuntaje());
    }
    private void GanarBalas()
    {
        var gm = gameManager.GetComponent<GameManager>();
        var uim = gameManager.GetComponent<UiManager>();

        gm.BalasPuntos();
        uim.PrintZombie(gm.GetBalas());
    }

    //Colisión para morir
    private void OnCollisionEnter2D(Collision2D other){
        Muerte = false;
        currentAnimation = 1;
      if (other.gameObject.CompareTag("Enemy")){

          audioSource.PlayOneShot(audios[3], 0.5f);
          gameManager = GameObject.Find("GameManager");
          var gm = gameManager.GetComponent<GameManager>();
          var uim = gameManager.GetComponent<UiManager>();

          gm.PerderVidas();
          uim.PrintVida(gm.GetVidas());
            
          int morir = gm.GetVidas();

          if(morir == 0){
            audioSource.PlayOneShot(audios[4]);
            Muerte = true;
            Debug.Log("Muerte");
            currentAnimation = 2;
            Invoke("DetenerTiempo", tiempoDeRetraso);
                muerteJugador?.Invoke(this, EventArgs.Empty);
            } 

          
          //Destroy(this.gameObject);
          //currentAnimation = 5;

      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que ha entrado en el trigger tiene la etiqueta "Potion", entonces lo hacemos desaparecer
        if (other.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(audios[2]);
            //Debug.Log("OnTrigger");
            other.gameObject.SetActive(false);
            GanarPuntos();
            //jumpEnemy = true;
        }
    }


    private void DetenerTiempo()
    {
        Time.timeScale = 0;
    }

    public void MoverIzquierda()
    {
        rb.velocity = new Vector2(-10, rb.velocity.y);
        sr.flipX = true;
    }
    public void MoverDerecha()
    {
        rb.velocity = new Vector2(10, rb.velocity.y);
        sr.flipX = false;
    }
    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void cambiar()
    {
        var gm = gameManager.GetComponent<GameManager>();
        var uim = gameManager.GetComponent<UiManager>();
        int cambio = gm.GetZombie();
        if (cambio > 1)
        {
            SceneManager.LoadScene("Scena3");
        }
    }
}
