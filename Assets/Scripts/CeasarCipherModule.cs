using System.Collections.Generic;
using System.Linq;
using CaesarCipher;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class CeasarCipherModule : MonoBehaviour
{
    // ReSharper disable once InconsistentNaming
    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMSelectable[] Buttons;
    public TextMesh[] ButtonLabels;
    public TextMesh DisplayText;

    private string _puzzleWord;
    private string _answer = "";

    void Start()
    {
        DisplayText.text = "";
        Module.OnActivate += Activate;

        // Generate 12 random characters to display on the buttons
        var randomChars = new List<char>();
        while (randomChars.Count < 12)
        {
            var ch = (char) Rnd.Range(65, 91);
            if (!randomChars.Contains(ch))
                randomChars.Add(ch);
        }

        // The solution consists of 5 of those characters
        _puzzleWord = new string(randomChars.Take(5).ToArray());
        Debug.Log("[CaesarCipher] Solution is " + _puzzleWord);

        // Re-scramble the order and then label the buttons
        var i = 0;
        while (randomChars.Count > 0)
        {
            var index = Rnd.Range(0, randomChars.Count);
            var letter = randomChars[index];
            randomChars.RemoveAt(index);

            ButtonLabels[i].text = letter.ToString();
            Buttons[i].OnInteract += delegate
            {
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                _answer += letter;
                if (_answer.Length == _puzzleWord.Length)
                {
                    if (_answer == _puzzleWord)
                        Module.HandlePass();
                    else
                        Module.HandleStrike();
                    _answer = "";
                }
                return false;
            };
            i++;
        }
    }

    private void Activate()
    {
        var serial = Bomb.GetSerialNumber();
        bool hasLitNsa, hasParallel, hasCar;
        int numBatteries;

        if (serial == null)
        {
            // Generate random values for testing in Unity
            serial = string.Join("", Enumerable.Range(0, 6).Select(i => Rnd.Range(0, 36)).Select(i => i < 10 ? ((char) ('0' + i)).ToString() : ((char) ('A' + i - 10)).ToString()).ToArray());
            hasLitNsa = Rnd.Range(0, 2) == 0;
            hasParallel = Rnd.Range(0, 2) == 0;
            hasCar = Rnd.Range(0, 2) == 0;
            numBatteries = Rnd.Range(0, 7);
        }
        else
        {
            hasLitNsa = Bomb.IsIndicatorOn(KMBombInfoExtensions.KnownIndicatorLabel.NSA);
            hasParallel = Bomb.GetPorts().Contains("Parallel");
            hasCar = Bomb.IsIndicatorPresent(KMBombInfoExtensions.KnownIndicatorLabel.CAR);
            numBatteries = Bomb.GetBatteryCount();
        }

        // Calculate the offset
        var offset = 0;
        if (hasLitNsa && hasParallel)
        {
            // Offset stays at 0
        }
        else
        {
            // Offset by Battery count
            offset += numBatteries;

            // Offset by Serial vowel check
            if (serial.Intersect("AEIOU").Any())
                offset -= 1;

            // Offset by CAR indicator
            if (hasCar)
                offset += 1;

            // Offset by Serial even ending
            if ("02468".Contains(serial.Last()))
                offset += 1;
        }

        Debug.Log("[CaesarCipher] Offset is " + offset);
        DisplayText.text = new string(_puzzleWord.Select(ch => (char) ((ch - 'A' - offset + 26) % 26 + 'A')).ToArray());
    }
}
