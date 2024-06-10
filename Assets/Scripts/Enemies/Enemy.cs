using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 30f;
    public float moveSpeed;
    public float attackDistance;
    public float attackTimer;

    [Header("Patrolling")]
    public Transform leftPoint;
    public Transform rightPoint;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject agroZone;
    public GameObject triggerArea;

    [Header("Attack")]
    public bool isRange = false;
    public GameObject arrowPrefab;
    public Transform shootPoint;

    private bool isFacingRight;

    private Animator _animator;
    private float _distance;
    private bool _attackMode;
    private bool _cooling;
    private float intTimer;

    // STATIC STRINGS
    private const string IDLE_ANIM = "Idle";
    private const string WALK_ANIM = "Walk";
    private const string ATTACK1_ANIM = "Attack1";
    private const string TAKE_DAMAGE_ANIM = "Take Damage";
    private const string DEATH_ANIM = "Death";

    private void Start()
    {
        intTimer = attackTimer;
        _animator = GetComponentInChildren<Animator>();
        SelectMoveTarget();
    }

    private void Update()
    {
        if (!_attackMode)
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !_animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK1_ANIM))
        {
            SelectMoveTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    private void EnemyLogic()
    {
        _distance = Vector2.Distance(transform.position, target.position);

        if (_distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= _distance && !_cooling)
        {
            Attack();
        }

        if (_cooling)
        {
            CoolDown();
            //_animator.SetBool(ATTACK1_ANIM, false);
        }
    }

    private void Move()
    {
        _animator.SetBool(WALK_ANIM, true);
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK1_ANIM))
        {
            Vector2 targetPostion = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPostion, moveSpeed * Time.deltaTime);
        }
    }

    public void SelectMoveTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftPoint.position);
        float distanceToRight = Vector2.Distance(transform.position, rightPoint.position);

        if (distanceToLeft > distanceToRight)
            target = leftPoint;
        else
            target = rightPoint;

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            isFacingRight = false;
            rotation.y = 180f;
        }
        else
        {
            isFacingRight = true;
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }

    private void Attack()
    {
        attackTimer = intTimer;
        _attackMode = true;

        _animator.SetBool(WALK_ANIM, false);
        //_animator.SetBool(ATTACK1_ANIM, true);
        _animator.SetTrigger(ATTACK1_ANIM);
        if(isRange)
            StartCoroutine(ShootArrow());

        TriggerCooling();
    }

    public IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(0.2f);
        var gb = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

        EnemyArrow arrow;
        gb.TryGetComponent<EnemyArrow>(out arrow);
        arrow?.Fire(isFacingRight);
    }

    private void StopAttack()
    {
        _cooling = false;
        _attackMode = false;
        //_animator.SetBool(ATTACK1_ANIM, false);
    }

    private void CoolDown()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0 && _cooling && _attackMode)
        {
            _cooling = false;
            attackTimer = intTimer;
        }
    }

    private void TriggerCooling()
    {
        _cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftPoint.position.x && transform.position.x < rightPoint.position.x;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            _animator.SetTrigger(DEATH_ANIM);
            this.enabled = false;
            StartCoroutine(CallDeath(3f));
            return;
        }

        _animator.SetTrigger(TAKE_DAMAGE_ANIM);
    }

    private IEnumerator CallDeath(float delay)
    {
        this.enabled = false;
        GameManager.Instance.AddToCoins(10);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
