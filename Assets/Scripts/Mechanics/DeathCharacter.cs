using UnityEngine;

public class DeathCharacter : Possessable
{
    [TextArea(3, 6)] public string[] dialogueLines;
    public string characterName = "Morte";
    public bool preventPossession = true; 

    // Chamado pelo GhostController quando o jogador tenta possuir
    public void TriggerDialogue(PlayerController ghost)
    { 
        if (ghost != null)
            ghost.enabled = false;
         
        DialogueManager.Instance.StartDialogue(dialogueLines, characterName, () =>
        { 
            if (ghost != null)
            { 
                ghost.enabled = true;
                 
            }
        });
    }
}
