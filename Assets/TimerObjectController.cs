using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimerController : MonoBehaviour
{
    [Header("設定")]
    public float timerDuration = 5f;
    public float delayAfterEnd = 3f;

    [Header("ボタンで開始したい")]
    public Button startButton;
    public GameObject buttonPanel;
    public GameObject manual;

    [Header("最初から止めたい（スクリプト）")]
    public MonoBehaviour[] stopAtStartScripts;

    [Header("最初から非表示にしたい（オブジェクト）")]
    public GameObject[] stopAtStartObjects;

    [Header("タイマー終了で止めたい（スクリプト）")]
    public GameObject[] stopOnEndButtons;
    public MonoBehaviour[] stopOnEndScripts;

    [Header("タイマー終了で非表示にしたい（オブジェクト）")]
    public GameObject[] stopOnEndObjects;

    [Header("タイマー終了後しばらくして表示したいボタン")]
    public GameObject[] enableAfterDelayButtons;

    [Header("タイマー終了後しばらくして動かしたいスクリプト")]
    public MonoBehaviour[] enableAfterDelayScripts;

    private float timer;
    private bool isRunning = false;
    private bool isReady = false;

    void Start()
    {
        timer = timerDuration;

        // ① 最初から止めたいスクリプト OFF
        foreach (var script in stopAtStartScripts)
            if (script != null) script.enabled = false;

        // ② 最初から非表示
        foreach (var obj in stopAtStartObjects)
            if (obj != null) obj.SetActive(false);

        // ③ タイマー終了後 ON にしたいもの → 最初は OFF
        foreach (var script in enableAfterDelayScripts)
            if (script != null) script.enabled = false;

        foreach (var obj in enableAfterDelayButtons)
            if (obj != null) obj.SetActive(false);

        // ④ 終了で止めたいものも最初は OFF（後で ON にする）
        foreach (var script in stopOnEndScripts)
            if (script != null) script.enabled = false;

        foreach (var obj in stopOnEndObjects)
            if (obj != null) obj.SetActive(false);

        // ⑤ スタートボタン設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(() =>
            {
                isReady = true;
                isRunning = true;

                if (buttonPanel != null)
                    buttonPanel.SetActive(false);

                if (manual != null)
                    Destroy(manual);
            });
        }
        else
        {
            Debug.LogWarning("StartButton が未設定です");
        }
    }

    void Update()
    {
        if (!isReady || !isRunning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isRunning = false;
            OnTimerEnd();
        }
    }

    void OnTimerEnd()
    {
        // タイマー終了時に止める
        foreach (var script in stopOnEndScripts)
            if (script != null) script.enabled = false;

        foreach (var obj in stopOnEndObjects)
            if (obj != null) obj.SetActive(false);

        // 最初から止めておいたものを復帰
        foreach (var script in stopAtStartScripts)
            if (script != null) script.enabled = true;

        foreach (var obj in stopAtStartObjects)
            if (obj != null) obj.SetActive(true);

        // 全停止（例外除く）
        DisableAllExceptWhitelist();

        // 一定時間後にボタン & スクリプト ON
        StartCoroutine(EnableAfterDelay());
    }

    void DisableAllExceptWhitelist()
    {
        var allScripts = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var script in allScripts)
        {
            if (script == null) continue;

            // TimerController 自身は止めない
            if (script == this) continue;

            // ★ UI 系は絶対止めない（これが無いと Button が壊れる）
            if (script.GetComponent<Canvas>() != null) continue;
            if (script.GetComponent<Graphic>() != null) continue; // Text, Image など
            if (script.GetComponent<Selectable>() != null) continue; // Button, Toggle など
            if (script.GetComponent<CanvasGroup>() != null) continue;
            if (script.GetComponent<CanvasRenderer>() != null) continue;
            if (script.GetComponent<EventSystem>() != null) continue;

            // ホワイトリスト判定
            bool isStopAtStart = System.Array.Exists(stopAtStartScripts, s => s == script);
            bool isEnableAfterDelayScript = System.Array.Exists(enableAfterDelayScripts, s => s == script);

            // ★ ボタン GameObject そのものに付いているコンポーネントは許可
            bool isEnableAfterDelayButton = false;
            foreach (var btnObj in enableAfterDelayButtons)
            {
                if (btnObj != null && script.gameObject == btnObj)
                {
                    isEnableAfterDelayButton = true;
                    break;
                }
            }

            // ホワイトリスト以外を止める
            if (!isStopAtStart && !isEnableAfterDelayScript && !isEnableAfterDelayButton)
            {
                script.enabled = false;
            }
        }
    }



    IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterEnd);

        // タイマー後に有効化するスクリプト
        foreach (var script in enableAfterDelayScripts)
            if (script != null) script.enabled = true;

        // タイマー後に表示するボタン
        foreach (var obj in enableAfterDelayButtons)
            if (obj != null) obj.SetActive(true);
    }
}

