using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{

    public float time;
    public float damage = 10;
    public AudioClip ExplosionSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <=0)
        {
            if(!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(ExplosionSound); // �Ҹ� �ѹ��� ����
            }
            GetComponent<Animator>().SetTrigger("Explosion");
            Invoke("DestroyThis", .3f);
        }
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Health>().Damage(damage); 
        }
    }
}
