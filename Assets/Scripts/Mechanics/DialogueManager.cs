using TMPro;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public Image profile;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject continueIndicator;

    [Header("Settings")]
    public float typingSpeed = 0.03f;

    private string[] dialogueLines;
    private int currentLine;
    private bool isTyping;
    private Action onDialogueEnd;


    public void Speech(string text, string actorName)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;
        characterNameText.text = actorName;


    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(string[] lines, string characterName, Action onEnd = null)
    {
        if (lines == null || lines.Length == 0) return;

        dialoguePanel.SetActive(true);
        characterNameText.text = characterName;
        dialogueLines = lines;
        currentLine = 0;
        onDialogueEnd = onEnd;
        StartCoroutine(TypeLine());
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                isTyping = false;
                continueIndicator.SetActive(true);
            }
            else
            {
                NextLine();
            }
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        continueIndicator.SetActive(false);
        dialogueText.text = "";

        string line = dialogueLines[currentLine];
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueIndicator.SetActive(true);
    }

    private void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }
}
