using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _speed;
    private Animator _anim;
    private Vector2 _direction;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _speed = _moveSpeed;
    }
    private void Update()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");

        _anim.SetFloat("Horizontal", _direction.x);
        _anim.SetFloat("Vertical", _direction.y);
        _anim.SetFloat("Speed", _direction.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _runSpeed;
            _anim.SetBool("IsRunning", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _moveSpeed;
            _anim.SetBool("IsRunning", false);
        }       
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * _speed * Time.deltaTime);
    }
}

