using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class CameraController : MonoBehaviour
{
    [SerializeField]
    List<ListData> data = new List<ListData>();
    Color PointColor = Color.yellow;//ポイントの色
    Color PathColor = Color.yellow;//ルートの色
    Color HandleColor = Color.green;//ハンドルの色
    [SerializeField]
    private float slide = 0;

    [SerializeField]
    GameObject camera;

    [SerializeField]
    bool loop;
    [SerializeField]
    bool move=false;

    private void Start()
    {
        StartCoroutine(CameraMove());
    }

    //３次ベジェ曲線に沿ってカメラを動かす。
    IEnumerator CameraMove()
    {
        if (move) yield break;

        Transform trs = camera.transform;

        for(int i =0;i<data.Count;i++)
        {
            float time = 0;

            if (i + 1 == data.Count) yield break;

            while (time <data[i].time)
            {
                //３次ベジェ
                Vector3 ab = Vector3.Lerp(data[i].pos, data[i].pos+data[i].RightHandle, time/ data[i].time);
                Vector3 bc = Vector3.Lerp(data[i].pos+data[i].RightHandle, data[i+1].pos + data[i+1].LeftHandle, time/ data[i].time);
                Vector3 cd = Vector3.Lerp(data[i + 1].pos + data[i + 1].LeftHandle, data[i + 1].pos, time/ data[i].time);

                //二次ベジェ
                Vector3 abc = Vector3.Lerp(ab, bc, time/ data[i].time);
                Vector3 bcd = Vector3.Lerp(bc, cd, time/ data[i].time);

                //移動位置
                Vector3 p = Vector3.Lerp(abc, bcd, time/ data[i].time);

                trs.position = p;
                time += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        yield return null;
    }

    ////////////////////////////////////////////////////
    //List部分の描画処理
    ////////////////////////////////////////////////////

    [Serializable]
    public class ListData
    {
        public Vector3 pos;
        public Vector3 LeftHandle = Vector3.zero;
        public Vector3 RightHandle = Vector3.zero;
        public Quaternion rot;
        public Color color;
        public float time;
    }
#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        foreach (ListData d in data)
        {
            //if(d.giz==null)
            //{
            //    d.giz = new Gizmos();
            //}

            Gizmos.color = d.color;
            Gizmos.DrawCube(d.pos, Vector3.one);
            //Gizmos.matrix = Matrix4x4.TRS(d.pos, transform.rotation, Vector3.one);
            //Gizmos.matrix = Matrix4x4.identity;
        }
    }

    //////////////////////////
    //インスペクターの拡張
    //////////////////////////

    ////////////////////////////////////////////////////
    //描画部分
    ////////////////////////////////////////////////////
    [CustomEditor(typeof(CameraController))]
    public class TestEditor : Editor
    {
        private bool colorTab = false;

        //区切り線
        private float splitterHeight = 1f;
        private float splitterWidth = 500;
        private float splitterPositionX = 25;
        private float splitterSpace = 15;
        private Color splitterColor = Color.Lerp(Color.black, Color.gray, 0.7f);

        //ReorderableList
        private ReorderableList _reorderableList;

        private Vector3 minPos = new Vector3(2, 0, 2);

        private void OnEnable()
        {
            //インスタンスの生成
            CameraController tes = target as CameraController;
            ////プロパティ名？
            var prop = serializedObject.FindProperty("data");
            //コンストラクタだから、設定した情報拾ってる？
            _reorderableList = new ReorderableList(serializedObject, prop);
            //そのまま要素の縦のサイズかな？
            _reorderableList.elementHeight = 90;

            _reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                //インデックスの要素を拾ってる？
                var element = prop.GetArrayElementAtIndex(index);
                //ずらし？
                rect.height -= 4;
                rect.y += 2;
                //要素描画っぽい？
                EditorGUI.PropertyField(rect, element);
            };

            _reorderableList.onAddCallback += Add;

            _reorderableList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, prop.displayName);
            };

            //要素を追加する
            void Add(ReorderableList list)
            {
                ListData listdata = new ListData();
                listdata.color = tes.PointColor;
                tes.data.Add(listdata);
            }

        }

        public override void OnInspectorGUI()
        {
            //インスタンスの生成
            CameraController tes = target as CameraController;
            //区切り線
            EditorGUILayout.Space(splitterSpace);


            tes.camera = EditorGUILayout.ObjectField("移動するカメラオブジェクト：", tes.camera, typeof(GameObject), true) as GameObject;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("カメラのルートがループするかどうか：");
            tes.loop = EditorGUILayout.Toggle(tes.loop);
            EditorGUILayout.EndHorizontal();
            //区切り線
            EditorGUILayout.Space(30);

            //再生ボタン
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("再生"))
            {
                
            }
            if (GUILayout.Button("一時停止"))
            {

            }
            if (GUILayout.Button("停止"))
            {

            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            //現在の再生地点
            tes.slide = EditorGUILayout.Slider(tes.slide, 0, 1);

            //区切り線
            EditorGUILayout.Space(20);

            //色タブ
            //if (colorTab = EditorGUILayout.Foldout(colorTab, "色情報"))
            //{

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ポイントの色情報");
            tes.PointColor = EditorGUILayout.ColorField(tes.PointColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ルートの色情報");
            tes.PathColor = EditorGUILayout.ColorField(tes.PathColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ハンドルの色情報");
            tes.HandleColor = EditorGUILayout.ColorField(tes.HandleColor);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("初期色に戻す。"))
            {
                tes.PointColor = Color.yellow;//ポイントの色
                tes.PathColor = Color.yellow;//ルートの色
                tes.HandleColor = Color.green;//ハンドルの色
            }
            //}
            //区切り線
            EditorGUILayout.Space(splitterSpace);
            DrowSplitter(splitterHeight, splitterWidth, splitterPositionX, splitterColor);
            EditorGUILayout.Space(splitterSpace);

            serializedObject.Update();
            _reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();



            void DrowSplitter(float height, float width, float positionX, Color color)
            {
                //区切り線の設定
                Rect splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(height));
                splitterRect.x = positionX;
                splitterRect.width = width;
                EditorGUI.DrawRect(splitterRect, color);
            }
        }

        //ハンドルの描画
        private void OnSceneGUI()
        {
            //インスタンスの生成
            CameraController tes = target as CameraController;
            Vector3 size= Handles.ScaleHandle(Vector3.one, Vector3.zero, Quaternion.identity, 1);

            foreach (ListData d in tes.data)
            {
                //ツールが移動・回転の際の挙動
                if (Tools.current == Tool.Move) d.pos = Handles.PositionHandle(d.pos, d.rot);
                else if (Tools.current == Tool.Rotate) d.rot = Handles.RotationHandle(d.rot, d.pos);

                //ハンドルを描画
                d.LeftHandle = DrawHandle(d.pos, d.LeftHandle, tes.HandleColor);
                d.RightHandle = DrawHandle(d.pos, d.RightHandle, tes.HandleColor);
            }

            for (int i = 0; i < tes.data.Count; i++)
            {
                //始点と終点を定義
                Vector3 hand1 = tes.data[i].pos + tes.data[i].RightHandle;
                Vector3 hand2 = tes.data.Count == i + 1 ? tes.data[0].pos + tes.data[0].LeftHandle : tes.data[i + 1].pos + tes.data[i + 1].LeftHandle;
                int i2 = tes.data.Count == i + 1 ? 0 : i + 1;

                if(!tes.loop&&i+1==tes.data.Count)
                {
                    hand2 = tes.data[i].pos;
                    i2 = i;
                }

                //曲線を描く（　始点　,　終点　,　終点側の曲線向き　,　始点側の曲線向き　,　線の色　,　null　,　線の太さ）
                Handles.DrawBezier(tes.data[i].pos, tes.data[i2].pos, hand1, hand2, tes.PathColor, null, 3);
                //Debug.Log("pos:" + tes.data[i].pos + "   left:" + tes.data[i].LeftHandle + "    right:" + tes.data[i].RightHandle);
            }

            Vector3 DrawHandle(Vector3 PointPos, Vector3 HandlePos, Color color)
            {
                Color startColor = Handles.color;
                Handles.color = color;

                //ポイントが移動してもハンドルが動かないようなことが無いように、
                Vector3 handle = HandlePos + PointPos;
                //ハンドル用の球の描画 
                HandlePos = Handles.Slider2D(handle, Vector3.right, Vector3.right, Vector3.forward, 0.5f, Handles.SphereCap, 1f, false) - PointPos;
                //ハンドルと球を繋ぐ線の描画
                Handles.DrawLine(PointPos, PointPos + HandlePos);

                Handles.color = startColor;
                return HandlePos;
            }
        }
    }


    //シリアライズされたデータの自動判定を自前の判定に置き換える
    [CustomPropertyDrawer(typeof(ListData))]
    public class ListDrawer : PropertyDrawer
    {
        private int _labelWidth = 25;//ラベルの幅

        private string _posName = "pos";//ラベルの幅
        private string _rotName = "rot";//ラベルの幅
        private string _labelName = "次の地点に到達するまでの時間：";
        private string _label2Name = "ポイント色：";
        private string _timeName = "time";//ラベルの幅
        private string _colorName = "color";//ラベルの幅

        //GUIの変更
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //元は１つのプロパティ（Listという）であることを示すためにPropertyScopeで囲む。
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                //ラベルの描画幅を設定
                EditorGUIUtility.labelWidth = _labelWidth;
                //高さの描画設定
                position.height = EditorGUIUtility.singleLineHeight;



                //各要素の描画サイズ(x,y→位置、width,height→サイズ)
                //位置
                var posRect = new Rect(position)
                {
                    width = position.width,
                    height = position.height
                };
                //回転
                var rotRect = new Rect(position)
                {
                    y = posRect.y + EditorGUIUtility.singleLineHeight + 2,
                    width = position.width,
                    height = position.height
                };
                //ラベル
                var labelRect = new Rect(position)
                {
                    y = rotRect.y + EditorGUIUtility.singleLineHeight + 2,
                    width = 175
                };
                //時間
                var timeRect = new Rect(position)
                {
                    y = labelRect.y,
                    x = labelRect.x + labelRect.width + 7,
                    width = 64
                };
                //ラベル
                var label2Rect = new Rect(position)
                {
                    y = timeRect.y,
                    x = timeRect.x + timeRect.width + 15,
                    width = 65
                };
                //色
                var colorRect = new Rect(position)
                {
                    y = label2Rect.y,
                    x = label2Rect.x + label2Rect.width + 7,
                    width = 170
                };

                Rect splitterRect = new Rect(position)
                {
                    y = colorRect.y + EditorGUIUtility.singleLineHeight + 12,
                    x = 30,
                    height = 1,
                    width = 500
                };



                //各要素のプロパティを取得
                //位置
                var posProperty = property.FindPropertyRelative(_posName);
                //回転
                var rotProperty = property.FindPropertyRelative(_rotName);
                //色
                var colorProperty = property.FindPropertyRelative(_colorName);
                //時間
                var timeProperty = property.FindPropertyRelative(_timeName);



                //各プロパティーをインスペクター上でどのように描画するかを定義する。
                //位置
                posProperty.vector3Value = EditorGUI.Vector3Field(posRect, posProperty.name, posProperty.vector3Value);
                //回転
                Vector3 euler = rotProperty.quaternionValue.eulerAngles;
                euler = EditorGUI.Vector3Field(rotRect, "Euler Angle(Quaternion)", euler);
                rotProperty.quaternionValue = Quaternion.Euler(euler);
                //時間
                timeProperty.floatValue = EditorGUI.FloatField(timeRect, timeProperty.floatValue);
                //色情報
                colorProperty.colorValue = EditorGUI.ColorField(colorRect, colorProperty.colorValue);
                //ラベル・区切り線
                EditorGUI.DrawRect(splitterRect, Color.Lerp(Color.black, Color.gray, 0.7f));
                EditorGUI.LabelField(labelRect, _labelName);
                EditorGUI.LabelField(label2Rect, _label2Name);
            }
        }
    }
#endif
}
