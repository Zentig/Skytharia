using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Animator _anim;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private Player _player;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _speed = _player.MoveSpeed;
        _player = gameObject.GetComponent<Player>();

        StartCoroutine(RestoreEnergy());
    }
    private void Update()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");

        _anim.SetFloat("Horizontal", _direction.x);
        _anim.SetFloat("Vertical", _direction.y);
        _anim.SetFloat("Speed", _direction.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.LeftShift) && _anim.GetFloat("Speed") > 0.1f && _player.Energy > 20 && _player.PlayerState == PlayerState.Idle)
        {
            StartCoroutine(Running());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _player.PlayerState == PlayerState.Run || _player.Energy < 20)
        {
            StartCoroutine(RestoreEnergy());
        }    
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * _speed * Time.deltaTime);
    }
    private IEnumerator Running()
    {
        _speed = _player.RunSpeed;
        _anim.SetBool("IsRunning", true);
        _player.PlayerState = PlayerState.Run;
        while (_player.PlayerState == PlayerState.Run)
        {
            _player.Energy -= 2;
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(Running());
    }
    private IEnumerator RestoreEnergy()
    {
        _speed = _player.MoveSpeed;
        _anim.SetBool("IsRunning", false);
        _player.PlayerState = PlayerState.Idle;
        while (_player.PlayerState == PlayerState.Idle)
        {
            _player.Energy++;
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(RestoreEnergy());
    }
}

