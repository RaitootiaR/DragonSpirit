using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected override int _maxHp { get => 1; set { } }
    protected override Animator _ani { get => default; set { } }
    protected override string _damageTag { get => Data.PlayerTag; set { } }

    private Transform Trs;


    //それぞれの大きさのドラゴンを入れる
    [SerializeField]
    private GameObject minDrag;
    [SerializeField]
    private GameObject midDrag;
    [SerializeField]
    private GameObject bigDrag;




    ///<summary>
    ///オブジェクトの生成
    ///</summary>
    [SerializeField, Tooltip("ドラゴンの口をここに割り当てる")]
    private GameObject DragonMouse;

    /// <summary>
    /// 射出するオブジェクト
    /// </summary>
    [SerializeField, Tooltip("射出するオブジェクトをここに割り当てる")]
    private GameObject ThrowingObject;

    /// <summary>
    /// 標的のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("標的のオブジェクトをここに割り当てる")]
    private GameObject TargetObject;

    /// <summary>
    /// 射出角度
    /// </summary>
    [SerializeField, Range(-90F, 90F), Tooltip("射出する角度")]
    private float ThrowingAngle;


    //ドラゴンの状態を表す
    enum Dragstate
    {
        Minimum,
        Middle,
        Biggest
    }

    Dragstate nowstate = Dragstate.Minimum;

   

    public override void OnStart()
    {
        base.OnStart();
        Trs = gameObject.transform;

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            // 干渉しないようにisTriggerをつける
            collider.isTrigger = true;
        }



    }

    public override void OnUpdate()
    {
        //基盤のUpdateの処理だから消さない
        base.OnUpdate();
        

      
    }

    protected override void Move()
    {
        //移動・回転
        Trs.position = MoveParameter(Trs.position);
        Trs.eulerAngles = RotParameter(Trs.eulerAngles);

        //スティックの入力に応じて、移動先の値を返す。
        Vector3 MoveParameter(Vector3 pos)
        {
            pos.x += Input.GetAxis(Data.Horizontal) * Data.Data.PlayerSpeed;
            pos.z += Input.GetAxis(Data.Vertical) * Data.Data.PlayerSpeed;
            return pos;
        }
        //スティックの入力に応じて、傾きの値を返す。
        Vector3 RotParameter(Vector3 rot)
        {
            rot.x = Input.GetAxis(Data.Vertical) * Data.Data.PlayerInclination;
            rot.z = -Input.GetAxis(Data.Horizontal) * Data.Data.PlayerInclination;
            return rot;
        }
    }

    float time = 0;
    protected override void Shot()
    {
        if (Input.GetKey(Data.ShotKey) && BulletCorrectlyInterval()  < time)
        {
            Quaternion rot = Input.GetKey(Data.GroundKey) ? Quaternion.Euler(Data.Data.PlayerGroundBulletRot) : Quaternion.identity;

            pool.Pop(Trs.position, rot);
            time = 0;
        }
        else if (Input.GetKey(Data.GShotKey) && BulletCorrectlyIntervalGround() < time)
        {
            ThrowingBall();
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }

        
        float BulletCorrectlyInterval()
        {
            float correctly = Input.GetAxis(Data.Vertical) * Data.Data.PlayerBulletCorrectly;//スティックの状態から補正値を求める。
            return Data.Data.PlayerBulletInterval + correctly;//補正値を加えた間隔値を返す
        }

        float BulletCorrectlyIntervalGround()
        {
            float correctly = Input.GetAxis(Data.Vertical) * Data.Data.PlayerBulletCorrectly;//スティックの状態から補正値を求める。
            return Data.Data.PlayerBulletInterval + correctly;//補正値を加えた間隔値を返す
        }
    }


   
    //ドラゴンを成長させるメソッド
    public void Draggrow()
    {
        
        switch (nowstate)
        {
            case Dragstate.Minimum:
                minDrag.SetActive(false);
                midDrag.SetActive(true);
                nowstate = Dragstate.Middle;
                
            break;


            case Dragstate.Middle:
                midDrag.SetActive(false);
                bigDrag.SetActive(true);
                nowstate = Dragstate.Biggest;

            break;


            case Dragstate.Biggest:
                bigDrag.SetActive(false);
                minDrag.SetActive(true);
                nowstate = Dragstate.Minimum;

            break;


        }
    }

    private void ThrowingBall()
    {
        if (ThrowingObject != null && TargetObject != null)
        {
            // Ballオブジェクトの生成
            GameObject ball = Instantiate(ThrowingObject, DragonMouse.transform.position, Quaternion.identity);

            // 標的の座標
            Vector3 targetPosition = TargetObject.transform.position;

            // 射出角度
            float angle = ThrowingAngle;

            // 射出速度を算出
            Vector3 velocity = CalculateVelocity(DragonMouse.transform.position, targetPosition, angle);

            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.AddForce(velocity * rid.mass, ForceMode.VelocityChange);
        }
        else
        {
            throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");
        }
    }

    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}

