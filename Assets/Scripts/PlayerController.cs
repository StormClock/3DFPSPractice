using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    public GameObject[] Weapons; // ���� ���


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
        Cursor.lockState = CursorLockMode.Locked; // ���콺Ŀ�� ����ȭ�� �ȿ� ���α�
        Cursor.visible = false; // ���콺 Ŀ�� ��ǥ��

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
        // �ڿ��� Ÿ�� ����
        
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
        

        //�����̵�
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move.magnitude > 1)
        {
            move.Normalize(); // �밢 �̵��� �� ������ ����
        }

        move = move * WalkingSpeed * Time.deltaTime;
        move = transform.TransformDirection(move); // ĳ���� �ü� ���� �̵����� ����
        characterController.Move(move);

        //�¿� ���콺
        float TurnPlayer = Input.GetAxis("Mouse X") * MouseSense;
        HorizontalAngle += TurnPlayer;
        if (HorizontalAngle > 360) HorizontalAngle -= 360;
        if (HorizontalAngle < 0) HorizontalAngle += 360;

        Vector3 currentAngle = transform.localEulerAngles;
        // �¿�� ���ƺ��°� Y�� ȸ��
        currentAngle.y = HorizontalAngle;
        transform.localEulerAngles = currentAngle;

        //���� ���콺

        float TurnCam = Input.GetAxis("Mouse Y") * MouseSense;
        VerticalAngle -= TurnCam;
        VerticalAngle = Mathf.Clamp(VerticalAngle, -89f, 89f); // Mathf.Clamp ù��° ���� �ι�°�� ����° �� ���̿� ���ֵ�
        currentAngle = CameraTransform.localEulerAngles;
        //���Ʒ� ���°� X�� ȸ��
        currentAngle.x = VerticalAngle;
        CameraTransform.localEulerAngles = currentAngle;

        //����

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            VerticalSpeed = JumpSpeed;
            IsGrounded = false;
        }


        //����

        VerticalSpeed -= 9.8f * Time.deltaTime;
        if (VerticalSpeed < -10) // ���ϼӵ� ������ ���� ����
        {
            VerticalSpeed = -10;
        }

        Vector3 VerticalMove = new Vector3(0, VerticalSpeed, 0);
        VerticalMove = VerticalMove * Time.deltaTime;
        CollisionFlags flag = characterController.Move(VerticalMove);

        //�ٴڰ� ���˿���
        if ((flag & CollisionFlags.Below) != 0)
        {
            VerticalSpeed = 0;
        }

        //���� ����
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

    void UpdateWeapon() // ������ ���� �ܿ� �ٸ� ����� ��Ȱ��ȭ
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
