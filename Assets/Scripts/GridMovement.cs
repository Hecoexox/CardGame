using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float tileSize = 1f;
    public float moveSpeed = 10f;
    public float rotationSpeed = 300f;
    public Vector2Int gridSize = new Vector2Int(4, 3);

    public bool onGrid = true;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isRotating = false;

    public Vector2Int currentTilePosition;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public Vector3 sitDownCameraOffset = new Vector3(0, 1f, 0.5f);
    public Vector3 sitDownCameraRotation = new Vector3(10, 0, 0);

    private Vector3 defaultCameraPosition;
    private Quaternion defaultCameraRotation;

    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        currentTilePosition = GetTilePosition(transform.position);

        // Oyunun baþýnda varsayýlan kamera pozisyonunu kaydet
        defaultCameraPosition = cameraTransform.localPosition;
        defaultCameraRotation = cameraTransform.localRotation;
    }

    void Update()
    {
        HandleInput();
        SmoothMove();
        SmoothRotate();
        currentTilePosition = GetTilePosition(transform.position);
    }

    void HandleInput()
    {
        if (!isRotating)
        {
            // Masada deðilken hareket ve dönüþleri kontrol et
            if (onGrid)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    MoveForward();
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    MoveBackward();
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    RotateLeft();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    RotateRight();
                }
            }
        }

        // Masada iken sadece S tuþu ile kalkma iþlemi
        if (Input.GetKeyDown(KeyCode.S) && !onGrid)
        {
            StandUp();
        }
    }

    public void MoveForward()
    {
        Vector3 nextPos = targetPosition + transform.forward * tileSize;
        Vector2Int nextTilePos = GetTilePosition(nextPos);

        if (currentTilePosition == new Vector2Int(2, 2) && transform.forward == Vector3.right)
        {
            SitDown();
            return;
        }

        if (IsInsideGrid(nextPos))
        {
            targetPosition = nextPos;
        }
    }

    void MoveBackward()
    {
        Vector3 nextPos = targetPosition - transform.forward * tileSize;
        if (IsInsideGrid(nextPos))
        {
            targetPosition = nextPos;
        }
    }

    void RotateLeft()
    {
        targetRotation *= Quaternion.Euler(0, -90, 0);
        isRotating = true;
    }

    void RotateRight()
    {
        targetRotation *= Quaternion.Euler(0, 90, 0);
        isRotating = true;
    }

    void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    void SmoothRotate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            isRotating = false;
        }
    }

    bool IsInsideGrid(Vector3 newPos)
    {
        int newX = Mathf.RoundToInt(newPos.x / tileSize);
        int newY = Mathf.RoundToInt(newPos.z / tileSize);

        if (newX == 3 && newY == 2)
        {
            return false;
        }

        return (newX >= 0 && newX < gridSize.x) && (newY >= 0 && newY < gridSize.y);
    }

    Vector2Int GetTilePosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tileSize);
        int y = Mathf.RoundToInt(worldPosition.z / tileSize);
        return new Vector2Int(x, y);
    }

    void SitDown()
    {
        Debug.Log("Oturuyor...");
        onGrid = false;

        // Kamera pozisyonunu deðiþtirme
        StartCoroutine(MoveCamera(sitDownCameraOffset, Quaternion.Euler(sitDownCameraRotation)));
    }

    void StandUp()
    {
        Debug.Log("Kalkýyor...");
        onGrid = true;

        // Kamera eski yerine dönsün
        StartCoroutine(MoveCamera(defaultCameraPosition, defaultCameraRotation));
    }

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPos = cameraTransform.localPosition;
        Quaternion startRot = cameraTransform.localRotation;

        while (elapsedTime < duration)
        {
            cameraTransform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            cameraTransform.localRotation = Quaternion.Lerp(startRot, targetRot, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = targetPos;
        cameraTransform.localRotation = targetRot;
    }
}
