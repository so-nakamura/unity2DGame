using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("エフェクトが付いた床を判定するか")] public bool checkPlatformGround;

    private string groundTag = "Ground";
    private string platformTag = "GroundPlatform";
    private string moveFloorTag = "MoveFloor";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    /// <summary>
    /// 接地判定を返すメソッド
    /// </summary>
    /// <returns>接地判定</returns>
    public bool IsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //タグ名が"Ground"の場合
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
        //インスペクターでチェックを入れている　かつ　タグ名が"GroundPlatform"の場合
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag))
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //タグ名が"Ground"の場合
        if (collision.tag == groundTag) {
            isGroundStay = true;
        }
        //インスペクターでチェックを入れている　かつ　タグ名が"GroundPlatform"の場合
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag))
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //タグ名が"Ground"の場合
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
        //インスペクターでチェックを入れている　かつ　タグ名が"GroundPlatform"の場合
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag))
        {
            isGroundExit = true;
        }
    }
}
