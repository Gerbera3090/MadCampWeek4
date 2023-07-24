using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public float speed = 10f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
