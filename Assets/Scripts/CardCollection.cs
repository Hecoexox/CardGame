using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    public List<CardData> allCards;         // Tüm kartlar burada (1-120)
    public List<CardData> unlockedCards;    // isUnlocked == true olanlar buraya eklenir
    public List<CardData> playerHand;       // Oyuncunun elindeki kartlar
    public List<CardData> currentDeck;

    public GameObject cardPrefab;
    public Transform[] handPositions;
    public Vector3 spawnPosition;

    // ID’si 1-20 olan kartlarý açýk hale getir (isUnlocked = true)
    public void UnlockInitialCards()
    {
        foreach (CardData card in allCards)
        {
            if (card.id >= 1 && card.id <= 20)
            {
                card.isUnlocked = true;
            }
        }
    }

    // isUnlocked == true olanlarý unlockedCards listesine aktar
    public void GenerateUnlockedCardsList()
    {
        foreach (CardData card in allCards)
        {
            if (card.isUnlocked)
            {
                unlockedCards.Add(card);
            }
        }
    }

    public void GenerateCurrentDeckCards()
    {
        currentDeck.Clear(); // Önce mevcut desteyi temizle

        List<CardData> tempList = new List<CardData>(unlockedCards);

        // unlockedCards destesinden rastgele 20 tane kart seç
        for (int i = 0; i < 20 && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            currentDeck.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); // Ayný kartýn tekrar seçilmesini engelle
        }
    }

    // Baþlangýçta oyuncunun eline 5 rastgele kart ver (artýk currentDeck'ten)
    public void DrawStartingHand()
    {
        playerHand.Clear(); // El daha önceden doluysa temizle

        for (int i = 0; i < 5 && currentDeck.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, currentDeck.Count);
            playerHand.Add(currentDeck[randomIndex]);
            currentDeck.RemoveAt(randomIndex); // Çekilen kartý desteden çýkar
        }
    }

    public void ShowHand()
    {
        StartCoroutine(SpawnHandCoroutine());
    }

    private IEnumerator SpawnHandCoroutine()
    {
        // Kartlarý tek tek spawn et
        for (int i = 0; i < playerHand.Count && i < handPositions.Length; i++)
        {
            // Kartý spawn position'da oluþtur
            GameObject cardGO = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);

            // Kartý hedef pozisyona hareket ettir
            StartCoroutine(MoveCardToPosition(cardGO, handPositions[i]));

            // Görsel setup
            CardDisplay display = cardGO.GetComponent<CardDisplay>();
            display.Setup(playerHand[i].artwork);

            // Biraz bekle, sonra sýradakine geç
            yield return new WaitForSeconds(0.1f); // Ýstersen ayarlanabilir yaparýz
        }
    }

    private IEnumerator MoveCardToPosition(GameObject card, Transform target)
    {
        float duration = 0.25f; // Ne kadar sürede gitsin
        float elapsed = 0f;

        Vector3 startPos = card.transform.position;
        Quaternion startRot = card.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            card.transform.position = Vector3.Lerp(startPos, target.position, t);
            card.transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);
            yield return null;
        }

        // Hedef pozisyona tam olarak yerleþtir
        card.transform.SetParent(target);
        card.transform.position = target.position;
        card.transform.rotation = target.rotation;
    }
}