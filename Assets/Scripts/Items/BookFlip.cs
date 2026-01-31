using UnityEngine;

public class BookFlip : MonoBehaviour
{
    public GameObject TextList;
    [SerializeField] private int i = 0;
    public void FlipRight()
    {
        if (i+1 < TextList.transform.childCount)
        {
            GameObject Oldchild = TextList.transform.GetChild(i).gameObject;
            GameObject child = TextList.transform.GetChild(i + 1).gameObject;
            if (child != null)
            {
                Oldchild.SetActive(false);
                child.SetActive(true);
            }
            i++;

        }
        else
        {
            return;
        } 
    }
    public void FlipLeft()
    {
        if (i - 1 >= 0 )
        {
            GameObject Oldchild = TextList.transform.GetChild(i).gameObject;
            GameObject child = TextList.transform.GetChild(i - 1).gameObject;
            if (child != null)
            {
                Oldchild.SetActive(false);
                child.SetActive(true);
            }
            i--;

        }
        else {  return; }
    }
}
