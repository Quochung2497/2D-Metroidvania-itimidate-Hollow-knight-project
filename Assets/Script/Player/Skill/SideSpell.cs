﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSpell : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] float speed;
    [SerializeField] float lifetime = 1f;
    [Header("HitObjectVfx")]
    [SerializeField] private GameObject HitObjectSplashEffect;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        // Di chuyển đạn với tốc độ đã được điều chỉnh theo thời gian thực
        transform.position += (speed * Time.fixedDeltaTime * transform.right);
    }

    // Phát hiện va chạm
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Enemy"))
        {
            HandleCollisionWithEnemy(_other);
        }
        else if(_other.CompareTag("Wall"))
        {
            HandleCollision(HitObjectSplashEffect);
            //Debug.Log("Cham vat can");
        }
    }
    private void HandleCollisionWithEnemy(Collider2D enemyCollider)
    {
        // Đảm bảo rằng Enemy có script với phương thức EnemyGetsHit phù hợp
        Enemy enemy = enemyCollider.GetComponent<Enemy>();
        if (enemy != null)
        {
            Vector2 hitDirection = (enemyCollider.transform.position - transform.position).normalized;
            enemy.EnemyGetsHit(damage, hitDirection, -hitForce);
        }

        HandleCollision(HitObjectSplashEffect);
    }
    private void HandleCollision(GameObject effect)
    {
        // Kích hoạt hiệu ứng và hủy đạn khi đụng tường hoặc mặt đất
        ActiveEffect(effect);
        Destroy(gameObject);
    }

    private void ActiveEffect(GameObject effect)
    {
        // Xác định hướng và góc quay của HitEnemySplashEffect dựa trên hướng của đạn
        Quaternion rotation = transform.right.x > 0
            ? Quaternion.Euler(0, 180, 0)          // Nếu đạn bắn từ trái sang phải
            : Quaternion.Euler(0, 0, 0);       // Nếu đạn bắn từ phải sang trái

        Instantiate(effect, transform.position, rotation);
    }
}
