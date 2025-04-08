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
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    cardCollection.ShowHand();
                    gameStarted = true;
                }
            }
        }
    }
}
