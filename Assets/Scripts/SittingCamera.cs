using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingCamera : MonoBehaviour
{
    public Transform cameraForward;
    public Transform cameraBack;
    public Transform cameraLeft;
    public Transform cameraRight;
    public Transform cameraCenter; // Orijinal kamera pozisyonu (baþlangýç pozisyonu)

    public GridMovement gridMovement;

    private Transform currentCameraPosition; // Geçerli kamera pozisyonu
    private bool isTransitioning = false; // Geçiþ yapýlýrken baþka tuþlara basýlmamasý için flag

    void Start()
    {
        // cameraCenter'in atandýðýndan emin ol
        if (cameraCenter == null)
        {
            Debug.LogError("Camera Center pozisyonu atanmadý!");
            return;
        }

        // Baþlangýçta kamerayý orijinal pozisyona yerleþtir
        currentCameraPosition = cameraCenter;
        transform.position = currentCameraPosition.position;
        transform.rotation = currentCameraPosition.rotation;
    }

    void Update()
    {
        // Eðer canStandUp true ise, kameranýn hareketini devre dýþý býrak
        if (gridMovement.canStandUp || isTransitioning)
        {
            return;
        }

        if (currentCameraPosition == cameraCenter)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                LookForward();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                LookBack();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                LookLeft();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                LookRight();
            }
        }
        else if (currentCameraPosition == cameraForward)
        {
            if (Input.GetKeyDown(KeyCode.S)) // Yalnýzca S tuþu ile Center'a dön
            {
                ReturnToCenter();
            }
        }
        else if (currentCameraPosition == cameraBack)
        {
            if (Input.GetKeyDown(KeyCode.W)) // Yalnýzca W tuþu ile Center'a dön
            {
                ReturnToCenter();
            }
        }
        else if (currentCameraPosition == cameraLeft)
        {
            if (Input.GetKeyDown(KeyCode.D)) // Yalnýzca D tuþu ile Center'a dön
            {
                ReturnToCenter();
            }
        }
        else if (currentCameraPosition == cameraRight)
        {
            if (Input.GetKeyDown(KeyCode.A)) // Yalnýzca A tuþu ile Center'a dön
            {
                ReturnToCenter();
            }
        }
    }

    public void LookForward()
    {
        if (currentCameraPosition != cameraForward)
        {
            currentCameraPosition = cameraForward;
            StartCoroutine(MoveCameraToPosition(cameraForward));
        }
    }

    public void LookBack()
    {
        if (currentCameraPosition != cameraBack)
        {
            currentCameraPosition = cameraBack;
            StartCoroutine(MoveCameraToPosition(cameraBack));
        }
    }

    public void LookLeft()
    {
        if (currentCameraPosition != cameraLeft)
        {
            currentCameraPosition = cameraLeft;
            StartCoroutine(MoveCameraToPosition(cameraLeft));
        }
    }

    public void LookRight()
    {
        if (currentCameraPosition != cameraRight)
        {
            currentCameraPosition = cameraRight;
            StartCoroutine(MoveCameraToPosition(cameraRight));
        }
    }

    private void ReturnToCenter()
    {
        // Geçiþi engelle, baþka tuþlara basýlmasýn
        isTransitioning = true;
        currentCameraPosition = cameraCenter;
        StartCoroutine(MoveCameraToPosition(cameraCenter));
    }

    private IEnumerator MoveCameraToPosition(Transform targetPosition)
    {
        float duration = 0.3f; // Hareket süresi
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, targetPosition.position, t);
            transform.rotation = Quaternion.Lerp(startRot, targetPosition.rotation, t);

            yield return null;
        }

        transform.position = targetPosition.position;
        transform.rotation = targetPosition.rotation;

        // Geçiþ tamamlandýðýnda, tuþlara tekrar basýlabilir
        isTransitioning = false;
    }
}
