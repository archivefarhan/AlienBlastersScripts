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
  public bool IsGrounded;
  Sprite _defaultSprite;
  SpriteRenderer _spriteRenderer;
  float _hortizontal;
  Animator _animator;

  void Awake() {
    _animator = GetComponent<Animator>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _defaultSprite = _spriteRenderer.sprite;
  }

  void OnDrawGizmos() {
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
    Gizmos.color = Color.red;
    Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
  }
  
  // Update is called once per frame
  void Update()
  {
    Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
    var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
    if (hit.collider)
      IsGrounded = true;
    else
      IsGrounded = false;
    
    _hortizontal = Input.GetAxis("Horizontal");
    Debug.Log(_hortizontal);
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    var vertical = rb.velocity.y;

    if (Input.GetButtonDown("Jump") && IsGrounded)
      _jumpEndTime = Time.time + _jumpDuration;

    if (Input.GetButton("Jump") && _jumpEndTime > Time.time)
      vertical = _jumpVelocity;

    _hortizontal *= _horizontalVelocity;
    rb.velocity = new Vector2(_hortizontal, vertical);
    UpdateSprite();
  }

  private void UpdateSprite()
  {
    _animator.SetBool("IsGrounded", IsGrounded);
    _animator.SetFloat("HorizontalSpeed", Math.Abs(_hortizontal));

    if (_hortizontal > 0)
      _spriteRenderer.flipX = false;
    else if (_hortizontal < 0)
      _spriteRenderer.flipX = true;
  }
}