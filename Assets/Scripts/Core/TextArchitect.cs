using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect
{
    #region Variables
    //shortcut variables
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;

    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = ""; //trying to build or append
    public string preText { get; private set; } = "";
    private int preTextLength = 0; //can be used for the fade method

    public string fullTargetText => preText + targetText;

    //Tracking
    public enum BuildMethod { instant, typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    //Chaning color
    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    //speed of text
    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;

    //speed up characters
    private int characterMultiplier = 1;
    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }

    public bool autoComplete = false;
    #endregion


    #region Constructors
    public TextArchitect(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }

    public TextArchitect(TextMeshPro tmpro_world)
    {
        this.tmpro_world = tmpro_world;
    }
    #endregion

    #region Coroutines

    /// <summary>
    /// Build text
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    /// <summary>
    /// Append text to what is already in the text architect
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    /// <summary>
    /// Stopping the text
    /// </summary>
    public void Stop()
    {
        if(!isBuilding)
        {
            return;
        }

        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    /// <summary>
    /// Building of the text
    /// </summary>
    /// <returns></returns>
    IEnumerator Building()
    {
        Prepare();

        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }


    }

    /// <summary>
    /// Runs when the build process is complete
    /// </summary>
    private void OnComplete()
    {
        buildProcess = null;
        autoComplete = false;
    }

    public void ForceComplete()
    {
        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod fade:
                break;
        }

        Stop();
        OnComplete();
    }

    /// <summary>
    /// Preparing everything based on whatever build method we have
    /// </summary>
    private void Prepare()
    {
        switch(buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    #region Prepare Methods
    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;

        tmpro.ForceMeshUpdate(); //Any changes we make will be applied at this point
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    private void Prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }
    private void Prepare_Fade()
    {

    }

    #endregion 

    private IEnumerator Build_Typewriter()
    {
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += autoComplete ? charactersPerCycle * 5 : charactersPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }

    #endregion



}
