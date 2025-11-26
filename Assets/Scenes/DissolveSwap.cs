using UnityEngine;

public class DissolveSwapManager : MonoBehaviour
{
    [Header("消えるオブジェクト (最大 4 個)")]
    [SerializeField] private Renderer[] groupA; 

    [Header("現れるオブジェクト (最大 4 個)")]
    [SerializeField] private Renderer[] groupB;

    [SerializeField] private float duration = 1f;
    [SerializeField] private float waitTime = 0.5f; // A が消えた後に B を出すまでの待ち時間

    private Material[] matsA;
    private Material[] matsB;

    private float timer = 0f;
    private bool isRunning = false;
    private enum Phase { Idle, DissolveA, Wait, AppearB }
    private Phase phase = Phase.Idle;

    void Start()
    {
        // マテリアル複製
        matsA = new Material[groupA.Length];
        matsB = new Material[groupB.Length];

        for (int i = 0; i < groupA.Length; i++)
        {
            matsA[i] = groupA[i].material;
            matsA[i].SetFloat("_Dissolve", 0f); // A は最初見える
        }

        for (int i = 0; i < groupB.Length; i++)
        {
            matsB[i] = groupB[i].material;
            matsB[i].SetFloat("_Dissolve", 1f); // B は最初消えてる
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && phase == Phase.Idle)
        {
            phase = Phase.DissolveA;
            timer = 0f;
        }

        switch (phase)
        {
            case Phase.DissolveA:
                timer += Time.deltaTime / duration;
                float valueA = Mathf.Clamp01(timer);
                foreach (var mat in matsA)
                    mat.SetFloat("_Dissolve", valueA);

                if (timer >= 1f)
                {
                    timer = 0f;
                    phase = Phase.Wait;
                }
                break;

            case Phase.Wait:
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0f;
                    phase = Phase.AppearB;
                }
                break;

            case Phase.AppearB:
                timer += Time.deltaTime / duration;
                float valueB = 1f - Mathf.Clamp01(timer);
                foreach (var mat in matsB)
                    mat.SetFloat("_Dissolve", valueB);

                if (timer >= 1f)
                {
                    phase = Phase.Idle;
                }
                break;
        }
    }
}
