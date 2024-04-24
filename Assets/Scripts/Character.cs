using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected int life;
    protected int strength;
    protected float speed;
    protected float attackDelay;
    protected float lastAttack;

    protected abstract void MoveTo(Vector3 direction);

    protected abstract void Attack();

    public abstract void TakeDamage(int damage);


}
