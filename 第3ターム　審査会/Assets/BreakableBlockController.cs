using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack タグを持つオブジェクトのトリガーとの接触が発生したら、パーティクルを再生してオブジェクトを消す機能を提供するコンポーネント
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class BreakableBlockController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            // ブロックのコライダーを無効にする
            Collider col = GetComponent<Collider>();
            col.enabled = false;

            // パーティクルを再生する
            ParticleSystem ps = GetComponent<ParticleSystem>();
            ps.Play();
            Destroy(this.gameObject, ps.main.duration); // 再生が終わったら破棄する

            // 描画を消す
            Renderer r = GetComponent<Renderer>();
            r.enabled = false;
        }
    }
}
