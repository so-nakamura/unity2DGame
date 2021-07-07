using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject [] continuePoint;

    private Player p;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            playerObj.transform.position = continuePoint[0].transform.position;

            //プレイヤーのスクリプト取得
            p = playerObj.GetComponent<Player>();
            if (p == null)
            {
                Debug.Log("プレイヤーじゃないものがアタッチされているよ！");
            }
        }
        else
        {
            Debug.Log("設定が足りてないよ");
        }; 
        
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーを取得できている　＆＆　コンティニュー待機状態
        if (p != null && p.IsContinueWaiting())
        {
            //コンティニューしたい位置の目印の設定が足りているか
            if (continuePoint.Length > GManager.instance.continueNum)
            {
                //　プレイヤーの位置　＝　目印の位置（へ移動させる）
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りてないよ");
            }
        }
    }
}
