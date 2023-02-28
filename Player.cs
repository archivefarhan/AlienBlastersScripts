using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private float _jumpEndTime;
  [SerializeField] private float _jumpVelocity = 5;
  [SerializeField] private float _jumpDuration = 0.5f;
  
  void OnDrawGizmos() {
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    float bottomY = spriteRenderer.bounds.extents.y;
    Vector2 origin = new Vector2(transform.position.x, transform.position.y - bottomY);
    Gizmos.color = Color.red;
    Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
  }
  
  // Update is called once per frame
  void Update()
  {
    var hortizontal = Input.GetAxis("Horizontal");
    Debug.Log(hortizontal);
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    var vertical = rb.velocity.y;

    if (Input.GetButtonDown("Jump"))
      _jumpEndTime = Time.time + _jumpDuration;

    if (Input.GetButton("Jump") && _jumpEndTime > Time.time)
      vertical = _jumpVelocity;


    rb.velocity = new Vector2(hortizontal, vertical);
  }
}
