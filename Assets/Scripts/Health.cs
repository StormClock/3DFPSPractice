using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 이미지에 적용할려면 필요

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
       healthListener = GetComponent<Health.IHealthListener>(); //Health의 IHealthListener 쓰는 애들 healthListener에 데려오기
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
            if(HealthImage != null) // HealthImage가 있는 애에 한해서 
            {
                HealthImage.fillAmount = HP / MaxHP; // MaxHP 분의 HP 해서 비율 계산한 퍼센트를 HealthImage.fillAmount 에 적용
            }

            LasthitTime = Time.time;

            if(HP <=0)
            {
                if(DeathSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(DeathSound);
                }
                if (healthListener != null) // 위에서 healthListener에 애들 잘 넣어놨으면
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
