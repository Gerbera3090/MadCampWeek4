using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 behavior가 있는 state은 fade하게 됨
public class FadeRemoveBehavior : StateMachineBehaviour
{
    public float fadeTime = 0.05f;
    private float timePassed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject objectToRemove;

    Color startColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       spriteRenderer = animator.GetComponent<SpriteRenderer>();
       startColor = spriteRenderer.color;
       objectToRemove = animator.gameObject;
    }

    // 색의 투명도를 시간에 따라 계속 증가
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timePassed += Time.deltaTime;

        float newAlpha = startColor.a * (1 - timePassed / fadeTime); // 지난 시간에 반비례하게 투명도 변함

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha); // 새 색으로 설정

        if(timePassed > fadeTime) {
            //Destroy(objectToRemove); 
            objectToRemove.SetActive(false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
