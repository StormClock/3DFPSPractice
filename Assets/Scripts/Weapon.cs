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

    virtual protected void Fire() // virtual 누군가가 나를 상속받을거임 표시
    {
        RaycastFire();
    }

    // 레이캐스팅 발사(히트스캔)
    void RaycastFire()
    {

        Camera cam = Camera.main; // 메인카메라 가져오기

        GetComponent<AudioSource>().PlayOneShot(GunshotSound);//소리 한번만 내기

        RaycastHit hit;
        Ray r = cam.ViewportPointToRay(Vector3.one / 2); // ViewportPoint 화면 왼쪽아래를 0,0 오른쪽 위를 1,1 로 잡고 Vector3.one / 2 0.5,0.5 에 ToRay 빛 쏘기

        Vector3 hitPosition = r.origin + r.direction * 200; // r 원점에서 r.direction 보고 있는 방향 * 200; 200미터까지 선 긋기
       
        if(Physics.Raycast(r,out hit,1000)) // 빛을 쏜게 1000미터 내의 거리에서 어딘가에 맞았다
        {
            hitPosition = hit.point;
            GameObject particle = Instantiate(particlePrefab); // 파티클 소환
            particle.transform.position = hitPosition; // 소환위치는 히트포지션
            particle.transform.forward = hit.normal; // 착탄점의 수직방향으로 분출

            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().Damage(damage); 
            }


        }

        if(TrailPrefab != null)
        {
            GameObject obj = Instantiate(TrailPrefab);
            Vector3[] pos = new Vector3[] { FiringPosition.position, hitPosition }; //쏜 곳과 맞은 곳 좌표 확보
            obj.GetComponent<LineRenderer>().SetPositions(pos); // LineRenderer에 SetPositions(pos) 좌표 보내서 선 그리기

            StartCoroutine(RemoveTrail(obj)); // RemoveTrail 을 코루틴으로 실행하겠다 // 기존 함수랑 별개로 독자적으로 실행
        }

        

        IEnumerator RemoveTrail(GameObject obj) // IEnumerator 코루틴 쓸려면 필요함
        {
            yield return new WaitForSeconds(0.3f); // .3초 기다려주세요

            Destroy(obj);
        }
    }
}
