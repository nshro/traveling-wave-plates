using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWavingPlates : MonoBehaviour
{
    private float PLATE_WIDTH = 0.5f;
    private float PLATE_THICKNESS = 0.1f;
    private int OBJECT_SIZE = 20;       // 平板の数
    public Transform spawnPos;
    public GameObject spawnee;

    private Vector3 addPosition = new Vector3(0, 0, 1.0f);
    private string[] id = {"plate0","plate1","plate2","plate3","plate4","plate5","plate6","plate7","plate8","plate9",
                           "plate10","plate11","plate12","plate13","plate14","plate15","plate16","plate17","plate18","plate19"};

    void CreatePlate(int index) {
        spawnPos.position += addPosition;
        GameObject plate = Instantiate(spawnee, spawnPos.position, spawnPos.rotation);
        plate.name = id[index];
        plate.GetComponent<Rigidbody>().useGravity = false;
    }
    void AddHingeJoint(int index) {
        GameObject plate = GameObject.Find(id[index]);
        GameObject prevPlate = GameObject.Find(id[index-1]);
        HingeJoint plateHinge = plate.AddComponent<HingeJoint>();
        plateHinge.anchor = new Vector3(0f,PLATE_THICKNESS,-PLATE_WIDTH);
        plateHinge.connectedBody = prevPlate.GetComponent<Rigidbody>();
    }

    private int TARGET_VELOCITY = 120;  // モータの速度
    private float TARGET_ANGLE = 45f;   // 回転を折り返す角度
    private float ROTATE_SPAN = 3;    // 収縮運動を実行する期間
    private int SPAN_SIZE = 8;          // 1周期の平板の個数
    private float SizeTimer;
    bool[] reverse = {false, false, false, false, false, false, false, false, false, false,
                      false, false, false, false, false, false, false, false, false, false};

    void ActivateMotor(int index) {
        GameObject plate = GameObject.Find(id[index]);
        HingeJoint plateHinge = plate.GetComponent<HingeJoint>();
        JointMotor motor = plateHinge.motor;
        float alpha = ROTATE_SPAN / SPAN_SIZE * index; // 位相ズレ
        if (Input.GetMouseButtonDown(0)) {
            motor.force = 99999999;
            plateHinge.useMotor = true;
        }
        // 正回転（SizeTimerが実行期間の半分より小さいとき）
        if (reverse[index] == false && SizeTimer > alpha) {
            motor.targetVelocity = TARGET_VELOCITY;
            if (plateHinge.angle > TARGET_ANGLE) {
                reverse[index] = true;
            }
        }
        // 逆回転（SizeTimerが実行期間の半分以上、かつ、実行期間より小さいとき）
        if (reverse[index] == true && SizeTimer > alpha) {   
            motor.targetVelocity = -TARGET_VELOCITY;
            if (plateHinge.angle < -TARGET_ANGLE) {
                reverse[index] = false;
            }
        }
        plateHinge.motor = motor;
    }

    void Start()
    {
        CreatePlate(0);
        for (int i = 1; i < OBJECT_SIZE; i++) {
            CreatePlate(i);
            AddHingeJoint(i);
        }
        SizeTimer = 0;
    }

    void Update()
    {
        for (int i = 1; i < OBJECT_SIZE; i++) {
            ActivateMotor(i);
        }
        SizeTimer += Time.deltaTime; // SizeTimerを時間の経過分増加させる
    }
}
