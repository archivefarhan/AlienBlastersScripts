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

  private void Awake() {
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
    {
      IsGrounded = true;
      _spriteRenderer.sprite = _defaultSprite;
    }
    else
    {
      IsGrounded = false;
      _spriteRenderer.sprite = _jumpSprite;
    }
    var hortizontal = Input.GetAxis("Horizontal");
    Debug.Log(hortizontal);
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    var vertical = rb.velocity.y;

    if (Input.GetButtonDown("Jump") && IsGrounded)
      _jumpEndTime = Time.time + _jumpDuration;

    if (Input.GetButton("Jump") && _jumpEndTime > Time.time)
      vertical = _jumpVelocity;

    hortizontal *= _horizontalVelocity;
    rb.velocity = new Vector2(hortizontal, vertical);
  }
}