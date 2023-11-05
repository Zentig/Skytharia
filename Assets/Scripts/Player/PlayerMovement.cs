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
    private Player _playerStats;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _speed = _moveSpeed;
        _playerStats = gameObject.GetComponent<Player>();
    }
    private void Update()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");

        _anim.SetFloat("Horizontal", _direction.x);
        _anim.SetFloat("Vertical", _direction.y);
        _anim.SetFloat("Speed", _direction.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.LeftShift) && _anim.GetFloat("Speed") > 0.1f && _playerStats.Energy > 20 && _playerStats.PlayerState == PlayerState.Idle)
        {
            StartCoroutine(Running());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _playerStats.PlayerState == PlayerState.Run || _playerStats.Energy < 20)
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
        _speed = _runSpeed;
        _anim.SetBool("IsRunning", true);
        _playerStats.PlayerState = PlayerState.Run;
        while (_playerStats.PlayerState == PlayerState.Run)
        {
            _playerStats.Energy -= 2;
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(Running());
    }
    private IEnumerator RestoreEnergy()
    {
        _speed = _moveSpeed;
        _anim.SetBool("IsRunning", false);
        _playerStats.PlayerState = PlayerState.Idle;
        while (_playerStats.PlayerState == PlayerState.Idle)
        {
            _playerStats.Energy++;
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(RestoreEnergy());
    }
}

