using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWavingPlates : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject spawnee;
    private Vector3 addPosition = new Vector3(0, 0, 1);
    private string[] id = {"plate0","plate1","plate2","plate3","plate4","plate5","plate6","plate7","plate8","plate9"};

    void CreatePlate(int index) {
        spawnPos.position += addPosition;
        GameObject plate = Instantiate(spawnee, spawnPos.position, spawnPos.rotation);
        plate.name = id[index];
    }
    void AddHingeJoint(int index) {
        GameObject plate = GameObject.Find(id[index]);
        GameObject prevPlate = GameObject.Find(id[index-1]);
        HingeJoint plateHinge = plate.AddComponent<HingeJoint>();
        plateHinge.anchor = new Vector3(0f,0.5f,-0.5f);
        plateHinge.connectedBody = prevPlate.GetComponent<Rigidbody>();
    }
    private float SizeTimer; // 大きくするか小さくするか判断する変数
    private float RotateSpan = 1/2; // 収縮運動を実行する期間
    private float delta = 0;

    void ActivateMotor(int index) {
        if (Input.GetMouseButtonDown(0)) {
            GameObject plate = GameObject.Find(id[index]);
            HingeJoint plateHinge = plate.GetComponent<HingeJoint>();
            JointMotor motor = plateHinge.motor;
            motor.force = 99999999;

            SizeTimer += Time.deltaTime; // SizeTimerを時間の経過分増加させる

            // 順回転（SizeTimerが実行期間の半分より小さいとき）
            if (SizeTimer < RotateSpan / 2 - delta) 
            {            
                // Lerp(最初の値,変更後の値,変更するスピード)で最初の値から変更後の値変更する
                motor.targetVelocity = 100;
                // Debug.Log("順転");
            }
    
            // 逆回転（SizeTimerが実行期間の半分以上、かつ、実行期間より小さいとき）
            if (SizeTimer >= RotateSpan / 2 - delta && SizeTimer < RotateSpan)
            {   
                motor.targetVelocity = -100;
                // Debug.Log("順転");
            }
    
            // SizeTimerを0に（SizeTimerが実行期間以上になったとき）
            if (SizeTimer >= RotateSpan)
            {
                SizeTimer = 0;
            }
            plateHinge.motor = motor;
            plateHinge.useMotor = true;
        }
    }

    void Start()
    {
        CreatePlate(0);
        for (int i = 1; i < 10; i++) {
            CreatePlate(i);
            AddHingeJoint(i);
        }
    }
    void Update()
    {
        for (int i = 1; i < 10; i++) {
            ActivateMotor(i);
        }
    }
}
