using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ��Ű�� �Ŵ������� AI �׺���̼� ��ġ �ʿ�

public class Enemy : MonoBehaviour, Health.IHealthListener // Health�� IHealthListener ���� �͵� ���Ŵ�
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
                                                         // �Ÿ� ���ϱ� // �÷��̾� ��ġ - �ڱ���ġ+ ĸ�� �ݶ��̴� �߽ɺ� // magnitude ���� ũ�� ��������
                float distance = (Player.transform.position - (transform.position + GetComponent<CapsuleCollider>().center)).magnitude; 
                if(distance < 1.8f)  // ���������� ���� �Ÿ��� 1.8 ����
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

                if(agent. remainingDistance < 1.8f || !agent.hasPath) // ���������� ���� �Ÿ��� 1.8 ���� || �������� �����ϴ� ���� ���ٰ� �Ǵܵ�
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
        agent.isStopped = true; //�������� ��
        animator.SetTrigger("Idle");
    }

    void StartWalk()
    {
        audio.Play();
        state = State.Walk;
        agent.destination = Player.transform.position;
        agent.isStopped = false; // ������
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
        agent.isStopped = true; //�������� ��
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
