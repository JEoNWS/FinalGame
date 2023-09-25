using System.Collections;
using UnityEngine;

public class TrainingBot : Enemy
{
    Animator anim;
    Rigidbody2D rigd;

    RaycastHit2D rayHit;

    CurrentState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
    }

    public override void Hit(float rotY, float force)
    {
        anim.SetTrigger("Hit");
    }

    protected override void Die()
    {

    }
    protected override void Facing(float a)
    {
    }
    public override void StateChange(int state)
    {
        this.state = (CurrentState)state;
    }
    public override void TurnAround()
    {
    }
    public override void Stun()
    {
    }

    public override void ReleaseStun()
    {
    }
}