using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class TestingArchitect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        string[] lines = new string[6]
        {
            "This is a random line of text...guess what? this is a random long line of text. Pretty cool right",
            "I want to say something, COME OVER HERE. Not there come over here. Noooooo here right here just come right here",
            "The world is so fucked up sometimes",
            "Don't lose hope, it won't get better!",
            "Fuck you Mehdi, and salma smells like cheese puffs mixed with peanut butter ass balls that have been rubbed in the dirt",
            "Clearly this is working. Check it out it's working. Wooooooow I was able to make it work that is so cool. Such a long sentence"
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if(!architect.autoComplete)
                    {
                        architect.autoComplete = true;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }
                }
                architect.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}
