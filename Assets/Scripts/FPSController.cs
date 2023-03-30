using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //移動用
    float x, z;
    //スピード調整用
    float speed = 0.1f;

    public GameObject cam;
    Quaternion cameraRot, characterRot;

    float Xsensitybity = 3f, Ysensityvity = 3f;

    bool cursorLock = true;


    //角度制限用変数
    float minX = -90f, maxX = 90f;

    
    //Updateの中で作成した関数を呼ぶ

    //アップデートでマウスの入力を受け取り、その動きをカメラに反映
    //カメラの正面方向に進むようにコード記述

    
    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()　
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensitybity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);

        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        //カーソルロックの関数はUPdateで呼び出す
        UpdateCursorLock();
    }

    private void FixedUpdate()
    {
        x = 0;
        z = 0;

        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;

        //transform.position += new Vector3(x, 0, z);

        transform.position += cam.transform.forward * z + cam.transform.right * x;
    }

    //マウスカーソルの表示を切り替える関数
    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //角度制限用関数
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x, y, z, w(x,y,zはベクトル(量と向き) :wはスカラー（座標とは無関係の量：回転する））

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w /= 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;

    }
}
