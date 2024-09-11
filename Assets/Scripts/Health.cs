using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // �̹����� �����ҷ��� �ʿ�

public class Health : MonoBehaviour
{
    public float HP = 10f;
    public IHealthListener healthListener;

    public float InveinsibleTime = .6f;
    float LasthitTime;

    public Image HealthImage;
    float MaxHP;

    public AudioClip HitSound;
    public AudioClip DeathSound;

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = HP;
       healthListener = GetComponent<Health.IHealthListener>(); //Health�� IHealthListener ���� �ֵ� healthListener�� ��������
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage (float damage)
    {
        if(HP > 0 && LasthitTime + InveinsibleTime <Time.time)
        {
            HP -= damage;
            if(HealthImage != null) // HealthImage�� �ִ� �ֿ� ���ؼ� 
            {
                HealthImage.fillAmount = HP / MaxHP; // MaxHP ���� HP �ؼ� ���� ����� �ۼ�Ʈ�� HealthImage.fillAmount �� ����
            }

            LasthitTime = Time.time;

            if(HP <=0)
            {
                if(DeathSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(DeathSound);
                }
                if (healthListener != null) // ������ healthListener�� �ֵ� �� �־������
                {
                    healthListener.Die();
                }
            }
            else
            {
                if (HitSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(HitSound);
                }

            }
        }
    }

    public interface IHealthListener
    {
        void Die();
    }
}
