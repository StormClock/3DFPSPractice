using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    public GameObject[] Weapons; // 무기 목록


    public float WalkingSpeed = 5;
    public float MouseSense = 10;
    public float JumpSpeed = 4;

    public Transform CameraTransform;

    float VerticalAngle;
    float HorizontalAngle;
    float VerticalSpeed = 0;

    bool IsGrounded;
    float GroundTimer;

    int currentWeapon;

    CharacterController characterController;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스커서 게임화면 안에 가두기
        Cursor.visible = false; // 마우스 커서 비표시

        VerticalAngle = 0;
        HorizontalAngle = transform.localEulerAngles.y;
        IsGrounded = true;
        GroundTimer = 0;

        characterController = GetComponent<CharacterController>();
        currentWeapon = 0;

        animator = GetComponent<Animator>();

        UpdateWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        // 코요테 타임 보정
        
        if (!characterController.isGrounded)
        {
            if (IsGrounded)
            {
                GroundTimer += Time.deltaTime;
                if (GroundTimer > 0.5f)
                {
                    IsGrounded = false;
                }
            }
        }
        else
        {
            IsGrounded = true;
            GroundTimer = 0;
        }

        // Debug.Log(IsGrounded);
        

        //평행이동
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move.magnitude > 1)
        {
            move.Normalize(); // 대각 이동이 더 빠른거 방지
        }

        move = move * WalkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move); // 캐릭터 시선 따라 이동방향 수정
        characterController.Move(move);

        //좌우 마우스
        float TurnPlayer = Input.GetAxis("Mouse X") * MouseSense;
        HorizontalAngle += TurnPlayer;
        if (HorizontalAngle > 360) HorizontalAngle -= 360;
        if (HorizontalAngle < 0) HorizontalAngle += 360;

        Vector3 currentAngle = transform.localEulerAngles;
        // 좌우로 돌아보는건 Y축 회전
        currentAngle.y = HorizontalAngle;
        transform.localEulerAngles = currentAngle;

        //상하 마우스

        float TurnCam = Input.GetAxis("Mouse Y") * MouseSense;
        VerticalAngle -= TurnCam;
        VerticalAngle = Mathf.Clamp(VerticalAngle, -89f, 89f); // Mathf.Clamp 첫번째 값을 두번째와 세번째 값 사이에 가둬둠
        currentAngle = CameraTransform.localEulerAngles;
        //위아래 보는건 X축 회전
        currentAngle.x = VerticalAngle;
        CameraTransform.localEulerAngles = currentAngle;

        //점프

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            VerticalSpeed = JumpSpeed;
            IsGrounded = false;
        }


        //낙하

        VerticalSpeed -= 9.8f * Time.deltaTime;
        if (VerticalSpeed < -10) // 낙하속도 무한한 증가 방지
        {
            VerticalSpeed = -10;
        }

        Vector3 VerticalMove = new Vector3(0, VerticalSpeed, 0);
        VerticalMove = VerticalMove * Time.deltaTime;
        CollisionFlags flag = characterController.Move(VerticalMove);

        //바닥과 접촉여부
        if ((flag & CollisionFlags.Below) != 0)
        {
            VerticalSpeed = 0;
        }

        //무기 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;

            /*
            currentWeapon++;
            if(currentWeapon >= Weapons.Length)
            {
                currentWeapon = 0;
            }
            */

            UpdateWeapon();

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Brother Moons are Awake");
            currentWeapon = 1;

            UpdateWeapon();
        }
    }

    void UpdateWeapon() // 선택한 무기 외에 다른 무기들 비활성화
    {
        foreach (GameObject w in Weapons)
        {
            w.SetActive(false);
        }

        Weapons[currentWeapon].SetActive(true);
    }

    public void Die()
    {
        Debug.Log("I'm Die. Thank you forever");
        animator.SetTrigger("Die");
        Invoke("ImDie", 1f);
    }

    void ImDie()
    {
        GameManager.instance.GameOverScene();
    }
}
