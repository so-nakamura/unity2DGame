using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    [Header("�v���C���[�Q�[���I�u�W�F�N�g")] public GameObject playerObj;
    [Header("�R���e�B�j���[�ʒu")] public GameObject [] continuePoint;

    private Player p;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            playerObj.transform.position = continuePoint[0].transform.position;

            //�v���C���[�̃X�N���v�g�擾
            p = playerObj.GetComponent<Player>();
            if (p == null)
            {
                Debug.Log("�v���C���[����Ȃ����̂��A�^�b�`����Ă����I");
            }
        }
        else
        {
            Debug.Log("�ݒ肪����ĂȂ���");
        }; 
        
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[���擾�ł��Ă���@�����@�R���e�B�j���[�ҋ@���
        if (p != null && p.IsContinueWaiting())
        {
            //�R���e�B�j���[�������ʒu�̖ڈ�̐ݒ肪����Ă��邩
            if (continuePoint.Length > GManager.instance.continueNum)
            {
                //�@�v���C���[�̈ʒu�@���@�ڈ�̈ʒu�i�ֈړ�������j
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
            }
            else
            {
                Debug.Log("�R���e�B�j���[�|�C���g�̐ݒ肪����ĂȂ���");
            }
        }
    }
}
