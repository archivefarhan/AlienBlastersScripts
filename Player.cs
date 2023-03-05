using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _jumpEndTime;
    [SerializeField] float _horizontalVelocity = 3;
    [SerializeField] float _jumpVelocity = 5;
    [SerializeField] float _jumpDuration = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _footOffset = 0.35f;

    public bool IsGrounded;
    SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    float _hortizontal;
    Animator _animator;
    int _jumpsRemaining;

  void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(
            transform.position.x,
            transform.position.y - spriteRenderer.bounds.extents.y
        );
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw Left Foot
        origin = new Vector2(
            transform.position.x - _footOffset,
            transform.position.y - spriteRenderer.bounds.extents.y
        );
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw Right Foot
        origin = new Vector2(
            transform.position.x + _footOffset,
            transform.position.y - spriteRenderer.bounds.extents.y
        );
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrounding();

        _hortizontal = Input.GetAxis("Horizontal");
        Debug.Log(_hortizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertical = rb.velocity.y;

        if (Input.GetButtonDown("Jump") && _jumpsRemaining > 0)
            {
              _jumpEndTime = Time.time + _jumpDuration;
              _jumpsRemaining--;
              _audioSource.Play();
            }

        if (Input.GetButton("Jump") && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        _hortizontal *= _horizontalVelocity;
        rb.velocity = new Vector2(_hortizontal, vertical);
        UpdateSprite();
    }

    void UpdateGrounding()
    {
        IsGrounded = false;

        //Check Center
        Vector2 origin = new Vector2(
            transform.position.x,
            transform.position.y - _spriteRenderer.bounds.extents.y
        );
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        // Check Left
        origin = new Vector2(
            transform.position.x - _footOffset,
            transform.position.y - _spriteRenderer.bounds.extents.y
        );
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        // Check Right
        origin = new Vector2(
            transform.position.x + _footOffset,
            transform.position.y - _spriteRenderer.bounds.extents.y
        );
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        if (IsGrounded && GetComponent<Rigidbody2D>().velocity.y <= 0)
        _jumpsRemaining = 2;
    }

    void UpdateSprite()
    {
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetFloat("HorizontalSpeed", Math.Abs(_hortizontal));

        if (_hortizontal > 0)
            _spriteRenderer.flipX = false;
        else if (_hortizontal < 0)
            _spriteRenderer.flipX = true;
    }
}
