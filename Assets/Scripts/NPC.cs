using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public float maxHighlightRange = 32.0f;
    public float maxInteractableRange = 3.0f;
    public bool isInteractable = true;
    public bool isInteracting = false;
    public bool isHighlighted = false;
    public bool mouseIsOver = false;
    public GameObject display;

    //Dialogue
    public Queue<string> sentences;
    public Dialogue baseDialogue;

    //Text UI
    public GameObject textObj;
    [SerializeField]
    GameObject textInstance;
    Text nameText;
    Text textText;

    //Highlight NPC
    float distanceToPlayerSquared;
    PlayerController player;
    Animator anim;

    AudioSource src;
    public AudioClip clip;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        sentences = new Queue<string>();
        player = FindObjectOfType<PlayerController>();
        player.isTalking = false;
    }

    public virtual void TriggerDialogue()
    {
        StartDialogue(baseDialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        src.PlayOneShot(clip);
        player.isTalking = true;
        //Debug.Log("Starting conversation with " + dialogue.name);

        if (textInstance == null)
        {
            textInstance = Instantiate(textObj, Vector3.zero, Quaternion.identity).transform.GetChild(0).gameObject;
        }
        else textInstance.SetActive(true);

        if (nameText == null || textText == null)
        {
            nameText = textInstance.transform.GetChild(0).GetComponent<Text>();
            textText = textInstance.transform.GetChild(1).GetComponent<Text>();
        }

        nameText.text = baseDialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        textText.text = sentence;
    }

    public virtual void EndDialogue()
    {
        src.PlayOneShot(clip);
        player.isTalking = false;
        isInteracting = false;
        textInstance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) player = FindObjectOfType<PlayerController>();
        if (isInteractable)
        {
            if (player != null)
            {
                Vector3 diffPos = transform.position - player.transform.position;
                distanceToPlayerSquared = (diffPos.x * diffPos.x) + (diffPos.y * diffPos.y);

                if (distanceToPlayerSquared > maxInteractableRange)
                {
                    isHighlighted = false;
                    isInteracting = false;
                }

                if (!isHighlighted)
                {
                    display.SetActive(false);
                }

                if (distanceToPlayerSquared <= maxInteractableRange && !isInteracting && isHighlighted)
                {
                    display.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        isInteracting = true;
                        TriggerDialogue();
                        //Debug.Log("Player is now interacting with NPC");
                        //deploy code to display NPC dialof and limit player controls
                        //NPC isInteracting=true indicates this state.
                    }

                    /*if (Input.GetKey(KeyCode.E))
                    {
                        isInteracting = true;
                        TriggerDialogue();
                        Debug.Log("Player is now interacting with NPC");
                        //deploy code to display NPC dialof and limit player controls
                        //NPC isInteracting=true indicates this state.
                    }*/
                }

                if (isInteracting)
                {
                    //deploy code to display NPC dialog and limit player controls
                    isHighlighted = false;
                    if (Input.GetKey(KeyCode.X))//placeholder key press to exit isInteracting state
                    {
                        isInteracting = false;
                        EndDialogue();
                        Debug.Log("Player is done talking");
                    }
                }
            }
        }
        anim.SetBool("Highlight", (isHighlighted && !isInteracting));
    }

    private void OnMouseOver()
    {
        mouseIsOver = true;
        if (distanceToPlayerSquared <= maxInteractableRange && !isHighlighted && !isInteracting)
        {
            //highlight NPC
            isHighlighted = true;
        }
    }

    private void OnMouseExit()
    {
        isHighlighted = false;
        mouseIsOver = false;
    }
}
