using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskPuzzle : MonoBehaviour
{
    public GameObject Disk1;
    public GameObject Disk2;
    public GameObject Disk3;
    public GameObject PuzzleCompleteObject; // Puzzle tamamlandýðýnda hareket ettirilecek obje

    // Her diskin rotasýný tutacak deðiþkenler
    private int completedRotationDisk1 = 0;
    private int completedRotationDisk2 = 0;
    private int completedRotationDisk3 = 0;

    private bool puzzleCompleted = false; // Puzzle'ýn tamamlanýp tamamlanmadýðýný takip eder
    private Vector3 targetPosition; // Hedef pozisyon (Z+ 2.5 birim)
    private float moveSpeed = 2.5f; // Hareket hýzýný kontrol etmek için bir hýz deðiþkeni
    private bool isMoving = false; // Obje hareket ediyor mu?

    void Start()
    {
        // Diskleri 45 derece artýþlarla rastgele baþlat
        completedRotationDisk1 = Random.Range(0, 8) * 45;
        completedRotationDisk2 = Random.Range(0, 8) * 45;
        completedRotationDisk3 = Random.Range(0, 8) * 45;

        // Disklere baþta rastgele rotalarý ver
        Disk1.transform.Rotate(0, completedRotationDisk1, 0);
        Disk2.transform.Rotate(0, completedRotationDisk2, 0);
        Disk3.transform.Rotate(0, completedRotationDisk3, 0);

        // Puzzle tamamlandýðýnda hedef pozisyonu belirle
        if (PuzzleCompleteObject != null)
        {
            targetPosition = PuzzleCompleteObject.transform.localPosition + new Vector3(0, 0, 1.2f);
        }
    }

    void Update()
    {
        if (!puzzleCompleted)
        {
            // Mouse'a týklama kontrolü
            if (Input.GetMouseButtonDown(0)) // 0, sol týklama butonudur
            {
                // Týklanan objeyi bulmak için raycast atýyoruz
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Raycast ile týklanan obje kontrolü
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

            // Eðer tüm disklerin rotasý 0 ise puzzle tamamlandý
            if (completedRotationDisk1 % 360 == 0 && completedRotationDisk2 % 360 == 0 && completedRotationDisk3 % 360 == 0)
            {
                Debug.Log("Baþardýn");

                // Puzzle tamamlandýðýnda disklere týklamayý engelle
                puzzleCompleted = true;

                // Puzzle tamamlandýðýnda objeyi hareket ettirmeye baþla
                isMoving = true;

                // Diskleri devre dýþý býrak
                Disk1.GetComponent<Collider>().enabled = false;
                Disk2.GetComponent<Collider>().enabled = false;
                Disk3.GetComponent<Collider>().enabled = false;
            }
        }

        // Objeyi smooth þekilde hareket ettir
        if (isMoving && PuzzleCompleteObject != null)
        {
            PuzzleCompleteObject.transform.localPosition = Vector3.Lerp(PuzzleCompleteObject.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Objeyi hedef pozisyona yakýnlaþtýr
            if (Vector3.Distance(PuzzleCompleteObject.transform.localPosition, targetPosition) < 0.01f)
            {
                PuzzleCompleteObject.transform.localPosition = targetPosition;
                isMoving = false; // Hareket durdurulmuþ olur
            }
        }
    }
}
