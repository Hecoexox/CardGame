using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingCamera : MonoBehaviour
{
    public Transform cameraForward;
    public Transform cameraBack;
    public Transform cameraLeft;
    public Transform cameraRight;
    public Transform cameraCenter; // Orijinal kamera pozisyonu (ba�lang�� pozisyonu)

    public GridMovement gridMovement;

    private Transform currentCameraPosition; // Ge�erli kamera pozisyonu
    private bool isTransitioning = false; // Ge�i� yap�l�rken ba�ka tu�lara bas�lmamas� i�in flag

    void Start()
    {
        // cameraCenter'in atand���ndan emin ol
        if (cameraCenter == null)
        {
            Debug.LogError("Camera Center pozisyonu atanmad�!");
            return;
        }

        // Ba�lang��ta kameray� orijinal pozisyona yerle�tir
        currentCameraPosition = cameraCenter;
        transform.position = currentCameraPosition.position;
        transform.rotation = currentCameraPosition.rotation;
    }

    void Update()
    {
        // E�er canStandUp true ise, kameran�n hareketini devre d��� b�rak
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
            if (Input.GetKeyDown(KeyCode.S)) // Yaln�zca S tu�u ile Center'a d�n
            {
                ReturnToCenter();
            }
        }
        else if (currentCameraPosition == cameraBack)
        {
            if (Input.GetKeyDown(KeyCode.W)) // Yaln�zca W tu�u ile Center'a d�n
            {
                ReturnToCenter();
            }
        }
        else if (currentCameraPosition == cameraLeft)
        {
            if (Input.GetKeyDown(KeyCode.D)) // Yaln�zca D tu�u ile Center'a d�n
            {
                ReturnToCenter();
            }
        }
        else if (currentCameraPosition == cameraRight)
        {
            if (Input.GetKeyDown(KeyCode.A)) // Yaln�zca A tu�u ile Center'a d�n
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
        // Ge�i�i engelle, ba�ka tu�lara bas�lmas�n
        isTransitioning = true;
        currentCameraPosition = cameraCenter;
        StartCoroutine(MoveCameraToPosition(cameraCenter));
    }

    private IEnumerator MoveCameraToPosition(Transform targetPosition)
    {
        float duration = 0.3f; // Hareket s�resi
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

        // Ge�i� tamamland���nda, tu�lara tekrar bas�labilir
        isTransitioning = false;
    }
}
