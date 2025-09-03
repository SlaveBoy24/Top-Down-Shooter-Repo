using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyPhoton _enemyPhoton;
    [SerializeField] private float _healths;
    [SerializeField] private int _damage;
    [SerializeField] private bool _isAlive;
    [SerializeField] private bool _canAttack;

    private NavMeshTriangulation Triangulation;
    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Transform> _targets;
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private GamePlayer _currentTargetController;

    [SerializeField] private bool _initialized;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(_agent);
            Destroy(this);
        }
        Initialize();
    }

    public virtual void Initialize()
    {
        Triangulation = NavMesh.CalculateTriangulation();
        _agent.updatePosition = false;
        _agent.updateRotation = true;
        _canAttack = true;
        _isAlive = true;

        StartCoroutine("EnemyLogic");
        _initialized = true;
    }

    private void Update()
    {
        if (_initialized && _isAlive)
        {
            SynchronizeAnimatorAndAgent();
            CheckAttack();
        }
    }

    private void SynchronizeAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;
        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        Velocity = SmoothDeltaPosition / Time.deltaTime;
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            Velocity = Vector2.Lerp(Vector2.zero, Velocity, _agent.remainingDistance);
        }

        bool shouldMove = Velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.stoppingDistance;

        _animator.SetBool("Run", shouldMove);
        _animator.SetFloat("Velocity", Mathf.Clamp01(Velocity.x + Velocity.y));
    }

    private IEnumerator EnemyLogic()
    {
        while (enabled)
        {
            if (_currentTarget)
                _agent.SetDestination(_currentTarget.position);

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void DamageDeal()
    {
        if (Vector3.Distance(transform.position, _currentTarget.position) <= 4)
            _currentTargetController.GetDamage(_damage);
    }

    public void CheckAttack()
    {
        if (_currentTarget == null)
            return;

        if (Vector3.Distance(transform.position, _currentTarget.position) <= 1)
        {
            if (_canAttack)
            {
                Debug.Log("ATAKA");
                _enemyPhoton.SetTrigger("Punch");
                _canAttack = false;
                StartCoroutine("AttackEnd");
            }
        }
    }

    private IEnumerator AttackEnd()
    {
        yield return new WaitForSeconds(4f);
        _canAttack = true;
    }

    private void OnAnimatorMove()
    {
        if (_initialized && _isAlive)
        {
            Vector3 rootPosition = _animator.rootPosition;
            rootPosition.y = _agent.nextPosition.y;
            transform.position = rootPosition;
            _agent.nextPosition = rootPosition;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCharacter" || other.gameObject.tag == "OtherCharacter")
        {
            _targets.Add(other.transform);
            _currentTarget = _targets[0];
            _currentTargetController = _currentTarget.transform.parent.GetComponent<GamePlayer>();
        }
    }
}
