using System.Collections.Generic;
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
        var serial = string.Empty;
        var serialResponses = GetComponent<KMBombInfo>().QueryWidgets(KMBombInfo.QUERYKEY_GET_SERIAL_NUMBER, null);
        foreach (var serialResponse in serialResponses)
        {
            Dictionary<string, string> serialDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialResponse);
            serial = serialDict["serial"];
        }

        var batteriesCount = 0;
        var batteriesResponses = GetComponent<KMBombInfo>().QueryWidgets(KMBombInfo.QUERYKEY_GET_BATTERIES, null);
        foreach (var batteriesResponse in batteriesResponses)
        {
            Dictionary<string, int> batteriesDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(batteriesResponse);
            batteriesCount += batteriesDict["numbatteries"];
        }

        Ccp.CalculateOffset(batteriesCount, serial);
        DisplayText.text = Ccp.GetDisplayText();
    }
}