using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private int count;
    private float applySpeed;

    public static bool GameIsPaused = false;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public GameObject pickUpParent;

    public GameObject panel;

    public Transform cameraTransform;
    // Transform���� ī�޶� �����ӿ� ���� �޶����Ƿ�,�ش� ���� ī�޶� �Ѱ��ֱ� ����
    // CameraTransform ���� ����

    public CharacterController characterController;
    // CharacterController�� 3D ������Ʈ�� �����ϱ� ���� characterController ���� ����

    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    // �̵� �ӵ�
    public float jumpSpeed = 10f;
    // ���� �ӵ�
    public float gravity = -20f;
    // �߷�
    public float yVelocity = 0;
    // Y�� ������

    private void Start()
    {
        count = 0;
        applySpeed = moveSpeed;

        winTextObject.SetActive(false);
        pickUpParent.SetActive(false);
        panel.SetActive(false);

        SetCountText();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        // h ������ Ű������ ���ΰ� (��, ��) �� �о�� ����� �ѱ��.
        // ��, ��, A, D Ű

        float v = Input.GetAxis("Vertical");
        // v ������ Ű������ ���ΰ� (��, ��) �� �о�� ����� �ѱ��.
        // ��, ��, W, S Ű

        Vector3 moveDirection = new Vector3(h, 0, v);
        // (x��, y��, z�� = h ����, 0, v ����) ���� �о�� ���� Vector3���� ����
        // �ش� ���� Vector3 ������ moveDirection ������ �ѱ��.

        moveDirection = cameraTransform.TransformDirection(moveDirection);
        // moveDirection ���� ī�޶� ��ġ

        moveDirection *= applySpeed;
        // �������� moveDirection ���� moveDirection * moveSpeed ���� ���� ���� ��.

        if (characterController.isGrounded)
        // ����, characterController�� ���� �پ��ִٸ�
        {
            yVelocity = 0;
            // y�� ������ ���� 0�̰�,
            if (Input.GetKeyDown(KeyCode.Space))
            // �����̽� �� Ű�� ���� ������ �ǽ��ϰ�,
            {
                yVelocity = jumpSpeed;
                // ����ڰ� ������ jumpSpeed ���� yVelocity ������ �Ѱܼ� ó���Ѵ�.
            }
        }

        yVelocity += (gravity * Time.deltaTime);
        // yVelocity ���� yVelocity + (�߷°� * Time.deltaTime)

        moveDirection.y = yVelocity;
        // ����� yVelocity ���� moveDirection.y (Y�� ������ ����) �� �Ѱ��ش�.

        characterController.Move(moveDirection * Time.deltaTime);
        // ���������� characterController�� �������� ���� * Time.deltaTime ��

        if (Input.GetKey(KeyCode.R))
        {
            if(!pickUpParent.activeSelf) {
                count = count + 1;
            }
            SetCountText();
            pickUpParent.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            pickUpParent.SetActive(false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            applySpeed = runSpeed;
        }

        // Shift Ű���� ���� ���� ���� ���ǹ�
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            applySpeed = moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (GameIsPaused)
            {
                panel.SetActive(false);
                Time.timeScale = 1f;
                GameIsPaused = false;
                //EditorApplication.isPaused = false;
            }
            else
            {
                panel.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;
                //EditorApplication.isPaused = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            other.gameObject.SetActive(false);

            winTextObject.SetActive(true);
        }
    }

    void SetCountText()
    {
        countText.text = "How many hints: " + count.ToString();
    }
}
