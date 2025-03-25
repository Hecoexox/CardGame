using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    public GameObject drawer;
    public GameObject table;
    private bool drawerOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    public float moveSpeed = 5f;

    void Start()
    {
        closedPosition = drawer.transform.position; // Çekmecenin başlangıç pozisyonu
        openPosition = closedPosition - new Vector3(0.1866f, 0, 0); // Açık pozisyon
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tık algılama
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Mouse'tan bir ışın gönder
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Eğer bir objeye çarptıysa
            {
                if (hit.transform.gameObject == drawer) // Eğer çekmeceye çarptıysa
                {
                    Debug.Log("Çekmeceye tıkladın!");
                    StopAllCoroutines(); // Önceki hareketleri durdur
                    StartCoroutine(MoveDrawer(drawerOpen ? closedPosition : openPosition)); // Hedef pozisyona hareket et
                    drawerOpen = !drawerOpen; // Durumu tersine çevir
                }
            }
        }
    }

    IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        while (Vector3.Distance(drawer.transform.position, targetPosition) > 0.001f)
        {
            drawer.transform.position = Vector3.Lerp(drawer.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        drawer.transform.position = targetPosition; // Pozisyonu tam olarak hedefe oturt
    }
}