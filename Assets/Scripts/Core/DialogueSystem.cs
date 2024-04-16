using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The controller for starting and controlling dialogue conversations on screen
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    #region System Variables
    //References to Systems
    public DialogueContainer dialogueContainer = new DialogueContainer();

    #endregion

    //singleton
    public static DialogueSystem instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
