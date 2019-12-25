using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungraviyEx2019
{
    public class ClearObject : MonoBehaviour
    {
        enum AnimType
        {
            None,
            Inhale,
            ToEat,
            Gulp
        }

        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void ToEatScale()
        {
            anim.SetInteger("State", (int)AnimType.ToEat);
        }
    }
}