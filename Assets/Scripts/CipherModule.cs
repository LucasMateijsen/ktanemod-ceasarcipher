using UnityEngine;

public abstract class CipherModule : MonoBehaviour
{
	protected string Answer = string.Empty;
    public CeasarCipherPuzzle Ccp;

	protected delegate void AnswerUpdate();

	// ReSharper disable once InconsistentNaming
	public KMAudio KMAudio;
	public KMSelectable[] Buttons;

	protected virtual void Init()
	{
        Ccp = CeasarCipherFactory.Instance.GenerateQuestion();

		SetUpButtons();
		SetUpButtonAudio();
	}

	private void SetUpButtonAudio()
	{
		foreach (var button in Buttons)
		{
			button.OnInteract += delegate
			{
				if (KMAudio != null) KMAudio.HandlePlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
				return false;
			};
		}
	}

	protected abstract void Solve();

	private void SetUpButtons()
	{
        var buttonsText = Ccp.GetButtonText();

		for (var i = 0; i < 12; i++)
		{
			var button = Buttons[i];
            var textmesh = Buttons[i].GetComponentInChildren<TextMesh>();
            textmesh.text = buttonsText[i];

            button.OnInteract += delegate
			{
				var buttonText = button.GetComponentInChildren<TextMesh>().text;
				Answer += buttonText;
                Solve();
				return false;
			};
		}
	}
}