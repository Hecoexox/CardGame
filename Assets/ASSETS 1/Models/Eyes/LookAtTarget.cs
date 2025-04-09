using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public GameObject target; // Inspector'dan atayaca��n hedef GameObject
    public float idleMovementAmount = 1f; // Idle hareketinin ne kadar g��l� olaca��n� belirler
    public float idleSpeed = 0.5f; // Idle hareketinin h�z�n� belirler
    public float breathingAmount = 0.05f; // Nefes alma hareketinin ne kadar olaca��n� belirler
    public float breathingSpeed = 1f; // Nefes alma h�z�n� belirler
    public GameObject childObject; // �ocu�u burada belirleyece�iz
    public float toggleInterval = 3f; // �ocu�un aktiflik durumunun de�i�me aral���
    public float openDuration = 0.5f; // �ocu�un a��lma s�resi

    private float randomIdleRotation;
    private float initialYPosition;

    void Start()
    {
        // Ba�lang��ta �ocu�u a��k yap
        if (childObject != null)
        {
            childObject.SetActive(true);
        }

        // Ba�lang��ta rastgele bir idle hareket a��s� olu�tur
        randomIdleRotation = Random.Range(-idleMovementAmount, idleMovementAmount);
        initialYPosition = transform.position.y; // Ba�lang��taki Y pozisyonunu kaydet

        // Coroutine'i ba�lat
        StartCoroutine(ToggleChildActiveCoroutine());
    }

    void Update()
    {
        // E�er hedef atanm��sa
        if (target != null)
        {
            // Bu GameObject'i hedefe do�ru d�nd�r
            Vector3 direction = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Y eksenine 90 derece ekle
            rotation *= Quaternion.Euler(0, 90, 0);

            // Hafif idle hareketi ekle
            float idleRotation = Mathf.Sin(Time.time * idleSpeed) * randomIdleRotation;

            // Objeye idle hareketi ekle
            transform.rotation = rotation * Quaternion.Euler(0, idleRotation, 0);

            // Nefes alma hareketi (Y ekseninde yukar�/a�a�� hareket)
            float breathingMovement = Mathf.Sin(Time.time * breathingSpeed) * breathingAmount;

            // Objeye pozisyon ekle (nefes al�yormu� gibi hareket)
            transform.position = new Vector3(transform.position.x, initialYPosition + breathingMovement, transform.position.z);
        }
    }

    // �ocu�u aktif/pasif yapma coroutine
    private IEnumerator ToggleChildActiveCoroutine()
    {
        while (true)
        {
            // 3 saniye bekle (objeyi gizlemek i�in)
            yield return new WaitForSeconds(toggleInterval);

            // �ocu�u gizle
            if (childObject != null)
            {
                childObject.SetActive(false);
            }

            // 0.5 saniye bekle (objeyi tekrar a�madan �nce)
            yield return new WaitForSeconds(openDuration);

            // �ocu�u geri a�
            if (childObject != null)
            {
                childObject.SetActive(true);
            }
        }
    }
}
