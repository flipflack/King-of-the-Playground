using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Yarn.Unity;

// Swaps Lola's and the NPC's faces using hashtags on Yarn lines:
//   Grace: sniffle #emotion:sad            -> the speaker (Grace) shows "sad"
//   Lola: What?! #emotion:upset            -> Lola shows "upset"
//   *She looks up at you* #npc:sad         -> narration: set the NPC's face
//   *You sigh* #lola:upset                 -> narration: set Lola's face
// A face stays until a later line changes it.
// Each emotion has a mouth-closed sprite and an optional mouth-open one:
// the mouth-open one is used while that character is the one talking.
public class CharacterEmotionPresenter : DialoguePresenterBase
{
    [Header("Characters on screen")]
    [SerializeField] private SpriteRenderer lolaRenderer;
    [SerializeField] private SpriteRenderer npcRenderer;
    [SerializeField] private string playerName = "Lola";

    [Header("Lola's faces")]
    [SerializeField] private Sprite lolaDefault;
    [SerializeField] private string lolaStartingEmotion = "neutral";
    [SerializeField] private List<EmotionSprite> lolaEmotions = new();

    [Header("Who is talking")]
    [Tooltip("Darken whoever is not talking, so it's clear who has the line.")]
    [SerializeField] private bool dimListener = true;
    [SerializeField] private Color listenerColor = new Color(0.65f, 0.65f, 0.65f, 1f);

    private string lolaEmotion;
    private string npcEmotion;

    public override YarnTask OnDialogueStartedAsync()
    {
        lolaEmotion = lolaStartingEmotion;
        npcEmotion = CurrentNPC() != null ? CurrentNPC().startingEmotion : null;
        Refresh(null);
        return YarnTask.CompletedTask;
    }

    public override YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
    {
        string speaker = line.CharacterName;
        bool speakerIsLola = IsLola(speaker);
        bool speakerIsNpc = IsNpc(speaker);

        foreach (string tag in line.Metadata ?? Array.Empty<string>())
        {
            int colon = tag.IndexOf(':');
            if (colon <= 0) continue;
            string key = tag.Substring(0, colon).Trim().ToLowerInvariant();
            string emotion = tag.Substring(colon + 1).Trim();

            if (key == "emotion")
            {
                if (speakerIsLola) lolaEmotion = emotion;
                else if (speakerIsNpc) npcEmotion = emotion;
            }
            else if (key == "lola") lolaEmotion = emotion;
            else if (key == "npc") npcEmotion = emotion;
        }

        Refresh(speakerIsLola ? lolaRenderer : speakerIsNpc ? npcRenderer : null);
        return YarnTask.CompletedTask;
    }

    public override YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
    {
        Refresh(lolaRenderer, mouthOpen: false);   // Lola is choosing what to say
        return YarnTask<DialogueOption>.FromResult(null);
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        Refresh(null);
        return YarnTask.CompletedTask;
    }

    private void Refresh(SpriteRenderer talking, bool mouthOpen = true)
    {
        if (lolaRenderer != null)
        {
            Sprite s = EmotionSprite.Find(lolaEmotions, lolaEmotion, mouthOpen && talking == lolaRenderer, lolaDefault);
            if (s != null) lolaRenderer.sprite = s;
        }

        NPC_Data npc = CurrentNPC();
        if (npcRenderer != null && npc != null)
        {
            Sprite s = npc.GetEmotionSprite(npcEmotion, mouthOpen && talking == npcRenderer);
            if (s != null) npcRenderer.sprite = s;
        }

        SpriteRenderer lit = dimListener ? talking : null;
        if (lolaRenderer != null) lolaRenderer.color = lit == null || lit == lolaRenderer ? Color.white : listenerColor;
        if (npcRenderer != null) npcRenderer.color = lit == null || lit == npcRenderer ? Color.white : listenerColor;
    }

    private bool IsLola(string speaker) =>
        !string.IsNullOrWhiteSpace(speaker) && string.Equals(speaker.Trim(), playerName, StringComparison.OrdinalIgnoreCase);

    private bool IsNpc(string speaker)
    {
        NPC_Data npc = CurrentNPC();
        return npc != null && !string.IsNullOrWhiteSpace(speaker) &&
               string.Equals(speaker.Trim(), npc.npcName, StringComparison.OrdinalIgnoreCase);
    }

    private static NPC_Data CurrentNPC() => GameManager.Instance != null ? GameManager.Instance.CurrentNPC : null;
}

[Serializable]
public class EmotionSprite
{
    [Tooltip("The word used in Yarn, e.g. sad -> #emotion:sad")]
    public string emotion;
    [Tooltip("Mouth closed (listening).")]
    public Sprite sprite;
    [Tooltip("Mouth open (talking). Optional: leave empty to always use the one above.")]
    public Sprite talkingSprite;

    public static Sprite Find(List<EmotionSprite> list, string emotion, bool talking, Sprite fallback)
    {
        if (list != null && !string.IsNullOrWhiteSpace(emotion))
        {
            foreach (EmotionSprite e in list)
            {
                if (e == null || !string.Equals(e.emotion?.Trim(), emotion.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                if (talking && e.talkingSprite != null) return e.talkingSprite;
                if (e.sprite != null) return e.sprite;
                if (e.talkingSprite != null) return e.talkingSprite;
            }
        }
        return fallback;
    }
}
