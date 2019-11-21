using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Controller を使って入力に従ってキャラクターを動かす
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerControllerCc : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [SerializeField] float m_moveSpeed = 1f;
    /// <summary>重力を働かせるか</summary>
    [SerializeField] bool m_useGravity = false;
    /// <summary>重力のスケール</summary>
    [SerializeField] float m_gravityScale = 2f;
    /// <summary>衝突時のログを出力するかどうか</summary>
    [SerializeField] bool m_printCollisionLog = false;
    CharacterController m_cc;
    Animator m_anim;

    void Start()
    {
        m_cc = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 方向の入力を取得し、入力方向の単位ベクトルを計算する
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = (v * Vector3.forward + h * Vector3.right).normalized; // dir: 移動方向・速度を決定するベクトル

        // 入力がある場合は平面上の移動ベクトルを求める
        if (dir != Vector3.zero)
        {
            this.transform.forward = dir;
            dir = dir * m_moveSpeed;
        }

        m_anim.SetFloat("Speed", dir.magnitude);    // アニメーターにパラメーターを渡す

        // 重力を使う時かつ接地していない場合は重力方向の移動ベクトルを加える
        if (m_useGravity && !m_cc.isGrounded)
        {
            dir += Physics.gravity * m_gravityScale * Time.deltaTime;
        }

        // Character Controller を使って移動する
        m_cc.Move(dir * Time.deltaTime);
    }

    /// <summary>
    /// Character Controller の接触判定を行う関数
    /// Collider の時とは違うものを使うことに注意すること
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (m_printCollisionLog)
            Debug.Log(hit.gameObject.name + " と接触した");
    }
}
