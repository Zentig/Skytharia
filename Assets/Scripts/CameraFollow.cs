using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothing;

    [SerializeField] private Vector2 _minPos;
    [SerializeField] private Vector2 _maxPos;

    private void FixedUpdate()
    {
        if (transform.position != _target.position)
        {
            Vector3 targetPos = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, _minPos.x, _maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, _minPos.y, _maxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, _smoothing);
        }
    }
}
