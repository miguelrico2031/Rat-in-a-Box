using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMBGMove : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;

    private Material _moveMat;

    private void Awake()
    {
        _moveMat = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        _moveMat.mainTextureOffset = Time.realtimeSinceStartup * _scrollSpeed * Vector2.one;
    }
}
