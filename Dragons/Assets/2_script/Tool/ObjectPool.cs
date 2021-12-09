using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class ObjectPool
{
    //生成するオブジェクト
    private GameObject _originalObj=default;
    //プール用スタック
    private List<GameObject> PoolList=default;

    //コンストラクタ
    public ObjectPool(GameObject original,int size)
    {
        _originalObj = original;
        PoolList = new List<GameObject>(size);
        GameObject obj = Object.Instantiate(original);
        obj.SetActive(false);
        PoolList.Add(obj);
    }

    //生成する。
    public GameObject Pop(Vector3 pos,Quaternion rot)
    {
        GameObject Obj;

        //非表示のオブジェクトがある場合
        if (PoolList.Any(_=>!_.activeSelf))
        {
            Obj = PoolList.First(_ => !_.activeSelf);
            Obj.SetActive(true);
            Obj.GetComponent<Bullet>().BulletSpeed = Data.Data.PlayerBulletSpeed;
            Obj.transform.position = pos;
            Obj.transform.rotation = rot;
            return Obj;
        }

        //新規生成
        Obj = Object.Instantiate(_originalObj,pos,rot);
        Obj.SetActive(true);
        PoolList.Add(Obj);
        return Obj;
    }
}
