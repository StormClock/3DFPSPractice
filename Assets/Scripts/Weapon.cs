using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public TextMeshProUGUI BulletNumberLabel;
    public GameObject TrailPrefab;
    public GameObject particlePrefab;
    public Transform FiringPosition;

    public int Bullet;
    public int TotalBullet;
    public int MaxMagazine;

    public AudioClip GunshotSound;

    public float damage = 2f;

    Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Bullet > 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Shot");
            }
            Fire();
            Bullet--;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (animator != null)
            {
                animator.SetTrigger("Reload");
            }


            if (TotalBullet >= MaxMagazine - Bullet)
            {
                TotalBullet -= MaxMagazine - Bullet;
                Bullet = MaxMagazine;
            }
            else
            {
                Bullet += TotalBullet;
                TotalBullet = 0;
            }
        }

        BulletNumberLabel.text = Bullet + " / " + TotalBullet;

    }

    virtual protected void Fire() // virtual �������� ���� ��ӹ������� ǥ��
    {
        RaycastFire();
    }

    // ����ĳ���� �߻�(��Ʈ��ĵ)
    void RaycastFire()
    {

        Camera cam = Camera.main; // ����ī�޶� ��������

        GetComponent<AudioSource>().PlayOneShot(GunshotSound);//�Ҹ� �ѹ��� ����

        RaycastHit hit;
        Ray r = cam.ViewportPointToRay(Vector3.one / 2); // ViewportPoint ȭ�� ���ʾƷ��� 0,0 ������ ���� 1,1 �� ��� Vector3.one / 2 0.5,0.5 �� ToRay �� ���

        Vector3 hitPosition = r.origin + r.direction * 200; // r �������� r.direction ���� �ִ� ���� * 200; 200���ͱ��� �� �߱�
       
        if(Physics.Raycast(r,out hit,1000)) // ���� ��� 1000���� ���� �Ÿ����� ��򰡿� �¾Ҵ�
        {
            hitPosition = hit.point;
            GameObject particle = Instantiate(particlePrefab); // ��ƼŬ ��ȯ
            particle.transform.position = hitPosition; // ��ȯ��ġ�� ��Ʈ������
            particle.transform.forward = hit.normal; // ��ź���� ������������ ����

            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().Damage(damage); 
            }


        }

        if(TrailPrefab != null)
        {
            GameObject obj = Instantiate(TrailPrefab);
            Vector3[] pos = new Vector3[] { FiringPosition.position, hitPosition }; //�� ���� ���� �� ��ǥ Ȯ��
            obj.GetComponent<LineRenderer>().SetPositions(pos); // LineRenderer�� SetPositions(pos) ��ǥ ������ �� �׸���

            StartCoroutine(RemoveTrail(obj)); // RemoveTrail �� �ڷ�ƾ���� �����ϰڴ� // ���� �Լ��� ������ ���������� ����
        }

        

        IEnumerator RemoveTrail(GameObject obj) // IEnumerator �ڷ�ƾ ������ �ʿ���
        {
            yield return new WaitForSeconds(0.3f); // .3�� ��ٷ��ּ���

            Destroy(obj);
        }
    }
}
