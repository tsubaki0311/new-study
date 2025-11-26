using UnityEngine;

public class Right_wall1: MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private float duration = 1f;

    private Material mat;
    private float value = 0f;
    private bool dissolveOut = false;

    // Start is called before the first frame update
    void Start()
    {
        mat = rend.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
          dissolveOut = true;

        if(Input.GetKeyDown(KeyCode.G))
          dissolveOut = false;

        float target = dissolveOut ? 1f : 0f;
        value = Mathf.MoveTowards(value, target, Time.deltaTime / duration);

        mat.SetFloat("_Dissolve",value);
    }
}
