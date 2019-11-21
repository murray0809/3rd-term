using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃のコライダー（トリガー）の有効/無効を切り替える
/// Animator と同じ GameObject に追加し、Animation Event から関数を呼び出すことを前提に作られている。
/// </summary>
public class AttackController : MonoBehaviour
{
    /// <summary>攻撃範囲のコライダー</summary>
    [SerializeField] Collider m_attackRange;

    void Start()
    {
        // コライダーが有効になっていたら無効にする
        if (m_attackRange.gameObject.activeSelf)
        {
            EndAttack();
        }
    }

    /// <summary>
    /// 攻撃を開始する時に呼ぶ
    /// コライダーが有効になる
    /// </summary>
    void BeginAttack()
    {
        m_attackRange.gameObject.SetActive(true);
    }

    /// <summary>
    /// 攻撃を終了する時に呼ぶ
    /// コライダーが無効になる
    /// </summary>
    void EndAttack()
    {
        m_attackRange.gameObject.SetActive(false);
    }
}
