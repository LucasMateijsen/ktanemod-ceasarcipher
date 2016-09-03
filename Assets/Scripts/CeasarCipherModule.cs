using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class CeasarCipherModule : CipherModule
{
	public TextMesh DisplayText;

	void Start()
	{
		DisplayText.text = string.Empty;
        Init();
		GetComponent<KMBombModule>().OnActivate += SetDisplay;
	}

	protected override void Solve()
	{
        if (Answer.Length != 5) return;

		if (Ccp.CheckAnswer(Answer.ToCharArray()))
			GetComponent<KMBombModule>().HandlePass();
		else
			GetComponent<KMBombModule>().HandleStrike();

		Answer = string.Empty;
	}

	private void SetDisplay()
    {
        int batteryCount = 0;
        List<string> responses = GetComponent<KMBombInfo>().QueryWidgets(KMBombInfo.QUERYKEY_GET_BATTERIES, null);
        foreach (string response in responses)
        {
            Dictionary<string, int> responseDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);
            batteryCount += responseDict["numbatteries"];
        }

        Ccp.CalculateOffset(batteryCount);
        DisplayText.text = Ccp.GetDisplayText();
	}
}