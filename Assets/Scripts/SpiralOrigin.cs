using UnityEngine;

public class SpiralOrigin : MonoBehaviour {

    [SerializeField]
    private GameObject createObject; // 生成するオブジェクト

    [SerializeField]
    private GameObject plateObject; // 生成するオブジェクト

    [SerializeField]
    private int itemCount = 100; // 生成するオブジェクトの数

    [SerializeField]
    private float radius = 1f; // 半径

    [SerializeField]
    private float repeat = 1f; // 何周期するか

    [SerializeField]
    private float length = 1f; // Z軸の長さ


    void Start () {

        var oneCycle = 2.0f * Mathf.PI; // sin の周期は 2π
        var oneLength = length / itemCount; // Z軸の1単位
        var z = transform.position.z - oneLength; // Z軸初期位置 (生成前に足しこみをしているので、一回分引いておく)

        for (var i = 0; i < itemCount; ++i)
        {
            var point = ((float)i / itemCount) * oneCycle; // 周期の位置 (1.0 = 100% の時 2π となる)
            var repeatPoint = point * repeat; // 繰り返し位置

            var x = Mathf.Sin(repeatPoint) * radius;
            // var x = transform.position.x;
            var y = transform.position.y + Mathf.Cos(repeatPoint) * radius + 0.5f;
            z += oneLength;

            var position = new Vector3(x, y, z);

            GameObject obj = Instantiate(
                createObject, 
                position, 
                Quaternion.identity, 
                transform
            );
            obj.name = i.ToString();
            obj.AddComponent<Rigidbody>();
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<Rigidbody>().isKinematic = true;

            // if (i < 2) {
            float offsetY = 0f;
            float rotationX = 0f;
            if (i < itemCount / 2){
                offsetY = 1.2f*(float)i + radius + 0.5f;
                rotationX = -61f;
            } else {
                offsetY = 1.2f*(float)(itemCount - 1) - 1f*(float)i + radius + 0.5f;
                rotationX = 61f;
            }
            var tentaclePos = new Vector3(transform.position.x, transform.position.y+offsetY, z);
            GameObject tentacle = Instantiate(
                plateObject, 
                tentaclePos,  
                Quaternion.identity, 
                transform
            );
            var rotation = new Vector3(rotationX, 0f, 0f);
            tentacle.transform.Rotate(rotation);

            int j = i + 100;
            tentacle.name = j.ToString();
            if (i != 0) {
                GameObject prevTentacle = GameObject.Find((j-1).ToString());
                HingeJoint Hinge1 = tentacle.AddComponent<HingeJoint>();
                Hinge1.connectedBody = prevTentacle.GetComponent<Rigidbody>();
                Hinge1.anchor = new Vector3(2f, 0f, -0.75f);
                HingeJoint Hinge2 = tentacle.AddComponent<HingeJoint>();
                Hinge2.connectedBody = prevTentacle.GetComponent<Rigidbody>();
                Hinge2.anchor = new Vector3(-2f, 0f, -0.75f);
            }

            if (i == 0 || i == itemCount - 1) {
                HingeJoint Hinge = obj.AddComponent<HingeJoint>();
                Hinge.connectedBody = tentacle.GetComponent<Rigidbody>();
                Hinge.anchor = new Vector3(0f, 0f, 0f);
            }
            // }

            // var rotation = new Vector3(0, 0, 0);
            // if (i == 0) {
            //     var delta_y =  Mathf.Cos(((float)(i+1) / itemCount) * oneCycle * repeat) - Mathf.Cos(repeatPoint);
            //     rotation = new Vector3(-Mathf.Atan(delta_y*radius)*Mathf.Rad2Deg, 0f, 0f);
            // } else if (i == itemCount - 1) {
            //     var delta_y = Mathf.Cos(repeatPoint) - Mathf.Cos(((float)(i-1) / itemCount) * oneCycle * repeat);
            //     rotation = new Vector3(-Mathf.Atan(delta_y*radius)*Mathf.Rad2Deg, 0f, 0f);
            // } else {
            //     var delta_y = Mathf.Cos(((float)(i+1) / itemCount) * oneCycle * repeat) - Mathf.Cos(((float)(i-1) / itemCount) * oneCycle * repeat);
            //     rotation = new Vector3(-Mathf.Atan(delta_y*radius)*Mathf.Rad2Deg, 0f, 0f);
            // }
            // tentacle.transform.Rotate(rotation);
        }

    }

    private bool isRotate = false;
    private float omega = 2; //角速度 [radian/s]

    void Update()
    {
        float t = Time.deltaTime;
        if (isRotate == true) {
            for (var i = 0; i < itemCount; ++i) 
            {
                Vector3 tmp = GameObject.Find(i.ToString()).transform.position;
                float x = Mathf.Cos(omega * t) * tmp.x - Mathf.Sin(omega * t) * (tmp.y - radius - 0.5f);
                float y = Mathf.Sin(omega * t) * tmp.x + Mathf.Cos(omega * t) * (tmp.y - radius - 0.5f);
                GameObject.Find(i.ToString()).transform.position = new Vector3(x, y + radius + 0.5f, tmp.z);

                int j = i + 100;
                Vector3 tmp1 = GameObject.Find(j.ToString()).transform.position;
                GameObject.Find(j.ToString()).transform.position = new Vector3(transform.position.x, y + radius + 0.5f, tmp1.z);
            }
            if (Input.GetMouseButtonDown(0)) {
                isRotate = false;
                return;
            }
        }

        if (isRotate == false && Input.GetMouseButtonDown(0)) {
            isRotate = true;
        }
    }

}