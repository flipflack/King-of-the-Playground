using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CharacterBoxThemePresenter : DialoguePresenterBase
{
    [Serializable]
    public class CharacterBoxStyle
    {
        public string characterName;      // Must match the name in your .yarn file (e.g. "Viviana")
        public Sprite crayon;             // Crayon nameplate for this character
        public Sprite boxBackground;      // Optional: wavy box sprite for this character
        public Sprite continueStar;       // Optional: star button sprite for this character
        public Color textColor = new Color(0.24f, 0.16f, 0.12f, 1f);
        public Color nameColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    }

    [Header("UI References")]
    [SerializeField] private Image boxBackgroundImage;
    [SerializeField] private Image crayonImage;
    [SerializeField] private Image continueStarImage;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text characterNameText;

    [Header("Character Styles")]
    [SerializeField] private CharacterBoxStyle defaultStyle;
    [SerializeField] private List<CharacterBoxStyle> characterStyles = new();

    [Header("Narrator & Choices")]
    [Tooltip("Lines with no speaker (e.g. *Andy sits on the ground*) hide the crayon and name.")]
    [SerializeField] private bool hideCrayonForNarrator = true;
    [Tooltip("Whose crayon to show while the player is picking an option.")]
    [SerializeField] private string playerName = "Lola";

    private readonly Dictionary<string, CharacterBoxStyle> _styleLookup =
        new(StringComparer.OrdinalIgnoreCase);

    private void Awake()
    {
        _styleLookup.Clear();
        foreach (var style in characterStyles)
        {
            if (style != null && !string.IsNullOrWhiteSpace(style.characterName))
            {
                _styleLookup[style.characterName.Trim()] = style;
            }
        }
    }

    public override YarnTask OnDialogueStartedAsync()
    {
        ApplyStyle(defaultStyle);
        return YarnTask.CompletedTask;
    }

    public override YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
    {
        ShowSpeaker(line.CharacterName);
        return YarnTask.CompletedTask;
    }

    public override YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
    {
        // the player is choosing what Lola says, so show Lola's crayon
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            ShowSpeaker(playerName);
            if (characterNameText != null) characterNameText.text = playerName;
        }
        return YarnTask<DialogueOption>.FromResult(null);
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        return YarnTask.CompletedTask;
    }

    private void ShowSpeaker(string speaker)
    {
        bool isNarrator = string.IsNullOrWhiteSpace(speaker);

        if (!isNarrator && _styleLookup.TryGetValue(speaker.Trim(), out var matchedStyle))
            ApplyStyle(matchedStyle);
        else
            ApplyStyle(defaultStyle);

        bool showNameplate = !(isNarrator && hideCrayonForNarrator);
        if (crayonImage != null) crayonImage.enabled = showNameplate;
        if (characterNameText != null) characterNameText.enabled = showNameplate;
    }

    private void ApplyStyle(CharacterBoxStyle style)
    {
        if (style == null) return;

        if (boxBackgroundImage != null && style.boxBackground != null)
            boxBackgroundImage.sprite = style.boxBackground;

        if (crayonImage != null)
        {
            Sprite crayon = style.crayon != null ? style.crayon : defaultStyle?.crayon;
            if (crayon != null) crayonImage.sprite = crayon;
        }

        if (continueStarImage != null && style.continueStar != null)
            continueStarImage.sprite = style.continueStar;

        if (dialogueText != null)
            dialogueText.color = style.textColor;

        if (characterNameText != null)
            characterNameText.color = style.nameColor;
    }
}
