using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("�G�t�F�N�g���t�������𔻒肷�邩")] public bool checkPlatformGround;

    private string groundTag = "Ground";
    private string platformTag = "GroundPlatform";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    /// <summary>
    /// �ڒn�����Ԃ����\�b�h
    /// </summary>
    /// <returns>�ڒn����</returns>
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
        //�^�O����"Ground"�̏ꍇ
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
        //�C���X�y�N�^�[�Ń`�F�b�N�����Ă���@���@�^�O����"GroundPlatform"�̏ꍇ
        else if (checkPlatformGround && collision.tag == platformTag)
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //�^�O����"Ground"�̏ꍇ
        if (collision.tag == groundTag) {
            isGroundStay = true;
        }
        //�C���X�y�N�^�[�Ń`�F�b�N�����Ă���@���@�^�O����"GroundPlatform"�̏ꍇ
        else if (checkPlatformGround && collision.tag == platformTag)
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�^�O����"Ground"�̏ꍇ
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
        //�C���X�y�N�^�[�Ń`�F�b�N�����Ă���@���@�^�O����"GroundPlatform"�̏ꍇ
        else if (checkPlatformGround && collision.tag == platformTag)
        {
            isGroundExit = true;
        }
    }
}
