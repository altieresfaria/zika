using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private List<GameObject> obstaculos;


    public Estado estado;

    public float espera;
    public GameObject obstaculo;
    public float tempoDestruicao;

    public static GameController instancia = null;
    public GameObject MenuCamera;
    public GameObject PanelMenu;

    public Text txtPontos;
    public Text txtMaiorPontos;
    private int pontos;

    public GameObject gameOverPanel;
    public GameObject pontosPanel;

    void Awake() {
        if (instancia == null) {
            instancia = this;
        }
        else if (instancia != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        obstaculos = new List<GameObject>();
        estado = Estado.AguardoComecar;
        PlayerPrefs.SetInt("HighScore", 0);
        MenuCamera.SetActive(true);
        PanelMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
    }

       IEnumerator DestruirObstaculo(GameObject obj) {
        yield return new WaitForSeconds(tempoDestruicao);
        if (obstaculos.Remove(obj)) {
            Destroy(obj);
        }
    }
   

    IEnumerator GerarObstaculos(){
        while (GameController.instancia.estado == Estado.Jogando){
            Vector3 pos = new Vector3(17f, Random.Range(-3f, 9f), 7.8f);
            GameObject obj = Instantiate(obstaculo, pos, Quaternion.Euler(0f,180f,0)) as GameObject;
            obstaculos.Add(obj);
            StartCoroutine(DestruirObstaculo(obj));
            yield return new WaitForSeconds(espera);
        }
        yield return null;
    }

    public void PlayerComecou(){
        MenuCamera.SetActive(false);
        PanelMenu.SetActive(false);
        pontosPanel.SetActive(true);
        StartCoroutine(GerarObstaculos());
        estado = Estado.Jogando;
        atualizarPontos(0);
        StartCoroutine(GerarObstaculos());
    }

    public void PlayerMorreu(){
        estado = Estado.GameOver;
        if (pontos > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", pontos);
            txtMaiorPontos.text = "" + pontos;
        }
        gameOverPanel.SetActive(true);
    }
    private void atualizarPontos(int x)
    {
        pontos = x;
        txtPontos.text = "" + x;
    }

    public void acrescentarPontos(int x)
    {
        atualizarPontos(pontos + 1);
    }

    public void PlayerVoltou()
    {
        estado = Estado.AguardoComecar;
        MenuCamera.SetActive(true);
        PanelMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
        GameObject.Find("meninoassustado").GetComponent<PlayerController>().recomecar();
    }



}
