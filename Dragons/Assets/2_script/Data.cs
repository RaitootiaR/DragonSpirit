using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoSingleton<Data>
{
    //操作方法
    public const KeyCode UpKey = KeyCode.UpArrow;//上移動キー
    public const KeyCode DownKey = KeyCode.DownArrow;//下移動キー
    public const KeyCode RightKey = KeyCode.RightArrow;//右移動キー
    public const KeyCode LeftKey = KeyCode.LeftArrow;//左移動キー
    public const KeyCode ShotKey = KeyCode.Z;//空中ショットキー
    public const KeyCode GroundKey = KeyCode.LeftShift;//地面攻撃切り替えキー
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";




    //プレイヤーステータス
    [Header("HP"), SerializeField]
    [Header("ーーープレイヤーステータスーーー")]
    private int _playerHp;
    [Header("スピード"), SerializeField] private float _playerSpeed;
    [Header("傾き具合"), SerializeField] private float _playerInclination;
    [Header("弾の速度"), SerializeField] private float _playerBulletSpeed;
    [Header("弾の発射間隔"), SerializeField] private float _playerBulletInterval;
    [Header("移動時の弾の間隔補正値"), SerializeField] private float _playerBulletCorrectly;
    [Header("地面弾の角度"), SerializeField] private Vector3 _playerGroundBulletRot;

    //Minimumドラゴンステータス
    [Header("中ドラゴンHP"), SerializeField]
    [Header("ーーープレイヤーステータスーーー")]
    private int _MinplayerHp;
    [Header("中ドラゴンスピード"), SerializeField] private float _MinplayerSpeed;
    [Header("弾の速度"), SerializeField] private float _MinplayerBulletSpeed;
    [Header("弾の発射間隔"), SerializeField] private float _MinplayerBulletInterval;
    [Header("移動時の弾の間隔補正値"), SerializeField] private float _MinplayerBulletCorrectly;

    //Middleドラゴンステータス
    [Header("中ドラゴンHP"), SerializeField]
    [Header("ーーープレイヤーステータスーーー")]
    private int _MidplayerHp;
    [Header("中ドラゴンスピード"), SerializeField] private float _MidplayerSpeed;
    [Header("弾の速度"), SerializeField] private float _MidplayerBulletSpeed;
    [Header("弾の発射間隔"), SerializeField] private float _MidplayerBulletInterval;
    [Header("移動時の弾の間隔補正値"), SerializeField] private float _MidplayerBulletCorrectly;

    //Biggestドラゴンステータス
    [Header("大ドラゴンHP"), SerializeField]
    [Header("ーーープレイヤーステータスーーー")]
    private int _BidplayerHp;
    [Header("大ドラゴンスピード"), SerializeField] private float _BigplayerSpeed;
    [Header("弾の速度"), SerializeField] private float _BigplayerBulletSpeed;
    [Header("弾の発射間隔"), SerializeField] private float _BigplayerBulletInterval;
    [Header("移動時の弾の間隔補正値"), SerializeField] private float _BigplayerBulletCorrectly;


    [HideInInspector] public int PlayerHP { get { return _playerHp; } }
    [HideInInspector] public float PlayerSpeed { get { return _playerSpeed; } }
    [HideInInspector] public float PlayerInclination { get { return _playerInclination; } }
    [HideInInspector] public float PlayerBulletSpeed { get { return _playerBulletSpeed; } }
    [HideInInspector] public float PlayerBulletInterval { get { return _playerBulletInterval; } }
    [HideInInspector] public float PlayerBulletCorrectly { get { return _playerBulletCorrectly; } }
    [HideInInspector] public Vector3 PlayerGroundBulletRot { get { return _playerGroundBulletRot; } }




    //ゲームオブジェクトタグ
    public const string PlayerTag = "Player";
    public const string GroundTag = "Ground";




    //アニメーションタグ
    public const string AnimationTagUp = "MoveUp";//上移動
    public const string AnimationTagDown = "MoveDown";//下移動
    public const string AnimationTagLeft = "MoveLeft";//左移動
    public const string AnimationTagRight = "MoveRight";//右移動
    public const string AnimationTagDamage = "Damage";//ダメージ
    public const string AnimationTagDeath = "Death";//死亡


    //パラメータ調整

    public void StatusReset()
    {
        _playerSpeed = _MinplayerSpeed;
        _playerBulletSpeed = _MinplayerBulletSpeed;
        _playerBulletInterval = _MinplayerBulletInterval;
        _playerBulletCorrectly = _MinplayerBulletCorrectly;
    }
    
    public void StatusUpdateMid()
    {
        _playerSpeed = _MidplayerSpeed;
        _playerBulletSpeed = _MidplayerBulletSpeed;
        _playerBulletInterval = _MidplayerBulletInterval;
        _playerBulletCorrectly = _MidplayerBulletCorrectly;
    }

    public void StatusUpdateBig()
    {
        _playerSpeed = _BigplayerSpeed;
        _playerBulletSpeed = _BigplayerBulletSpeed;
        _playerBulletInterval = _BigplayerBulletInterval;
        _playerBulletCorrectly = _BigplayerBulletCorrectly;
    }


}
