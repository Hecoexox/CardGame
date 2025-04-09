using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    public List<CardData> allCards;         // T�m kartlar burada (1-120)
    public List<CardData> unlockedCards;    // isUnlocked == true olanlar buraya eklenir
    public List<CardData> playerHand;       // Oyuncunun elindeki kartlar
    public List<CardData> currentDeck;

    public GameObject cardPrefab;
    public Transform[] handPositions;
    public Vector3 spawnPosition;

    // ID�si 1-20 olan kartlar� a��k hale getir (isUnlocked = true)
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

    // isUnlocked == true olanlar� unlockedCards listesine aktar
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
        currentDeck.Clear(); // �nce mevcut desteyi temizle

        List<CardData> tempList = new List<CardData>(unlockedCards);

        // unlockedCards destesinden rastgele 20 tane kart se�
        for (int i = 0; i < 20 && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            currentDeck.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); // Ayn� kart�n tekrar se�ilmesini engelle
        }
    }

    // Ba�lang��ta oyuncunun eline 5 rastgele kart ver (art�k currentDeck'ten)
    public void DrawStartingHand()
    {
        playerHand.Clear(); // El daha �nceden doluysa temizle

        for (int i = 0; i < 5 && currentDeck.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, currentDeck.Count);
            playerHand.Add(currentDeck[randomIndex]);
            currentDeck.RemoveAt(randomIndex); // �ekilen kart� desteden ��kar
        }
    }

    public void ShowHand()
    {
        StartCoroutine(SpawnHandCoroutine());
    }

    private IEnumerator SpawnHandCoroutine()
    {
        for (int i = 0; i < playerHand.Count && i < handPositions.Length; i++)
        {
            CardData cardData = playerHand[i];

            GameObject cardGO = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);

            // Spawn edilen objeye cardName ver
            cardGO.name = cardData.cardName;

            // Tag olarak CardLevel enum�undaki string de�er atan�yor
            string levelTag = cardData.level.ToString();
            if (IsValidTag(levelTag))
            {
                cardGO.tag = levelTag;
            }
            else
            {
                Debug.LogWarning($"'{levelTag}' tag olarak tan�ml� de�il. Unity edit�rden Tags k�sm�na eklemelisin.");
            }

            // Kart� hedef pozisyona ta��
            StartCoroutine(MoveCardToPosition(cardGO, handPositions[i]));

            // G�rsel setup
            CardDisplay display = cardGO.GetComponent<CardDisplay>();
            display.Setup(cardData.artwork);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator MoveCardToPosition(GameObject card, Transform target)
    {
        float duration = 0.25f; // Ne kadar s�rede gitsin
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

        // Hedef pozisyona tam olarak yerle�tir
        card.transform.SetParent(target);
        card.transform.position = target.position;
        card.transform.rotation = target.rotation;
    }

    private bool IsValidTag(string tag)
    {
        // Unity sadece edit�rde �al��t�rabilir bu k�sm�
#if UNITY_EDITOR
    return UnityEditorInternal.InternalEditorUtility.tags.Contains(tag);
#else
        return true; // Build ortam�nda tag kontrol� yap�lamaz, o y�zden true d�neriz
#endif
    }
}