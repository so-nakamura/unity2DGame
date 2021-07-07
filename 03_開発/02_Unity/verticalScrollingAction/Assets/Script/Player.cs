using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプ制限時間")] public float jumpLimitTime;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("重力")] public float gravity;
    [Header("接地判定")] public GroundCheck ground;
    [Header("頭をぶつけた判定")] public GroundCheck head;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    #endregion

    #region//プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capcol = null;
    private bool isGround = false; //接地判定（足元）
    private bool isHead = false; //接地判定（頭上）
    private bool isJump = false; //ジャンプ判定
    private bool isRun = false; //走行判定
    private bool isDown = false; //ダウン判定
    private bool isOtherJump = false; 
    private float jumpPos = 0.0f; //ジャンプした位置
    private float otherJumpHeight = 0.0f; //踏んづけたものから跳ねる高さ
    private float dashTime, jumpTime; 
    private float beforeKey; //前回の入力値
    private string enemyTag = "Enemy"; //タグ名
    #endregion

    void Start()
    {
        //Animatorコンポーネントのインスタンスを取得
        anim = GetComponent<Animator>();
        //Rigidbody2Dコンポーネントのインスタンスを取得
        rb = GetComponent<Rigidbody2D>();
        //CapsuleCollider2Dコンポーネントのインスタンスを取得
        capcol = GetComponent<CapsuleCollider2D>();

    }

    void FixedUpdate()
    {
        if (!isDown) {
        //接地判定を取得
        isGround = ground.IsGround();
        isHead = head.IsGround();

        //各種座標軸の速度を求める
        float ySpeed = GetYSpeed();
        float xSpeed = GetXSpeed();

        //アニメーションを適用
        SetAnimation();
        
        //移動速度を設定
        rb.velocity = new Vector2(xSpeed, ySpeed);
        }
        else
        {
            //ダウン中は重力のみ適用
            rb.velocity = new Vector2(0, -gravity);
        }
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        //地面から離れている場合（ジャンプ中）
        if (isOtherJump)
        {
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプする時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;

            //上方向キーを押している　かつ　飛べる高さより下にいる　かつ　ジャンプ時間がオーバーしてない　かつ　頭をぶつけていない
            if (canHeight && canTime && !isHead)
            {
                //上昇する
                ySpeed = jumpSpeed;
                //上昇している間に進んだゲーム内時間を毎回足す
                jumpTime += Time.deltaTime;
            }
            //入力がない場合
            else
            {
                //下降する
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }

        //地面についている場合
        else if (isGround)
        {
            //上方向キーが押されている場合
            if (verticalKey > 0)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f; //ジャンプ時間をリセット
            }
            //入力がない場合
            else
            {
                isJump = false;
            }
        }
        //地面から離れている場合（ジャンプ中）
        else if (isJump)
        {
            //上方向キーを押しているか
            bool pushUpKey = verticalKey > 0;
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプする時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;

            //上方向キーを押している　かつ　飛べる高さより下にいる　かつ　ジャンプ時間がオーバーしてない　かつ　頭をぶつけていない
            if (pushUpKey && canHeight && canTime && !isHead)
            {
                //上昇する
                ySpeed = jumpSpeed;
                //上昇している間に進んだゲーム内時間を毎回足す
                jumpTime += Time.deltaTime;
            }
            //入力がない場合
            else
            {
                //下降する
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        //アニメーションカーブを速度に適用
        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(dashTime);
        }

        return ySpeed;
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal");

        float xSpeed = 0.0f;

        //右方向キーが入力された場合
        if (horizontalKey > 0)
        {
            //右向きに走る
            transform.localScale = new Vector3(1, 1, 1);
            isRun = true;
            //ダッシュ中時間を計測
            dashTime += Time.deltaTime;
            //右を押したら正の速度
            xSpeed = speed;
        }
        //左方向キーが入力された場合
        else if (horizontalKey < 0)
        {
            //左向きに走る
            transform.localScale = new Vector3(-1, 1, 1);
            isRun = true;
            //ダッシュ中時間を計測
            dashTime += Time.deltaTime;
            //左を押したら負の速度
            xSpeed = -speed;
        }
        //入力がない場合
        else
        {
            isRun = false;
            dashTime = 0.0f;
            xSpeed = 0.0f;
        }

        //前回の入力からダッシュの反転を判断して速度を変える
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;

        //アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);

        return xSpeed;
    }

    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }

    #region//接触判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == enemyTag)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {                
                if (p.point.y < judgePos)
                {
                    //もう一度跳ねる
                    ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                    if(o != null)
                    {
                        otherJumpHeight = o.boundHeight; //踏んづけたものから跳ねる高さを取得する
                        o.playerStepOn = true; //踏んづけたものに対して踏んづけた事を通知する
                        jumpPos = transform.position.y; //ジャンプした位置を記録する
                        isOtherJump = true;
                        isJump = false;
                        jumpTime = 0.0f;
                    }
                    else
                    {
                        Debug.Log("ObjectCollisionが付いてないよ！");
                    }
                }
                else
                {
                    //ダウンする
                    anim.Play("player_down");
                    isDown = true;
                    break;
                }
            }            
        }
    }
    #endregion
}
