using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class Heart : MonoBehaviour
    {
        Animator anim;

        void Awake()
        {
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// 点ける
        /// </summary>
        public void On()
        {
            anim.SetBool("On", true);
        }

        /// <summary>
        /// 消す
        /// </summary>
        public void Off()
        {
            anim.SetBool("On", false);
        }
    }
}
