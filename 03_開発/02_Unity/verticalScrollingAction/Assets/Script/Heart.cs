using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    private Text heartText = null;
    private int oldHeartNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<Text>();
        if (GManager.instance != null)
        {
            heartText.text = "Heart " + GManager.instance.heartNum;
        }
        else
        {
            Debug.Log("�Q�[���}�l�[�W���[�u���Y��Ă��I");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldHeartNum != GManager.instance.score)
        {
            heartText.text = "Heart " + GManager.instance.heartNum;
            oldHeartNum = GManager.instance.heartNum;
        }
    }
}
