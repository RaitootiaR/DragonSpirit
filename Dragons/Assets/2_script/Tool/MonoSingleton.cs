using UnityEngine;
/// <summary>
/// MonoBehaviour継承のシングルトン
/// </summary>
/// <typeparam name="T">型</typeparam>
public abstract class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    public static T Data;//シングルトン

    private void Awake()
    {
        //シングルトンの作成
        if (Data == null)
        {
            Data = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
