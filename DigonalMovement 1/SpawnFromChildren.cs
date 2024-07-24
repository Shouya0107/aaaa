using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFromChildren : MonoBehaviour
{
    // 生成するスプライトをインスペクターで指定
    public List<Sprite> objectsToSpawn;
    // 生成間隔をインスペクターで指定
    public float spawnInterval = 2.0f;
    // 子要素のリスト
    private List<Transform> childTransforms = new List<Transform>();
    // 生成するオブジェクトのサイズをインスペクターで指定
    public Vector3 fixedSize = new Vector3(1.0f, 1.0f, 1.0f);
    // 生成するオブジェクトの移動速度と角度をインスペクターで指定
    public float moveSpeed = 1.0f;
    public float moveAngle = 45.0f;
    // ランダム化のフラグをインスペクターで指定
    public bool useRandomSprites = false;

    // Startメソッドは最初に呼び出される
    void Start()
    {
        // 子要素の位置を取得
        foreach (Transform child in transform)
        {
            childTransforms.Add(child);
        }

        // 一定間隔でオブジェクトを生成するコルーチンを開始
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            for (int i = 0; i < childTransforms.Count; i++)
            {
                // 子要素の位置にオブジェクトを生成
                if (objectsToSpawn.Count > 0)
                {
                    // スプライトを選択
                    Sprite selectedSprite;
                    if (useRandomSprites)
                    {
                        // ランダムなインデックスを取得
                        int randomIndex = Random.Range(0, objectsToSpawn.Count);
                        selectedSprite = objectsToSpawn[randomIndex];
                    }
                    else
                    {
                        // 順番にスプライトを取得
                        selectedSprite = objectsToSpawn[i % objectsToSpawn.Count];
                    }

                    // 新しいGameObjectを作成
                    GameObject spawnedObject = new GameObject("SpawnedObject");
                    // SpriteRendererコンポーネントを追加
                    SpriteRenderer renderer = spawnedObject.AddComponent<SpriteRenderer>();
                    // スプライトを設定
                    renderer.sprite = selectedSprite;
                    // 位置を設定
                    spawnedObject.transform.position = childTransforms[i].position;
                    // サイズを固定
                    spawnedObject.transform.localScale = fixedSize;
                    // 生成したオブジェクトにDiagonalMoveスクリプトをアタッチ
                    DiagonalMove moveScript = spawnedObject.AddComponent<DiagonalMove>();
                    // 生成したオブジェクトの速度と角度を設定
                    moveScript.speed = moveSpeed;
                    moveScript.angle = moveAngle;
                }
            }
            // 指定した間隔で待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
