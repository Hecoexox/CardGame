using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public GameObject target; // Inspector'dan atayacaðýn hedef GameObject
    public float idleMovementAmount = 1f; // Idle hareketinin ne kadar güçlü olacaðýný belirler
    public float idleSpeed = 0.5f; // Idle hareketinin hýzýný belirler
    public float breathingAmount = 0.05f; // Nefes alma hareketinin ne kadar olacaðýný belirler
    public float breathingSpeed = 1f; // Nefes alma hýzýný belirler
    public GameObject childObject; // Çocuðu burada belirleyeceðiz
    public float toggleInterval = 3f; // Çocuðun aktiflik durumunun deðiþme aralýðý
    public float openDuration = 0.5f; // Çocuðun açýlma süresi

    private float randomIdleRotation;
    private float initialYPosition;

    void Start()
    {
        // Baþlangýçta çocuðu açýk yap
        if (childObject != null)
        {
            childObject.SetActive(true);
        }

        // Baþlangýçta rastgele bir idle hareket açýsý oluþtur
        randomIdleRotation = Random.Range(-idleMovementAmount, idleMovementAmount);
        initialYPosition = transform.position.y; // Baþlangýçtaki Y pozisyonunu kaydet

        // Coroutine'i baþlat
        StartCoroutine(ToggleChildActiveCoroutine());
    }

    void Update()
    {
        // Eðer hedef atanmýþsa
        if (target != null)
        {
            // Bu GameObject'i hedefe doðru döndür
            Vector3 direction = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Y eksenine 90 derece ekle
            rotation *= Quaternion.Euler(0, 90, 0);

            // Hafif idle hareketi ekle
            float idleRotation = Mathf.Sin(Time.time * idleSpeed) * randomIdleRotation;

            // Objeye idle hareketi ekle
            transform.rotation = rotation * Quaternion.Euler(0, idleRotation, 0);

            // Nefes alma hareketi (Y ekseninde yukarý/aþaðý hareket)
            float breathingMovement = Mathf.Sin(Time.time * breathingSpeed) * breathingAmount;

            // Objeye pozisyon ekle (nefes alýyormuþ gibi hareket)
            transform.position = new Vector3(transform.position.x, initialYPosition + breathingMovement, transform.position.z);
        }
    }

    // Çocuðu aktif/pasif yapma coroutine
    private IEnumerator ToggleChildActiveCoroutine()
    {
        while (true)
        {
            // 3 saniye bekle (objeyi gizlemek için)
            yield return new WaitForSeconds(toggleInterval);

            // Çocuðu gizle
            if (childObject != null)
            {
                childObject.SetActive(false);
            }

            // 0.5 saniye bekle (objeyi tekrar açmadan önce)
            yield return new WaitForSeconds(openDuration);

            // Çocuðu geri aç
            if (childObject != null)
            {
                childObject.SetActive(true);
            }
        }
    }
}
