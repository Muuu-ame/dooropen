using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private GameObject[] doorPrefabs;
    [SerializeField] private int maxDoors = 10;
    [SerializeField] private Vector3 doorSpacing = new Vector3(0, 0, 5f);

    private int doorCount = 0;
    private int lastIndex = -1;
    private List<GameObject> activeDoors = new List<GameObject>();

    [Header("外部参照")]
    public MonoBehaviour timerScript;   // ⏱ 止めたいタイマー
    public GameObject clearButton;      // 🟩 クリア後に出したいボタン

    // reactとの接合部分の関数
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void NextButton();
#endif



    void Start()
    {
        // 🔹 ゲーム開始時はボタン非表示
        if (clearButton != null)
            clearButton.SetActive(false);

        // 🔹 最初のドア生成
        for (int i = 0; i < 3 && doorCount < maxDoors; i++)
        {
            SpawnDoor(i * doorSpacing);
        }

        ActivateFrontDoor();
    }

    private void SpawnDoor(Vector3 position)
    {
        if (doorCount >= maxDoors) return;

        int index;
        do
        {
            index = UnityEngine.Random.Range(0, doorPrefabs.Length);
        } while (index == lastIndex && doorPrefabs.Length > 1);

        lastIndex = index;
        GameObject newDoor = Instantiate(doorPrefabs[index], position, Quaternion.identity);
        newDoor.tag = "Door";
        doorCount++;

        Door door = newDoor.GetComponent<Door>();
        SlideDoor slide = newDoor.GetComponent<SlideDoor>();
        if (door != null && slide != null)
        {
            slide.SetActive(false);
            slide.doorComponent = door;

            door.onDoorOpened.AddListener(() =>
            {
                OnDoorOpened(newDoor);
            });
        }

        activeDoors.Add(newDoor);
    }

    private void OnDoorOpened(GameObject openedDoor)
    {
        activeDoors.Remove(openedDoor);
        Destroy(openedDoor);

        // ✅ 全ドア開放時の処理
        if (doorCount >= maxDoors && activeDoors.Count == 0)
        {
            Debug.Log("All doors opened!");

            // ⏹ タイマー停止
            if (timerScript != null)
            {
                timerScript.enabled = false;
                Debug.Log("Timer stopped.");
            }

            // 🟩 ボタン表示
            if (clearButton != null)
            {
                clearButton.SetActive(true);
                Debug.Log("Clear button activated!");
            }

            return;
        }

        // 🔁 残りがあるなら次のドア生成
        if (doorCount < maxDoors)
        {
            Vector3 lastPos = activeDoors.Count > 0
                ? activeDoors[activeDoors.Count - 1].transform.position
                : openedDoor.transform.position;

            Vector3 nextPos = lastPos + doorSpacing;
            SpawnDoor(nextPos);
        }

        ActivateFrontDoor();
    }

    private void ActivateFrontDoor()
    {
        foreach (var door in activeDoors)
        {
            var slide = door.GetComponent<SlideDoor>();
            if (slide != null) slide.SetActive(false);
        }

        if (activeDoors.Count > 0)
        {
            var frontSlide = activeDoors[0].GetComponent<SlideDoor>();
            if (frontSlide != null) frontSlide.SetActive(true);
        }
    }
    public void OnNextButtonClick()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        NextButton();
#endif
    }


}
