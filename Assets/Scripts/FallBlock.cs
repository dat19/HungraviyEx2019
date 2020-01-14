using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class FallBlock : MonoBehaviour
    {
        Animator anim = null;
        Rigidbody2D rb = null;
        const float DestroySeconds = 1f;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player") && rb.bodyType == RigidbodyType2D.Kinematic)
            {
                SoundController.Play(SoundController.SeType.FallBlock);
                rb.bodyType = RigidbodyType2D.Dynamic;
                anim.SetTrigger("Fall");
                Destroy(gameObject, DestroySeconds);
            }
        }
    }
}