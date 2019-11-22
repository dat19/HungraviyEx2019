using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class BlackholeMove : MonoBehaviour
    {
        Rigidbody2D rb;
        [SerializeField]
        float speed = 0.5f;
        [SerializeField]
        float maxDistance = 15f;
        [SerializeField]
        float maxSpeed = 0.5f;

        GameObject blackholeObject = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            blackholeObject = GameObject.FindGameObjectWithTag("Blackhole");
        }

        void Update()
        {
            if (!GameParams.CanMove)
            {
                return;
            }

            if (blackholeObject != null)
            {
                Vector3 move = blackholeObject.transform.position - transform.position;
                //a.velocity = move.normalized * speed; 

                float kyori = Vector2.Distance(blackholeObject.transform.position, transform.position);
                if (kyori <= maxDistance)
                {
                    float kasoku = (-0.5f / 15f) * kyori + 0.5f;
                    rb.AddForce(move.normalized * kasoku, ForceMode2D.Impulse);
                }
            }
        }
    }
}