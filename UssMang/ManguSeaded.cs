public class MänguSeaded
{
    public int Laius { get; set; }
    public int Kõrgus { get; set; }
    public int KiirusMS { get; set; }
    public bool KahesMängija { get; set; }

    public MänguSeaded(int tase)
    {
        KahesMängija = false;
        switch (tase)
        {
            case 1: KiirusMS = 200; Laius = 40; Kõrgus = 20; break;
            case 2: KiirusMS = 100; Laius = 40; Kõrgus = 20; break;
            case 3: KiirusMS = 50; Laius = 40; Kõrgus = 20; break;
            case 4: KiirusMS = 100; Laius = 50; Kõrgus = 25; KahesMängija = true; break;
            default: KiirusMS = 150; Laius = 40; Kõrgus = 20; break;
        }
    }
}