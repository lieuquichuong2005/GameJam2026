using TMPro;
using UnityEngine;
using TMPro;
public interface IDIalogueable
{
    TMP_Text TextHolder { get; }
    string Dialogues { get; }
    

    bool IsActive { get; }
    void Execute();
}
