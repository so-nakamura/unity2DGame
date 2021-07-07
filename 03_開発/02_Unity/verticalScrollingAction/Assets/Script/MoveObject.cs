using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("�ړ��o�H")] public GameObject[] movePoint;
    [Header("����")] public float speed = 1.0f;

    private Rigidbody2D rb = null;
    private int nowPoint = 0;
    private bool returnPoint = false;
    private Vector2 oldPos = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(movePoint != null && movePoint.Length > 0 && rb != null)
        {
            rb.position = movePoint[0].transform.position;
            oldPos = rb.position; //�����ʒu���L�^
        }
    }

    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movePoint != null && movePoint.Length > 1 && rb != null)
        {
            //�ʏ�i�s
            if (!returnPoint)
            {
                int nextPoint = nowPoint + 1;

                //�ڕW�̃|�C���g�Ƃ̌덷���킸���ɂȂ�܂ňړ�
                if(Vector2.Distance(transform.position,movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //���ݒn���玟�̃|�C���g�ւ̃x�N�g�����쐬
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //���̃|�C���g�ֈړ�
                    rb.MovePosition(toVector);
                }
                //���̃|�C���g���P�i�߂�
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    ++nowPoint;

                    //���ݒn���z��̍Ōゾ�����ꍇ
                    if(nowPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }
                }
            }
            else
            {
                int nextPoint = nowPoint - 1;

                //�ڕW�̃|�C���g�Ƃ̌덷���킸���ɂȂ�܂ňړ�
                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //���ݒn���玟�̃|�C���g�ւ̃x�N�g�����쐬
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //���̃|�C���g�ֈړ�
                    rb.MovePosition(toVector);
                }
                //���̃|�C���g���P�i�߂�
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    --nowPoint;

                    //���ݒn���z��̍ŏ��������ꍇ
                    if (nowPoint <= 0)
                    {
                        returnPoint = false;
                    }
                }
            }
        }

        //�����@���i���ݒn - �O�̈ʒu�j�@/�@���ԁ@
        myVelocity = (rb.position - oldPos) / Time.deltaTime;
        oldPos = rb.position; //�O�̈ʒu���L�^
    }
}
