using System.Collections.Generic;
using System.Linq;

public class CeasarCipherPuzzle
{
    public int[] PuzzleWord;
    public int Offset;

    public CeasarCipherPuzzle()
    {
        // Generate word with 5 random characters
        var randomChars = new List<int>();
        while (randomChars.Count < 5)
        {
            var j = UnityEngine.Random.Range(65, 91);
            if (randomChars.Contains(j) == false)
            {
                randomChars.Add(j);
            }
        }
        PuzzleWord = randomChars.ToArray();
    }

    public void CalculateOffset(int batteryCount, string serial, List<string> ports, Dictionary<string, bool> indicators)
    {
        Offset = 0;

        // Check for paralel port and FRK. If so, we are done
        var frkOn = indicators.Any(x => x.Key == "NSA" && x.Value);
        if (frkOn && ports.Contains("Parallel"))
        {
            return;
        }

        // Offset by Battery count
        Offset += batteryCount;

        // Offset by Serial vowel check
        var vowels = "AEIOU".ToCharArray();
        var serialArr = serial.ToCharArray();
        var hasVowels = serialArr.Intersect(vowels).Any();

        if (hasVowels)
        {
            Offset -= 1;
        }

        // Offset by CAR indicator
        if (indicators.ContainsKey("CAR"))
        {
            Offset += 1;
        }

        // Offset by Serial even ending
        var evens = "02468".ToCharArray();
        var lastchar = serial[5];

        if (evens.Contains(lastchar))
        {
            Offset += 1;
        }
    }

    public bool CheckAnswer(char[] answer)
    {
        var charsAsInt = answer.Select(x => (int)x).ToArray();

        for (var i = 0; i < PuzzleWord.Length; i++)
        {
            if (charsAsInt[i] != PuzzleWord[i])
            {
                return false;
            }
        }

        return true;
    }

    public string[] GetButtonText()
    {
        var uniqueChars = new List<int>();
        uniqueChars.AddRange(PuzzleWord);

        while (uniqueChars.Count < 12)
        {
            var charInt = UnityEngine.Random.Range(65, 91);
            if (uniqueChars.Contains(charInt) == false)
            {
                uniqueChars.Add(charInt);
            }
        }

        var chars = uniqueChars.Select(x => (char)x);

        return chars.Select(x => x.ToString()).ToArray();
    }

    public string GetDisplayText()
    {
        var text = "";

        for (int i = 0; i < PuzzleWord.Length; i++)
        {
            text += (char)GetCharWithOffset(PuzzleWord[i], Offset);
        }

        return text;
    }

    private int GetCharWithOffset(int ch, int offset)
    {
        int result = ch - offset;

        //if (result > 90)
        //{
        //    result = 64 + (result - 90);
        //}

        if (result < 65)
        {
            result = 91 - (65 - result);
        }

        return result;
    }
}