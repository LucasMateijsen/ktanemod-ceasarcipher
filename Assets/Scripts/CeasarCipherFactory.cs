public class CeasarCipherFactory
{
	private static CeasarCipherFactory _instance;

	public static CeasarCipherFactory Instance
	{
		get { return _instance ?? (_instance = new CeasarCipherFactory()); }
	}

	public CeasarCipherPuzzle GenerateQuestion()
	{
        var puzzle = new CeasarCipherPuzzle ();
		return puzzle;
	}
}