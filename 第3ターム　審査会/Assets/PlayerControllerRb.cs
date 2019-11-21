using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRb : MonoBehaviour
{
    /// <summary>力を加えて動かすか/速度を直接操作するか</summary>
    [SerializeField] MovingType m_movingType = MovingType.AddForce;
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>接地判定の際、足元からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 0.2f;
    Rigidbody m_rb;
    Animator m_anim;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = (Vector3.forward * v + Vector3.right * h).normalized;

        // 入力があれば、そちらの方向に動かす
        if (dir != Vector3.zero)
        {
            this.transform.forward = dir;
            if (m_movingType == MovingType.AddForce)
            {
                m_rb.AddForce(this.transform.forward * m_movingSpeed);
            }
            else if (m_movingType == MovingType.Velocity)
            {
                m_rb.velocity = this.transform.forward * m_movingSpeed;
            }
        }

        // Animator Controller のパラメータをセットする
        m_anim.SetFloat("Speed", m_rb.velocity.magnitude);

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
            // Animator Controller のパラメータをセットする
            m_anim.SetTrigger("Jump");
        }
    }

    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Vector3 start = this.transform.position + col.center;   // start: 体の中心
        Vector3 end = start + Vector3.down * (col.center.y + m_isGroundedLength);  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }
}

public enum MovingType
{
    AddForce,
    Velocity,
}