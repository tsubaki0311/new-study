using UnityEngine;

public class DissolveSwapByPosition : MonoBehaviour
{
    [SerializeField] private Renderer[] rendA; // 消す側（A）4つ
    [SerializeField] private Renderer[] rendB; // 出す側（B）4つ
    [SerializeField] private float duration = 1f;

    [SerializeField] private Transform triggerObject;  

    [Header("トリガー条件（このXZを超えたら発動）")]
    [SerializeField] private float triggerX = 10f;     // この位置を超えたら発動
    [SerializeField] private float triggerZ = 5f;

    private Material[] matsA;
    private Material[] matsB;

    private float valueA = 0f;
    private float valueB = 1f;

    private bool isSwapping = false;
    private bool hasTriggered = false;

    void Start()
    {
        matsA = new Material[rendA.Length];
        matsB = new Material[rendB.Length];

        for (int i = 0; i < rendA.Length; i++)
        {
            matsA[i] = rendA[i].material;
            matsA[i].SetFloat("_Dissolve", 0f); // A = 見える
        }

        for (int i = 0; i < rendB.Length; i++)
        {
            matsB[i] = rendB[i].material;
            matsB[i].SetFloat("_Dissolve", 1f); // B = 消えている
        }
    }

    void Update()
    {
        // 条件（位置判定）
        if (!hasTriggered && triggerObject.position.x >= triggerX && triggerObject.position.z >= triggerZ)
        {
            isSwapping = true;
            hasTriggered = true;
        }

        // スワップ中
        if (isSwapping)
        {
            valueA = Mathf.MoveTowards(valueA, 1f, Time.deltaTime / duration);
            valueB = Mathf.MoveTowards(valueB, 0f, Time.deltaTime / duration);

            foreach (var m in matsA) m.SetFloat("_Dissolve", valueA); // A消える
            foreach (var m in matsB) m.SetFloat("_Dissolve", valueB); // B現れる

            if (valueA >= 1f && valueB <= 0f)
                isSwapping = false;
        }
    }
}
