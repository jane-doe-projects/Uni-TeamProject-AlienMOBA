using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimationController : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    Animator anim;
    NavMeshAgent agent;
    StateManager state;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        state = GameObject.Find("GameManager").GetComponent<GameManager>().state;
        anim = GameObject.Find("Player/Alien").GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        // checking for partial path status prevents the move animation from triggering when the player slighthly gets pushed by creeps
        if (agent.velocity.magnitude > 0 || agent.pathStatus == NavMeshPathStatus.PathPartial)
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);

        anim.SetBool("playerDead", state.PlayerIsDead);
    }

    public void TriggerSpellAttackAnim()
    {
        anim.SetTrigger("playerUsesSpell");
    }

    public void SetMatchStarted()
    {
        anim.SetBool("matchStarted", true);
    }
}
