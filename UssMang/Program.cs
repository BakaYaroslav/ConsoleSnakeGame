using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UssMang;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        Console.Write("Vali režiim (1-Lihtne, 2-Keskmine, 3-Raske, 4-2 mängijat): ");
        if (!int.TryParse(Console.ReadLine(), out int tase)) tase = 1;
        MänguSeaded seaded = new MänguSeaded(tase);
        Console.Clear();
        Console.SetWindowSize(seaded.Laius * 2 + 20, seaded.Kõrgus + 3);
        Console.SetBufferSize(seaded.Laius * 2 + 20, seaded.Kõrgus + 3);

        Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);
        kaart.Joonista(new Punkt(0, 0, ""));

        Uss uss1 = new Uss(10, 10, 3);
        Uss2 uss2 = seaded.KahesMängija ? new Uss2(seaded.Laius - 12, 10, 3) : null;



        Toit toit = new Toit(seaded.Laius, seaded.Kõrgus);
        toit.LooUusToit(uss1.Keha);

        int skoor1 = 0;
        int skoor2 = 0;

        while (true)
        {
           
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo klahv = Console.ReadKey(true);

                if (klahv.Key == ConsoleKey.UpArrow && uss1.PraeguneSuund != Suund.Alla)
                    uss1.PraeguneSuund = Suund.Üles;
                else if (klahv.Key == ConsoleKey.DownArrow && uss1.PraeguneSuund != Suund.Üles)
                    uss1.PraeguneSuund = Suund.Alla;
                else if (klahv.Key == ConsoleKey.LeftArrow && uss1.PraeguneSuund != Suund.Paremale)
                    uss1.PraeguneSuund = Suund.Vasakule;
                else if (klahv.Key == ConsoleKey.RightArrow && uss1.PraeguneSuund != Suund.Vasakule)
                    uss1.PraeguneSuund = Suund.Paremale;

             
                if (uss2 != null)
                {
                    if (klahv.Key == ConsoleKey.W && uss2.PraeguneSuund != Suund.Alla)
                        uss2.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.S && uss2.PraeguneSuund != Suund.Üles)
                        uss2.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.A && uss2.PraeguneSuund != Suund.Paremale)
                        uss2.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.D && uss2.PraeguneSuund != Suund.Vasakule)
                        uss2.PraeguneSuund = Suund.Paremale;
                }
            }

            uss1.Liigu();
            uss2?.Liigu();

            // Kontrollime kokkupõrget kahe ussi vahel
            if (uss2 != null)
            {
                Punkt p1 = uss1.HangiPea();
                Punkt p2 = uss2.HangiPea();

                // Uss1 pea põrkas uss2 kehasse
                bool uss1OstusTeis = uss2.Keha.Any(p => p.X == p1.X && p.Y == p1.Y);
                // Uss2 pea põrkas uss1 kehasse
                bool uss2OstusTeis = uss1.Keha.Any(p => p.X == p2.X && p.Y == p2.Y);
                // Pead põrkasid otsa kokku
                bool peadPõrkasid = p1.X == p2.X && p1.Y == p2.Y;

                if (uss1OstusTeis || uss2OstusTeis || peadPõrkasid) break;
            }

            Punkt pea1 = uss1.HangiPea();
            Punkt pea2 = uss2?.HangiPea();

         
            bool uss1Suri = kaart.OnSein(pea1.X, pea1.Y) || uss1.KasHammustasEnnast();
         
            bool uss2Suri = uss2 != null && (kaart.OnSein(pea2.X, pea2.Y) || uss2.KasHammustasEnnast());

            if (seaded.KahesMängija)
            {
                if (uss1Suri || uss2Suri) break;
            }
            else
            {
                if (uss1Suri) break;
            }

            // Еда
            if (pea1.X == toit.Asukoht.X && pea1.Y == toit.Asukoht.Y)
            {
                skoor1 += 10;
                uss1.Kasva();
                toit.LooUusToit(uss1.Keha);
            }
            if (uss2 != null && pea2.X == toit.Asukoht.X && pea2.Y == toit.Asukoht.Y)
            {
                skoor2 += 10;
                uss2.Kasva();
                toit.LooUusToit(uss2.Keha);
            }

            // Счёт
            Console.SetCursorPosition(seaded.Laius + 2, 2);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Mängija 1: {skoor1}    ");
            if (seaded.KahesMängija)
            {
                Console.SetCursorPosition(seaded.Laius + 2, 3);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Mängija 2: {skoor2}    ");
            }
            Console.ResetColor();

            Thread.Sleep(seaded.KiirusMS);
        }

        // Конец игры
        Console.Clear();
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(0, 0);
        Console.SetCursorPosition(0, 0);
      
        if (seaded.KahesMängija)
            Console.WriteLine($"MÄNG LÄBI! M1: {skoor1}  M2: {skoor2}");

        else
            Console.WriteLine($"MÄNG LÄBI! Skoor: {skoor1}");
        Console.ResetColor();
        Console.Write("Sisesta oma nimi edetabeli jaoks: ");
        string nimi = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nimi)) nimi = "Mängija";
        Edetabel.Salvesta(nimi, skoor1);
        Edetabel.KuvaEdetabel();
        Console.ReadLine();
    }
}
