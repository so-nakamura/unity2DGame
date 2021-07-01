using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ肷��
    [Header("�ړ����x")] public float speed;
    [Header("�W�����v���x")] public float jumpSpeed;
    [Header("�W�����v���鍂��")] public float jumpHeight;
    [Header("�W�����v��������")] public float jumpLimitTime;
    [Header("�d��")] public float gravity;
    [Header("�ڒn����")] public GroundCheck ground;
    [Header("�����Ԃ�������")] public GroundCheck head;
    [Header("�_�b�V���̑����\��")] public AnimationCurve dashCurve;
    [Header("�W�����v�̑����\��")] public AnimationCurve jumpCurve;
    #endregion

    #region//�v���C�x�[�g�ϐ�
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
        //Animator�R���|�[�l���g�̃C���X�^���X���擾
        anim = GetComponent<Animator>();
        //Rigidbody2D�R���|�[�l���g�̃C���X�^���X���擾
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        //�ڒn������擾
        isGround = ground.IsGround();
        isHead = head.IsGround();

        //�e����W���̑��x�����߂�
        float ySpeed = GetYSpeed();
        float xSpeed = GetXSpeed();

        //�A�j���[�V������K�p
        SetAnimation();
        
        //�ړ����x��ݒ�
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    /// <summary>
    /// Y�����ŕK�v�Ȍv�Z�����A���x��Ԃ�
    /// </summary>
    /// <returns>Y���̑���</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        //�n�ʂɂ��Ă���ꍇ
        if (isGround)
        {
            //������L�[��������Ă���ꍇ
            if (verticalKey > 0)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //�W�����v�����ʒu���L�^����
                isJump = true;
                jumpTime = 0.0f; //�W�����v���Ԃ����Z�b�g
            }
            //���͂��Ȃ��ꍇ
            else
            {
                isJump = false;
            }
        }
        //�n�ʂ��痣��Ă���ꍇ
        else if (isJump)
        {
            //������L�[�������Ă��邩
            bool pushUpKey = verticalKey > 0;
            //���݂̍�������ׂ鍂����艺��
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //�W�����v���鎞�Ԃ������Ȃ肷���ĂȂ���
            bool canTime = jumpLimitTime > jumpTime;

            //������L�[�������Ă���@���@��ׂ鍂����艺�ɂ���@���@�W�����v���Ԃ��I�[�o�[���ĂȂ��@���@�����Ԃ��Ă��Ȃ�
            if (pushUpKey && canHeight && canTime && !isHead)
            {
                //�㏸����
                ySpeed = jumpSpeed;
                //�㏸���Ă���Ԃɐi�񂾃Q�[�������Ԃ𖈉񑫂�
                jumpTime += Time.deltaTime;
            }
            //���͂��Ȃ��ꍇ
            else
            {
                //���~����
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        //�A�j���[�V�����J�[�u�𑬓x�ɓK�p
        if (isJump)
        {
            ySpeed *= jumpCurve.Evaluate(dashTime);
        }

        return ySpeed;
    }

    /// <summary>
    /// X�����ŕK�v�Ȍv�Z�����A���x��Ԃ�
    /// </summary>
    /// <returns>X���̑���</returns>
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal");

        float xSpeed = 0.0f;

        //�E�����L�[�����͂��ꂽ�ꍇ
        if (horizontalKey > 0)
        {
            //�E�����ɑ���
            transform.localScale = new Vector3(1, 1, 1);
            isRun = true;
            //�_�b�V�������Ԃ��v��
            dashTime += Time.deltaTime;
            //�E���������琳�̑��x
            xSpeed = speed;
        }
        //�������L�[�����͂��ꂽ�ꍇ
        else if (horizontalKey < 0)
        {
            //�������ɑ���
            transform.localScale = new Vector3(-1, 1, 1);
            isRun = true;
            //�_�b�V�������Ԃ��v��
            dashTime += Time.deltaTime;
            //�����������畉�̑��x
            xSpeed = -speed;
        }
        //���͂��Ȃ��ꍇ
        else
        {
            isRun = false;
            dashTime = 0.0f;
            xSpeed = 0.0f;
        }

        //�O��̓��͂���_�b�V���̔��]�𔻒f���đ��x��ς���
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;

        //�A�j���[�V�����J�[�u�𑬓x�ɓK�p
        xSpeed *= dashCurve.Evaluate(dashTime);

        return xSpeed;
    }

    /// <summary>
    /// �A�j���[�V������ݒ肷��
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }
}
