using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 패키지 매니저에서 AI 네비게이션 설치 필요

public class Enemy : MonoBehaviour, Health.IHealthListener // Health의 IHealthListener 안의 것도 쓸거다
{
    enum State
    {
        Idle,
        Walk,
        Attack,
        Die
    }

    public GameObject Player;
    NavMeshAgent agent;
    State state;
    Animator animator;

    float NextStateTime = 2f;

    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;

    }

    // Update is called once per frame
    void Update()
    {

        switch(state)
        {
            case State.Idle:
                                                         // 거리 구하기 // 플레이어 위치 - 자기위치+ 캡슐 콜라이더 중심부 // magnitude 벡터 크기 가져오기
                float distance = (Player.transform.position - (transform.position + GetComponent<CapsuleCollider>().center)).magnitude; 
                if(distance < 1.8f)  // 목적지까지 남은 거리가 1.8 이하
                {
                    Attack();
                }
                else
                {
                    NextStateTime -= Time.deltaTime;
                    if (NextStateTime < 0)
                    {
                        StartWalk();
                    }
                }
                break;

            case State.Walk:

                if(agent. remainingDistance < 1.8f || !agent.hasPath) // 목적지까지 남은 거리가 1.8 이하 || 목적지에 도달하는 길이 없다고 판단됨
                {
                    StartIdle();
                }
                break;

             case State.Attack:

                NextStateTime -= Time.deltaTime;
                if (NextStateTime < 0)
                {
                    StartIdle();
                }
                break;

        }
    }

    void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        NextStateTime = Random.Range(1f, 2f);
        agent.isStopped = true; //움직이지 마
        animator.SetTrigger("Idle");
    }

    void StartWalk()
    {
        audio.Play();
        state = State.Walk;
        agent.destination = Player.transform.position;
        agent.isStopped = false; // 움직여
        animator.SetTrigger("Walk");
    }
    
    void Attack()
    {
        state = State.Attack;
        NextStateTime = 1.5f;
        animator.SetTrigger("Attack");
    }

    public void Die()
    {
        state = State.Die;
        Debug.Log("RIP");
        agent.isStopped = true; //움직이지 마
        animator.SetTrigger("Die");
        Invoke("DestroyThis", 2f);
    }

    void DestroyThis()
    {
        GameManager.instance.EnemyCount();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(1);
        }
    }


}
