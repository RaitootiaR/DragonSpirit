using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
 
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



//初期化時に子のRendererを全て取得しておく。Startか、Awakeに追記すれば良い。
//Renderer系全てに対応。
	void start()
	{
		childrenRenderer = GetComponentsInChildren<Renderer>();
		
	}


	public void Damaged()
	{
//ダメージ点滅中は二重に実行しない。
		if (isDamaged)
			return;

/*
//ここでHPを減らしたりとかする。
		hp -= damage;
		if (hp < 0)
			hp = 0;
*/

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

		while (true) {

			flickerTotalElapsedTime += Time.deltaTime;
			flickerElapsedTime += Time.deltaTime;



			if (flickerInterval <= flickerElapsedTime) {
//ここが被ダメージ点滅の処理。


				flickerElapsedTime = 0;
//Rendererの有効、無効の反転。
				isEnabledRenderers = !isEnabledRenderers;
				SetEnabledRenderers(isEnabledRenderers);

			}


			if (flickerDuration <= flickerTotalElapsedTime) {
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
		if (flicker != null) {
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
