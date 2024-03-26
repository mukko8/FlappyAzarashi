using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    //ゲームステート
    enum State{ //Enum(列挙型)3,4つの時に使う 2つのときはbool
        Ready,
        Play,
        GameOver
    }
    State state;
    int score;
    public AzarashiController azarashi;
    public GameObject blocks;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI stateText;
    // Start is called before the first frame update
    void Start()
    {
        //開始と同時にReadyステートに移行
        Ready();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //ゲームのステートごとにイベントを監視
        switch(state){
            case State.Ready:
                //タッチしたらゲームスタート
                if(Input.GetButtonDown("Fire1")) GameStart();
                break;
            case State.Play:
                //キャラクターが死亡したらゲームオーバー
                if(azarashi.IsDead()) GameOver();
                break;
            case State.GameOver:
                //タッチしたらシーンをリロード
                if(Input.GetButtonDown("Fire1")) Reload();
                break; //最後のbreakは省けない
        }
        
    }
    void Ready(){
        state=State.Ready;
        //各オブジェクトを無効状態にする
        azarashi.SetSteerActive(false);
        blocks.SetActive(false);

        //ラベルを更新
        scoreText.text="Score:"+0;

        stateText.gameObject.SetActive(true);
        stateText.text="Ready";
    }
    void GameStart(){
        state=State.Play;
        //各オブジェクトを有効にする
        azarashi.SetSteerActive(true);
        blocks.SetActive(true);
        //最初の入力だけゲームコントローラーから渡す
        azarashi.Flap();

        //ラベルを更新
        stateText.gameObject.SetActive(false);
        stateText.text="";
        
    }
    void GameOver(){
        state=State.GameOver;
        //シーン中のすべてのScrollObjectコンポーネントを探し出す
        ScrollObject[] scrollObjects=FindObjectsOfType<ScrollObject>();
        //全ScrollObjectのスクロール処理を無効にする
        foreach (ScrollObject so in scrollObjects)so.enabled=false; //拡張for文
        
        //ラベルを更新
        stateText.gameObject.SetActive(true);
        stateText.text="GameOver";
    }
    void Reload(){
        //現在読み込んでいるScene(FlappyAzarashi)を再読み込み
        string currentSceneName=SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void IncreaseScore(){
        score++;
        scoreText.text="Score:"+score;
    }
}
