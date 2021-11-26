#if UNITY_EDITOR
using UnityEngine;

public class ITOSO : MonoBehaviour
{
    [SerializeField]
    int count;
    [SerializeField]
    Transform light;
    [SerializeField]
    float time;

    float _t = 0;
    int con;

    private void Update()
    {
        if(con>count)
        {
            if (time % _t==0) return;

            Vector3 rot= light.eulerAngles;
            _t += Time.deltaTime;
            rot.y = Mathf.Lerp(-30,50,_t/time);
            light.eulerAngles = rot;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        con++;
        if(con>count)
        {
            Physics.autoSimulation = false;
            Physics2D.autoSimulation = false;
        }
    }
}
#endif