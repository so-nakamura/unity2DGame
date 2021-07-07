using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZako : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ肷��
    [Header("�ړ����x")] public float speed;
    [Header("�d��")] public float gravity;
    [Header("��ʊO�ł��s������")] public bool nonVisibleAct;
    [Header("�ڐG����")] public EnemyCollisionCheck checkCollision;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false; //���E�̃t���O
    private bool isDead = false; //���S�t���O
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2D�R���|�[�l���g�̃C���X�^���X���擾
        rb = GetComponent<Rigidbody2D>();
        //SpriteRenderer�R���|�[�l���g�̃C���X�^���X���擾
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�v���C���[�ɓ��܂�Ă��Ȃ��ꍇ
        if (!oc.playerStepOn)
        {
            if (sr.isVisible || nonVisibleAct)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1; //������
                if (rightTleftF)
                {
                    xVector = 1; //�E����
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            //�f���Ă��Ȃ��ꍇ
            else
            {
                //�������Z��؂�
                rb.Sleep();
            }
        }
        //�v���C���[�ɓ��܂ꂽ�ꍇ
        else
        {
            if (!isDead)
            {
                anim.Play("enemy_Dead");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                //BoxCollider2D�𖳌��ɂ���
                col.enabled = false;
                //�R�b��ɃQ�[���I�u�W�F�N�g���폜
                Destroy(gameObject, 3f);
            }
            else
            {
                //�U�R�G����]����
                transform.Rotate(new Vector3(0, 0, 5));
            }

        }
    }
}
