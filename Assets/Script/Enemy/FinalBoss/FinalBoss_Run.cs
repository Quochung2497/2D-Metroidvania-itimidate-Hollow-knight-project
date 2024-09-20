using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss_Run : StateMachineBehaviour
{
    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TargetPlayerPosition(animator);

        if (FinalBoss.Instance.attackCountdown <= 0)
        {
            FinalBoss.Instance.AttackHandler();
            FinalBoss.Instance.attackCountdown = Random.Range(FinalBoss.Instance.attackTimer - 1, FinalBoss.Instance.attackTimer + 1);
        }
    }
    void TargetPlayerPosition(Animator animator)
    {
        if (FinalBoss.Instance.Grounded())
        {
            FinalBoss.Instance.Flip();
            Vector2 _target = new Vector2(PlayerController.Instance.transform.position.x, rb.position.y);
            Vector2 _newPos = Vector2.MoveTowards(rb.position, _target, FinalBoss.Instance.runSpeed * Time.fixedDeltaTime);
            rb.MovePosition(_newPos);

        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -25); //if knight is not grounded, fall to ground
        }
        if (Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= FinalBoss.Instance.attackRange)
        {
            animator.SetBool("Run", false);
        }
        else
        {
            return;
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Run", false);
    }
}
