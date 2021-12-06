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
    public enum Dragstate
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

            pool.Pop(DragonMouse.transform.position, rot);
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

        
    }


   
    //ドラゴンを成長させるメソッド
    public void Draggrow()
    {
        
        switch (nowstate)
        {
            case Dragstate.Minimum:
                minDrag.SetActive(false);
                midDrag.SetActive(true);
                Data.Data.StatusUpdateMid();
                nowstate = Dragstate.Middle;
                
                
            break;


            case Dragstate.Middle:
                midDrag.SetActive(false);
                bigDrag.SetActive(true);
                Data.Data.StatusUpdateBig();
                nowstate = Dragstate.Biggest;

            break;


            case Dragstate.Biggest:
                bigDrag.SetActive(false);
                minDrag.SetActive(true);
                Data.Data.StatusReset();
                nowstate = Dragstate.Minimum;

            break;


        }
    }

    
}

