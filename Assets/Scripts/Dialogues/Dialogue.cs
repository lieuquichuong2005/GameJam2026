using UnityEngine;
using TMPro;
using System.Collections;
public class Dialogue : MonoBehaviour, IDIalogueable
{
    [SerializeField] protected private TMP_Text popUpText;
    [TextArea]
    [SerializeField] protected private string dialogue;

    [SerializeField] private Animator anim;

    private bool isActive;
    private Camera cam;

    public TMP_Text TextHolder => popUpText;

    public string Dialogues => dialogue;

    public bool IsActive => isActive;

    public void Execute()
    {
        if (IsActive)
        {
            StopAllCoroutines();
        }
        anim.Play("FadeIn");
        popUpText.text = dialogue;
        isActive = true;
        StartCoroutine(CloseDialogue(2f));
    }
    private IEnumerator CloseDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("FadeOut");
        popUpText.text = null;
        isActive = false;

    }

}
