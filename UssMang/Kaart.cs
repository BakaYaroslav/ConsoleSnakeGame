using System.Drawing;

public class Kaart
{
    // List takistuste (seinte) hoidmiseks
    public List<Punkt> Takistused { get; private set; } = new List<Punkt>();

    private int laius;
    private int kõrgus;

    public Kaart(int laius, int kõrgus)
    {
        this.laius = laius;
        this.kõrgus = kõrgus;

        // Veendume, et laius on paarisarv, et ruudustik ja sammud klapiksid
        if (this.laius % 2 != 0) this.laius++;

        // Loome ülemise ja alumise seina
        // Horisontaalselt liigume sammuga 2, sest kasutame topeltsümbolit "##"
        for (int x = 0; x < this.laius; x += 2)
        {
            Takistused.Add(new Punkt(x, 0, "##"));
            Takistused.Add(new Punkt(x, this.kõrgus - 1, "##"));
        }

        // Loome vasaku ja parema seina
        for (int y = 1; y < this.kõrgus - 1; y++)
        {
            Takistused.Add(new Punkt(0, y, "##"));
            Takistused.Add(new Punkt(this.laius - 2, y, "##")); // Laius - 2, sest seina laius on 2 sümbolit
        }
    }

    // Meetod seina kontrollimiseks (lahendab errorilistis olnud 'OnSein' vea)
    public bool OnSein(int x, int y)
    {
        // Kontrollime, kas ussi pea koordinaadid kattuvad mõne seina punktiga
        // Kuna sein on 2 sümbolit lai (x ja x+1), kontrollime mõlemat ruutu
        foreach (var p in Takistused)
        {
            if ((p.X == x || p.X + 1 == x) && p.Y == y)
            {
                return true;
            }
        }
        return false;
    }


    public void Joonista(Punkt Asukoht)
    {
      
        for (int y = 1; y < kõrgus - 1; y++)
        {
            for (int x = 2; x < laius - 2; x += 2)
            {
                Console.SetCursorPosition(x, y);
                if ((x / 2 + y) % 2 == 0)
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                else
                    Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("  ");
            }
        }
        Console.ResetColor();

        // Рисуем стены
        foreach (var p in Takistused)
        {
            Console.SetCursorPosition(p.X, p.Y);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  ");
            Console.ResetColor();
        }
    }
}