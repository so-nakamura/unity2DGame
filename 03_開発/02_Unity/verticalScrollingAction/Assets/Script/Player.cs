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
    [Header("重力")] public float gravity;
    [Header("接地判定")] public GroundCheck ground;
    [Header("頭をぶつけた判定")] public GroundCheck head;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    #endregion

    #region//プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isRun = false;
    private float jumpPos = 0.0f;
    private float jumpTime = 0.0f;
    private float dashTime = 0.0f;
    private float beforeKey = 0.0f;
    #endregion

    void Start()
    {
        //Animatorコンポーネントのインスタンスを取得
        anim = GetComponent<Animator>();
        //Rigidbody2Dコンポーネントのインスタンスを取得
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
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

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        //地面についている場合
        if (isGround)
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
        //地面から離れている場合
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
        if (isJump)
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
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }
}
