using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskPuzzle : MonoBehaviour
{
    public GameObject Disk1;
    public GameObject Disk2;
    public GameObject Disk3;
    public GameObject PuzzleCompleteObject; // Puzzle tamamland���nda hareket ettirilecek obje

    // Her diskin rotas�n� tutacak de�i�kenler
    private int completedRotationDisk1 = 0;
    private int completedRotationDisk2 = 0;
    private int completedRotationDisk3 = 0;

    private bool puzzleCompleted = false; // Puzzle'�n tamamlan�p tamamlanmad���n� takip eder
    private Vector3 targetPosition; // Hedef pozisyon (Z+ 2.5 birim)
    private float moveSpeed = 2.5f; // Hareket h�z�n� kontrol etmek i�in bir h�z de�i�keni
    private bool isMoving = false; // Obje hareket ediyor mu?

    void Start()
    {
        // Diskleri 45 derece art��larla rastgele ba�lat
        completedRotationDisk1 = Random.Range(0, 8) * 45;
        completedRotationDisk2 = Random.Range(0, 8) * 45;
        completedRotationDisk3 = Random.Range(0, 8) * 45;

        // Disklere ba�ta rastgele rotalar� ver
        Disk1.transform.Rotate(0, completedRotationDisk1, 0);
        Disk2.transform.Rotate(0, completedRotationDisk2, 0);
        Disk3.transform.Rotate(0, completedRotationDisk3, 0);

        // Puzzle tamamland���nda hedef pozisyonu belirle
        if (PuzzleCompleteObject != null)
        {
            targetPosition = PuzzleCompleteObject.transform.localPosition + new Vector3(0, 0, 1.2f);
        }
    }

    void Update()
    {
        if (!puzzleCompleted)
        {
            // Mouse'a t�klama kontrol�
            if (Input.GetMouseButtonDown(0)) // 0, sol t�klama butonudur
            {
                // T�klanan objeyi bulmak i�in raycast at�yoruz
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Raycast ile t�klanan obje kontrol�
                    if (hit.collider.gameObject == Disk1)
                    {
                        completedRotationDisk1 += 45;
                        Disk1.transform.Rotate(0, 45, 0);
                    }
                    else if (hit.collider.gameObject == Disk2)
                    {
                        completedRotationDisk2 += 45;
                        Disk2.transform.Rotate(0, 45, 0);
                    }
                    else if (hit.collider.gameObject == Disk3)
                    {
                        completedRotationDisk3 += 45;
                        Disk3.transform.Rotate(0, 45, 0);
                    }
                }
            }

            // E�er t�m disklerin rotas� 0 ise puzzle tamamland�
            if (completedRotationDisk1 % 360 == 0 && completedRotationDisk2 % 360 == 0 && completedRotationDisk3 % 360 == 0)
            {
                Debug.Log("Ba�ard�n");

                // Puzzle tamamland���nda disklere t�klamay� engelle
                puzzleCompleted = true;

                // Puzzle tamamland���nda objeyi hareket ettirmeye ba�la
                isMoving = true;

                // Diskleri devre d��� b�rak
                Disk1.GetComponent<Collider>().enabled = false;
                Disk2.GetComponent<Collider>().enabled = false;
                Disk3.GetComponent<Collider>().enabled = false;
            }
        }

        // Objeyi smooth �ekilde hareket ettir
        if (isMoving && PuzzleCompleteObject != null)
        {
            PuzzleCompleteObject.transform.localPosition = Vector3.Lerp(PuzzleCompleteObject.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Objeyi hedef pozisyona yak�nla�t�r
            if (Vector3.Distance(PuzzleCompleteObject.transform.localPosition, targetPosition) < 0.01f)
            {
                PuzzleCompleteObject.transform.localPosition = targetPosition;
                isMoving = false; // Hareket durdurulmu� olur
            }
        }
    }
}
