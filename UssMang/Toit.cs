public class Toit
{
    private Random rnd = new Random();
    private int ekraaniLaius;
    private int ekraaniKõrgus;
    public Punkt Asukoht { get; private set; }

    public Toit(int laius, int kõrgus)
    {
        ekraaniLaius = laius;
        ekraaniKõrgus = kõrgus;

    }

    // Genereerib uue toidu ja kontrollib, et see ei asuks ussi keha peal
    public void LooUusToit(List<Punkt> keha)
    {
        int x, y;
        bool sisseIlmunud;

        do
        {
            sisseIlmunud = false;
            // Genereerime suvalised koordinaadid ekraani piirides
         
            x = rnd.Next(1, (ekraaniLaius - 4) / 2) * 2;
            y = rnd.Next(1, ekraaniKõrgus - 2);

            // Kontrollime iga ussi keha punkti
            foreach (var p in keha)
            {
                if (p.X == x && p.Y == y)
                {
                    sisseIlmunud = true; // Koordinaadid kattuvad ussiga, proovime uuesti
                    break;
                }
            }
        } while (sisseIlmunud); // Tsükkel kordub, kuni leitakse vaba koht

        // Loome uue toidu punkti ja joonistame selle välja
        Asukoht = new Punkt(x, y, "🍎");
        Asukoht.Joonista();
    }
}