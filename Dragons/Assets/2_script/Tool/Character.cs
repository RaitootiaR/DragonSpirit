using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Character : MonoBehaviour
{
    #region 抽象
    protected abstract int _maxHp { get; set; }//最大HP
    protected abstract Animator _ani { get; set; }//アニメーター
    protected abstract string _damageTag { get; set; }//ダメージを受けるタグ
    protected abstract void Move();
    protected abstract void Shot();
    #endregion

    [SerializeField, Header("弾")] private GameObject Bullet;
    protected ObjectPool pool;

    private int _hp = default;//現在のHP
    private bool _isDamage = false;

    //表示されたタイミングで初期処理を呼ぶ。
    private void OnEnable()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    //ゲーム開始時に呼び出す
    public virtual void OnStart()
    {
        pool = new ObjectPool(Bullet,10);
    }

    //表示時に呼び出す。
    public virtual void OnAwake()
    {
        _hp = _maxHp;
        _isDamage = false;
    }

    //常に呼び出す。
    public virtual void OnUpdate()
    {
        //移動処理
        Move();
        //撃つ処理
        Shot();

        //ダメージ処理
        if (!_isDamage) return;
        _isDamage = false;

        Damage();

        //HPが無くなったら、死亡する。
        if (_hp <= 0)
        {
            Death();
        }
    }
    protected void Damage()
    {
        //Hpを減らす。
        _hp--;
        //死亡アニメーション

        //_ani.SetBool()


    }

    protected void Death()
    {




    }

    private void OnTriggerEnter(Collider col) => _isDamage = col.CompareTag(_damageTag);
    private void OnTriggerExit(Collider col) => _isDamage = !col.CompareTag(_damageTag);
}
