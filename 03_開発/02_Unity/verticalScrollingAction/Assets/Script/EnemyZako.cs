using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZako : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false; //左右のフラグ
    private bool isDead = false; //死亡フラグ
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dコンポーネントのインスタンスを取得
        rb = GetComponent<Rigidbody2D>();
        //SpriteRendererコンポーネントのインスタンスを取得
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //プレイヤーに踏まれていない場合
        if (!oc.playerStepOn)
        {
            if (sr.isVisible || nonVisibleAct)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1; //左向き
                if (rightTleftF)
                {
                    xVector = 1; //右向き
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            //映っていない場合
            else
            {
                //物理演算を切る
                rb.Sleep();
            }
        }
        //プレイヤーに踏まれた場合
        else
        {
            if (!isDead)
            {
                anim.Play("enemy_Dead");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                //BoxCollider2Dを無効にする
                col.enabled = false;
                //３秒後にゲームオブジェクトを削除
                Destroy(gameObject, 3f);
            }
            else
            {
                //ザコ敵が回転する
                transform.Rotate(new Vector3(0, 0, 5));
            }

        }
    }
}
