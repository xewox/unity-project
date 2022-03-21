﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager gm;
    private GameObject[] obj;

    #region 각종 필요 속성 선언
    public int myGold = 0;
    public GameObject TowerPrefab;
    public GameObject RoundButton;
    public int PlayRound;
    public int nowRound = 0;
    #endregion
    // UI Text Variable
    Text UItext;
    private void Awake()
    {
        if (gm == null)
            gm = this;
    }
    // 게임상태 상수
    public enum GameState
    {
        Ready, // 게임 준비 상태
        Play, // 게임 실행
        GameOver // 게임종료
    }
    // 게임상태 변수
    public GameState gState;

    // Start is called before the first frame update
    void Start()
    {
        // 게임 초기상태 준비 상태 설정

        // 게임 실행상태 설정
        gState = GameState.Play;
        Debug.Log("Starting");
        this.veiling();
        int Hp = GameObject.Find("Castle").GetComponent<TowerHP>().CastleHp;
        GameObject.Find("hpOfCastle").GetComponent<Text>().text = "Castle HP : " + Hp.ToString();
        // 게임종료상태 설정

    }

    // Update is called once per frame
    void Update()
    {
        //int Hp = GameObject.Find("Castle").GetComponent<TowerHP>().CastleHp;
        //GameObject.Find("hpOfCastle").GetComponent<Text>().text = "Castle HP : " + Hp.ToString();
    }
    #region 각종 기능 함수

    #region target 초기화
    private void veiling()
    {
        obj = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject i in obj)
        {
            i.SetActive(false);
            string message = i.ToString();
        }
    }
    #endregion

    #region 골드 획득 함수
    // 골드 획득량 증가 함수
    public void GetGold(GameObject obj) {
        myGold++;
        nowGold();
        Debug.Log("Gold : " + myGold.ToString());
        obj.GetComponent<Coin>().Collect();
    }
    #endregion

    #region 웨이브 시작함수
    // 웨이브 시작함수
    public void WaveStart(GameObject obj) {
        Debug.Log("TEST : ");
        // Data Chaning by wave
        nowRound++;
        this.NowRound();
        // data = {라운드 시작되는 term, 소환 interval, 반복횟수}
        // {x, y, z} => x초 후에 y초 간격으로 z번 반복해라
        float[] data = new float[3]{1, 2, nowRound * 5};
        GameObject.Find("Gate").SendMessage("WaveStart", data);
        Destroy(obj);
    }
    #endregion

    #region 타워생성
    // 타워 생성
    public void CreateTower(GameObject obj) {
        if (myGold >= 5) {
            myGold -= 5;
            Instantiate(TowerPrefab, obj.transform.position, obj.transform.rotation);
            Destroy(obj);
        }
        else {
            Debug.Log("Message : not enough Gold");
        }
    }
    #endregion
    
    #region 시작버튼 생성
    // 시작버튼 생성
    public void WaveStartButton() {
        GameObject.Find("Gate").GetComponent<WaveSpawn>().SendMessage("CreateButton");
    }
    #endregion

    #region 상태창 텍스트 
    // Round 텍스트 변경
    public void NowRound() {
        string s = "현재 라운드 : " + nowRound.ToString() + "/" + "임시 총라운드";
        GameObject.Find("Round").GetComponent<Text>().text = s;
    }
    public void nowGold() {
        string s = "Gold : " + myGold.ToString();
        GameObject.Find("Gold").GetComponent<Text>().text = s;
    }
    public void HP(int hp) {
        GameObject.Find("hpOfCastle").GetComponent<Text>().text = "Castle HP : " + hp.ToString();
    }
    #endregion

    #endregion
}