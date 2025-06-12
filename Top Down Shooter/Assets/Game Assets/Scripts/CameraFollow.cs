using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private bool _initialized;

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private void LateUpdate()
    {
        if (_target)
        {
            Vector3 desiredPosition = _target.position + _offset;
            desiredPosition.y = _offset.y;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * _speed);
            transform.position = smoothedPosition;
        }
    }

    private IEnumerator FindPlayer()
    {
        GameObject target = GameObject.FindGameObjectWithTag("MainCharacter");
        
        yield return null;

        if (target != null)
        {
            _target = target.transform;
            _initialized = true;
        }
        else
            StartCoroutine(FindPlayer());
    }
}