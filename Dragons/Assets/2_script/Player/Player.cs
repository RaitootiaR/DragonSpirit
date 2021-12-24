using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
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
    private GameObject DragonSingleMouse;

    ///<summary>
    ///ドラゴン2ショット
    /// </summary>
    [SerializeField, Tooltip("ドラゴンの右口を割り当てる")]
    private GameObject DragonTwinMouseR;
    [SerializeField, Tooltip("ドラゴンの左口を割り当てる")]
    private GameObject DragonTwinMouseL;


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




    //子のRendererの配列。
    Renderer[] childrenRenderer;

    //今childrenRendererが有効か無効かのフラグ。
    bool isEnabledRenderers;

    //ダメージを受けているかのフラグ。
    bool isDamaged;

    //リセットする時の為にコルーチンを保持しておく。
    Coroutine flicker;


    //ダメージ点滅の長さ。無敵時間と共通。

    //任意の値。
    float flickerDuration = 1.5f;

    //ダメージ点滅の合計経過時間。    
    float flickerTotalElapsedTime;
    //ダメージ点滅のRendererの有効・無効切り替え用の経過時間。
    float flickerElapsedTime;

    //ダメージ点滅のRendererの有効・無効切り替え用のインターバル。
    //任意の値。
    float flickerInterval = 0.075f;

    Animator Drani;



    //ドラゴンの状態を表す
    public enum Dragstate
    {
        Minimum,
        Middle,
        Biggest
    }

    //炎の量
    private enum Fireamount
    {
        Single,
        Twin,
        Triple
    }
    private enum FireSpeed
    {
        slow,
        normal,
        fast
    } 

    Dragstate nowstate = Dragstate.Minimum;
    Fireamount nowamount = Fireamount.Single;
    FireSpeed nowfirespeed = FireSpeed.slow;
   

    public override void OnStart()
    {
        base.OnStart();
        Trs = gameObject.transform;
        Drani = minDrag.GetComponent<Animator>();
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
            pos.x += Input.GetAxisRaw(Data.Horizontal) * Data.Data.PlayerSpeed;
            pos.z += Input.GetAxisRaw(Data.Vertical) * Data.Data.PlayerSpeed;
            return pos;
        }
        //スティックの入力に応じて、傾きの値を返す。
        Vector3 RotParameter(Vector3 rot)
        {
            rot.x = Input.GetAxisRaw(Data.Vertical) * Data.Data.PlayerInclination;
            rot.z = -Input.GetAxisRaw(Data.Horizontal) * Data.Data.PlayerInclination;
            return rot;
        }
    }

    float time = 0;
    protected override void Shot()
    {
        if (Input.GetKey(Data.ShotKey) && BulletCorrectlyInterval() < time)
        {
            Quaternion rot = Input.GetKey(Data.GroundKey) ? Quaternion.Euler(Data.Data.PlayerGroundBulletRot) : Quaternion.identity;
            switch (nowamount) {
                
                case Fireamount.Single:
                    pool.Pop(DragonSingleMouse.transform.position, rot);
                    time = 0;
                    break;

                case Fireamount.Twin:
                    pool.Pop(DragonTwinMouseR.transform.position, rot);
                    pool.Pop(DragonTwinMouseL.transform.position, rot);
                    time = 0;
                    break;

                case Fireamount.Triple:
                    pool.Pop(DragonSingleMouse.transform.position, rot);
                    rot = Input.GetKey(Data.GroundKey) ? Quaternion.Euler(Data.Data.PlayerGroundBulletRot) : Quaternion.Euler(Data.Data.PlayerRightBulletRot);
                    pool.Pop(DragonTwinMouseR.transform.position, rot);
                    rot = Input.GetKey(Data.GroundKey) ? Quaternion.Euler(Data.Data.PlayerGroundBulletRot) : Quaternion.Euler(Data.Data.PlayerLeftBulletRot);
                    pool.Pop(DragonTwinMouseL.transform.position, rot);
                    time = 0;
                    break;
            }
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
                Drani = midDrag.GetComponent<Animator>();
                Data.Data.StatusUpdateMidSpeed();
                nowstate = Dragstate.Middle;
                
                
                
            break;


            case Dragstate.Middle:
                midDrag.SetActive(false);
                bigDrag.SetActive(true);
                Drani = bigDrag.GetComponent<Animator>();
                Data.Data.StatusUpdateBigSpeed();
                nowstate = Dragstate.Biggest;

            break;


            case Dragstate.Biggest:
                bigDrag.SetActive(false);
                minDrag.SetActive(true);
                Drani = bigDrag.GetComponent<Animator>();
                Data.Data.SpeedReset();
                nowstate = Dragstate.Minimum;

            break;


        }
    }

    public void Fire_powerUp()
    {
        switch (nowamount)
        {
            case Fireamount.Single:
                nowamount = Fireamount.Twin;
                Debug.Log(nowamount);
                break;

            case Fireamount.Twin:
                nowamount = Fireamount.Triple;
                Debug.Log(nowamount);
                break;

            case Fireamount.Triple:
                nowamount = Fireamount.Single;
                Debug.Log(nowamount);
                break;
        }
    }

    public void Fire_speedUp()
    {
        switch (nowfirespeed)
        {
            case FireSpeed.slow:
                nowfirespeed = FireSpeed.normal;
                Data.Data.StatusUpdateNormalFireSpeed();
                break;

            case FireSpeed.normal:
                nowfirespeed = FireSpeed.fast;
                Data.Data.StatusUpdateFastFireSpeed();
                break;

            case FireSpeed.fast:
                nowfirespeed = FireSpeed.slow;
                Data.Data.StatusResetFireSpeed();
                break;
        }
    }

    public void Damaged()
    {
        //ダメージ点滅中は二重に実行しない。
        if (isDamaged)
            return;

        Data.Data.Damage();
        

        /*
        //そのダメージによって死んだ場合は、ダメージ点滅させない。
                if (hp <= 0) {
                    Died();
                    return;
                }
        */

        StartFlicker();
    }



    void SetEnabledRenderers(bool b)
    {
        if (childrenRenderer != null)
        {
            for (int i = 0; i < childrenRenderer.Length; i++)
            {

                childrenRenderer[i] = null;
            }
        }
        switch (nowstate)
        {
            case Dragstate.Minimum:
                childrenRenderer = minDrag.GetComponentsInChildren<Renderer>();
                break;


            case Dragstate.Middle:
                childrenRenderer = midDrag.GetComponentsInChildren<Renderer>();
                break;


            case Dragstate.Biggest:
                childrenRenderer = bigDrag.GetComponentsInChildren<Renderer>();
                break;
        }
        //多分forの方が軽いと思ったからこう書いた。foreachでも良いです。
        for (int i = 0; i < childrenRenderer.Length; i++)
        {
           
            childrenRenderer[i].enabled = b;
        }
    }


    void StartFlicker()
    {
        //isDamagedで多重実行を防いでいるので、ここで多重実行を弾かなくてもOK。        
        flicker = StartCoroutine(Flicker());
    }


    IEnumerator Flicker()
    {

        isDamaged = true;

        flickerTotalElapsedTime = 0;
        flickerElapsedTime = 0;
        Drani.SetTrigger("Damagetrigger");
        while (true)
        {

            flickerTotalElapsedTime += Time.deltaTime;
            flickerElapsedTime += Time.deltaTime;



            if (flickerInterval <= flickerElapsedTime)
            {
                //ここが被ダメージ点滅の処理。


                flickerElapsedTime = 0;
                //Rendererの有効、無効の反転。
                isEnabledRenderers = !isEnabledRenderers;
                SetEnabledRenderers(isEnabledRenderers);

            }


            if (flickerDuration <= flickerTotalElapsedTime)
            {
                //ここが被ダメージ点滅の終了時の処理。

                isDamaged = false;

                //最後には必ずRendererを有効にする(消えっぱなしになるのを防ぐ)。
                isEnabledRenderers = true;
                SetEnabledRenderers(true);

                flicker = null;
                yield break;
            }

            yield return null;
        }

    }

    //コルーチンのリセット用。
    void ResetFlicker()
    {
        if (flicker != null)
        {
            StopCoroutine(flicker);
            flicker = null;
        }
    }


    /*
    //リセットの雰囲気。ゲームオーバー時等に実行。
        void Reset()
        {
            ResetFlicker();

            isDamaged = false;
            isEnabledRenderers = true;
            SetEnabledRenderers(true);



        }
    */

}

