using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardCollection cardCollection;

    public bool gameStarted = false;

    void Start()
    {
        cardCollection.UnlockInitialCards();
        cardCollection.GenerateUnlockedCardsList();
        cardCollection.GenerateCurrentDeckCards();
        cardCollection.DrawStartingHand();
    }

    void Update()
    {
        // Deck'e yalnýzca bir kez týklanabilsin
        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    cardCollection.ShowHand();
                    gameStarted = true; // Deck týklandýðýnda bir daha týklanamayacak
                }
            }
        }
    }
}