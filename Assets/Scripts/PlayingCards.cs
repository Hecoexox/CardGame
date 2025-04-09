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
        // Fare ile kartý seçmek
        if (Input.GetMouseButtonDown(0))  // Sol fare tuþuna týklama
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Eðer týklanan þey bir kart ise, kartý seç
                if (hit.collider.CompareTag(microTags[0]) || hit.collider.CompareTag(microTags[1]))
                {
                    selectedCard = hit.collider.gameObject;
                    Debug.Log("Kart seçildi: " + selectedCard.name);
                    // Kartýn üzerine týklandýðýnda yapýlacak iþlemler
                }
            }
        }
        // Eðer seçili kart varsa ve fare týklanýrsa, kartý slotlara yerleþtir
        if (selectedCard != null && Input.GetMouseButtonDown(1))  // Sað fare tuþuna týklama
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Eðer týklanan þey bir slot ise, kartý o slota yerleþtir
                if (hit.collider.CompareTag("Slot"))
                {
                    Transform slot = hit.collider.transform;

                    // Kartý smooth bir þekilde slotta yerine yerleþtir
                    StartCoroutine(MoveCardToSlot(selectedCard, slot.position, slot));

                    selectedCard = null;  // Kartý býraktýk, seçim sýfýrlanýr
                    Debug.Log("Kart slota yerleþtirildi.");
                }
            }
        }
    }

    // Kartý smooth bir þekilde slotlara taþýmak için Coroutine
    private IEnumerator MoveCardToSlot(GameObject card, Vector3 targetPosition, Transform slot)
    {
        float duration = 0.15f;  // Hareketin süresi (saniye cinsinden)
        float timeElapsed = 0f;  // Zamaný takip etmek için

        Vector3 startingPosition = card.transform.position;  // Baþlangýç pozisyonu

        // Kartýn hareketi
        while (timeElapsed < duration)
        {
            card.transform.position = Vector3.Lerp(startingPosition, targetPosition, timeElapsed / duration);  // Lerp ile yavaþça hareket et
            timeElapsed += Time.deltaTime;  // Zamaný güncelle
            yield return null;  // Bir frame bekle
        }

        // Hareket tamamlandýðýnda, son pozisyona ulaþ
        card.transform.position = targetPosition;

        // Kartýn parent'ýný slot olarak deðiþtir
        card.transform.SetParent(slot);

        // Kartýn yerel rotasýný ayarla (slot'un rotation'ýna göre)
        card.transform.localRotation = Quaternion.Euler(0, 180, 180);
    }
}
