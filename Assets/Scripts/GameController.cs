﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    public Estado estado;

    public float espera;
    public GameObject obstaculo;
    public float tempoDestruicao;
    
    public static GameController instancia = null;
    public GameObject menu;
    public GameObject PanelMenu;

    public Text txtPontos;
    private int pontos;

	void Awake () {
        if(instancia == null){
            instancia = this;
        }
        else if (instancia != null){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

    void Start(){
        estado = Estado.AguardoComecar;
    }

    IEnumerator GerarObstaculos(){
        while (GameController.instancia.estado == Estado.Jogando){
            Vector3 pos = new Vector3(17f, Random.Range(-3f, 9f), 7.8f);
            GameObject obj = Instantiate(obstaculo, pos, Quaternion.Euler(0f,180f,0)) as GameObject;
            Destroy(obj, tempoDestruicao);
            yield return new WaitForSeconds(espera);
        }
        yield return null;
    }

    public void PlayerComecou(){
        menu.SetActive(false);
        PanelMenu.SetActive(false);
        StartCoroutine(GerarObstaculos());
        estado = Estado.Jogando;
        atualizarPontos(0);
        StartCoroutine(GerarObstaculos());
    }

    public void PlayerMorreu(){
        estado = Estado.GameOver;
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


}
