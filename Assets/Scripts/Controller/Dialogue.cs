using UnityEngine;
using TMPro;
using System.Collections;
public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text popUpText;
    [TextArea]
    [SerializeField] private string dialogue;

    [SerializeField] private Animator anim;

    private bool IsActive;
    private Camera cam;
    public void Execute()
    {
        if (IsActive)
        {
            Debug.Log("Stop");
            StopAllCoroutines();
        }
        anim.Play("FadeIn");
        popUpText.text = dialogue;
        IsActive = true;
        StartCoroutine(CloseDialogue(2f));
    }
    private IEnumerator CloseDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("FadeOut");
        popUpText.text = null;
        IsActive = false;

    }

    private void Awake()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        if (!cam) return;
        transform.forward = cam.transform.forward;
    }
}
