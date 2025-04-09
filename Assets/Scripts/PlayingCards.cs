using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCards : MonoBehaviour
{
    public GameObject selectedCard = null;
    public Transform[] slots;  // Slotlar (masadaki 5 slot)
    private string[] microTags = { "Micro_Predator", "Micro_Herbivore", };
    private string[] mesoTags = { "Meso_Predator", };
    private string[] alphaTags = { "Alpha_Predator", };
    private string[] apexTags = { "Apex_Predator", };

    void Update()
    {
        // Fare ile kart� se�mek
        if (Input.GetMouseButtonDown(0))  // Sol fare tu�una t�klama
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // E�er t�klanan �ey bir kart ise, kart� se�
                if (hit.collider.CompareTag(microTags[0]) || hit.collider.CompareTag(microTags[1]))
                {
                    selectedCard = hit.collider.gameObject;
                    Debug.Log("Kart se�ildi: " + selectedCard.name);
                    // Kart�n �zerine t�kland���nda yap�lacak i�lemler
                }
            }
        }
        // E�er se�ili kart varsa ve fare t�klan�rsa, kart� slotlara yerle�tir
        if (selectedCard != null && Input.GetMouseButtonDown(1))  // Sa� fare tu�una t�klama
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // E�er t�klanan �ey bir slot ise, kart� o slota yerle�tir
                if (hit.collider.CompareTag("Slot"))
                {
                    Transform slot = hit.collider.transform;

                    // Kart� smooth bir �ekilde slotta yerine yerle�tir
                    StartCoroutine(MoveCardToSlot(selectedCard, slot.position, slot));

                    selectedCard = null;  // Kart� b�rakt�k, se�im s�f�rlan�r
                    Debug.Log("Kart slota yerle�tirildi.");
                }
            }
        }
    }

    // Kart� smooth bir �ekilde slotlara ta��mak i�in Coroutine
    private IEnumerator MoveCardToSlot(GameObject card, Vector3 targetPosition, Transform slot)
    {
        float duration = 0.15f;  // Hareketin s�resi (saniye cinsinden)
        float timeElapsed = 0f;  // Zaman� takip etmek i�in

        Vector3 startingPosition = card.transform.position;  // Ba�lang�� pozisyonu

        // Kart�n hareketi
        while (timeElapsed < duration)
        {
            card.transform.position = Vector3.Lerp(startingPosition, targetPosition, timeElapsed / duration);  // Lerp ile yava��a hareket et
            timeElapsed += Time.deltaTime;  // Zaman� g�ncelle
            yield return null;  // Bir frame bekle
        }

        // Hareket tamamland���nda, son pozisyona ula�
        card.transform.position = targetPosition;

        // Kart�n parent'�n� slot olarak de�i�tir
        card.transform.SetParent(slot);

        // Kart�n yerel rotas�n� ayarla (slot'un rotation'�na g�re)
        card.transform.localRotation = Quaternion.Euler(0, 180, 180);
    }
}
