
using System;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
   [SerializeField]  Transform target;
    [SerializeField] float searchRadius;
    [SerializeField] Health currentTarget;
    Animator animator;
    private Transform handTransform;
    [SerializeField] float areaAttack = 0.2f;
    [SerializeField] LayerMask playerLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameObject targetGO = GameObject.FindGameObjectWithTag("Player");
        if(targetGO != null)
        {
            target = targetGO.transform;
        }
        
        handTransform = animator.GetBoneTransform(HumanBodyBones.RightHand);
    }

    private void Update()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        Health enemyHealth = FindNewTargetInRange(searchRadius);
        if (enemyHealth == null)
        {
            return;
        }
        currentTarget = enemyHealth;
    }

    private Health FindNewTargetInRange(float searchRadius)
    {
        Health best = null;
        float bestDistance = Mathf.Infinity;
        foreach (var candidate in FindAllTargetsInRange(searchRadius))
        {
            float candidateDistance = Vector3.Distance(transform.position, candidate.transform.position);
            if (candidateDistance < bestDistance)
            {
                best = candidate;
                bestDistance = candidateDistance;
            }
        }
        return best;
    }

    private IEnumerable<Health> FindAllTargetsInRange(float searchRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, searchRadius, Vector3.up, 0);

        foreach (var hit in hits)
        {
            Health target = hit.transform.GetComponent<Health>();
            
            if(target == null)
            {
                continue;
            }
            if(target.gameObject != this.gameObject)
            {
                yield return target;
            }           
        }
    }

    public Health GetCurrentTarget()
    {
        return currentTarget;
    }

    public Transform GetPlayer()
    {
        return target;
    }
    //Animation Event
    void Hit()
    {

        float damage = 6f;
        Collider[] colliders = Physics.OverlapSphere(handTransform.position, areaAttack, playerLayer);

        if (colliders.Length > 0)
        {

            foreach (Collider collider in colliders)
            {
                collider.GetComponent<Health>()?.TakeDamage(this.gameObject, damage);
            }
        }


        if (target == null) { return; }

    }
}
