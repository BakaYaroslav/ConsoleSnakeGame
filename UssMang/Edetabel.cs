public static class Edetabel
{
    private static string failiTee = "skoorid.txt";
    private static string failiTee2 = "skoorid2.txt";

    public static void Salvesta(string nimi, int skoor, bool kahesMängija = false)
    {
        string fail = kahesMängija ? failiTee2 : failiTee;
        File.AppendAllLines(fail, new[] { $"{nimi};{skoor}" });
    }

    public static void KuvaEdetabel(bool kahesMängija = false)
    {
        string fail = kahesMängija ? failiTee2 : failiTee;
        if (!File.Exists(fail)) return;

        var skoorid = File.ReadAllLines(fail)
            .Select(rida => rida.Split(';'))
            .Where(osad => osad.Length == 2)
            .Select(osad => new { Nimi = osad[0], Punktid = int.Parse(osad[1]) })
            .OrderByDescending(x => x.Punktid)
            .Take(5);

        Console.Clear();
        Console.WriteLine(kahesMängija ? "--- TOP 5 (2 MÄNGIJAT) ---" : "--- TOP 5 EDETABEL ---");
        foreach (var s in skoorid)
            Console.WriteLine($"{s.Nimi}: {s.Punktid}");
    }
}