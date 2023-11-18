using System;
using System.Collections;
using System.Collections.Generic;
using Skytharia.Enemy;
using UnityEngine;

public class InitialEnemyTestControl : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;

    private bool _allowDamage = true;
    private EnemyBase _spawnedEnemy = null;

    private void Start()
    {
        _spawnedEnemy = Instantiate(enemy);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _allowDamage)
        {
            if (_spawnedEnemy)
                _spawnedEnemy.ApplyDamage(10.0f);
            else
                _spawnedEnemy = Instantiate(enemy);
            _allowDamage = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            _allowDamage = true;
    }
}
