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
        try
        {


            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();
            Console.Write("Vali režiim (1-Lihtne, 2-Keskmine, 3-Raske, 4-2 mängijat, 5-AI): ");
            if (!int.TryParse(Console.ReadLine(), out int tase)) tase = 1;
            MänguSeaded seaded = new MänguSeaded(tase);
            Console.Clear();
            Console.SetWindowSize(seaded.Laius * 2 + 20, seaded.Kõrgus + 3);
            Console.SetBufferSize(seaded.Laius * 2 + 20, seaded.Kõrgus + 3);

            Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);
            kaart.Joonista(new Punkt(0, 0, ""));

            Uss uss1 = new Uss(10, 10, 3);
            Uss2 uss2 = seaded.KahesMängija ? new Uss2(seaded.Laius - 12, 10, 3) : null;
            UssBot bot = seaded.OnBot ? new UssBot(seaded.Laius - 12, 10, 3) : null;



            Toit toit = new Toit(seaded.Laius, seaded.Kõrgus);
            toit.LooUusToit(uss1.Keha);

            int skoor1 = 0;
            int skoor2 = 0;
            string võitja = "";

            while (true)
            {

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo klahv = Console.ReadKey(true);

                    if (klahv.Key == ConsoleKey.W && uss1.PraeguneSuund != Suund.Alla)
                        uss1.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.S && uss1.PraeguneSuund != Suund.Üles)
                        uss1.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.A && uss1.PraeguneSuund != Suund.Paremale)
                        uss1.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.D && uss1.PraeguneSuund != Suund.Vasakule)
                        uss1.PraeguneSuund = Suund.Paremale;


                    if (uss2 != null)
                    {
                        if (klahv.Key == ConsoleKey.UpArrow && uss2.PraeguneSuund != Suund.Alla)
                            uss2.PraeguneSuund = Suund.Üles;
                        else if (klahv.Key == ConsoleKey.DownArrow && uss2.PraeguneSuund != Suund.Üles)
                            uss2.PraeguneSuund = Suund.Alla;
                        else if (klahv.Key == ConsoleKey.LeftArrow && uss2.PraeguneSuund != Suund.Paremale)
                            uss2.PraeguneSuund = Suund.Vasakule;
                        else if (klahv.Key == ConsoleKey.RightArrow && uss2.PraeguneSuund != Suund.Vasakule)
                            uss2.PraeguneSuund = Suund.Paremale;
                    }
                }


                uss1.Liigu();
                bot?.TeraAI(toit.Asukoht, kaart);
                bot?.Liigu();
                uss2?.Liigu();

                Punkt pea1 = uss1.HangiPea();
                Punkt pea2 = uss2?.HangiPea();
                Punkt peaBot = bot?.HangiPea();



                // Kontrollime kokkupõrget kahe ussi vahel
                if (uss2 != null)
                {
                    bool uss1OstusTeis = uss2.Keha.Any(p => p.X == pea1.X && p.Y == pea1.Y);
                    bool uss2OstusTeis = uss1.Keha.Any(p => p.X == pea2.X && p.Y == pea2.Y);
                    bool peadPõrkasid = pea1.X == pea2.X && pea1.Y == pea2.Y;

                    if (uss1OstusTeis) { võitja = "Mängija 2 võitis!"; break; }
                    if (uss2OstusTeis || peadPõrkasid) { võitja = "Mängija 1 võitis!"; break; }
                }

                // Kontrollime kokkupõrget botiga
                if (bot != null)
                {
                    bool uss1OstusBotti = bot.Keha.Any(p => p.X == pea1.X && p.Y == pea1.Y);
                    bool botOstusUss1 = uss1.Keha.Any(p => p.X == peaBot.X && p.Y == peaBot.Y);
                    bool peadPõrkasid = pea1.X == peaBot.X && pea1.Y == peaBot.Y;

                    if (uss1OstusBotti) { võitja = "Bot võitis!"; break; }
                    if (botOstusUss1 || peadPõrkasid) { võitja = "Mängija 1 võitis!"; break; }
                }



              


                bool uss1Suri = kaart.OnSein(pea1.X, pea1.Y) || uss1.KasHammustasEnnast();

                bool uss2Suri = uss2 != null && (kaart.OnSein(pea2.X, pea2.Y) || uss2.KasHammustasEnnast());

                if (seaded.KahesMängija)
                {
                    if (uss1Suri) { võitja = "Mängija 2 võitis!"; break; }
                    if (uss2Suri) { võitja = "Mängija 1 võitis!"; break; }
                }
                else if (!seaded.OnBot)
                {
                    if (uss1Suri) { break; }
                }
                else // bot režiim
                {
                    bool botSuri = bot != null && (kaart.OnSein(peaBot.X, peaBot.Y) || bot.KasHammustasEnnast());
                    if (uss1Suri) { võitja = "Bot võitis!"; break; }
                    if (botSuri) { võitja = "Mängija 1 võitis!"; break; }
                }

                // söök
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

                // Boti toidusöömine
                if (bot != null && peaBot.X == toit.Asukoht.X && peaBot.Y == toit.Asukoht.Y)
                {
                    skoor2 += 10;
                    bot.Kasva();
                    toit.LooUusToit(uss1.Keha);
                }


                Console.SetCursorPosition(seaded.Laius + 2, 2);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"Mängija 1: {skoor1}    ");
                if (seaded.KahesMängija)
                {
                    Console.SetCursorPosition(seaded.Laius + 2, 3);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Mängija 2: {skoor2}    ");
                }
                if (seaded.OnBot)
                {
                    Console.SetCursorPosition(seaded.Laius + 2, 3);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"Bot: {skoor2}    ");
                }
                Console.ResetColor();
                Console.ResetColor();

                Thread.Sleep(seaded.KiirusMS);
            }

            // lõpp
            Console.Clear();
            Console.Clear();
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (!string.IsNullOrEmpty(võitja))
                Console.WriteLine(võitja);
            Console.ForegroundColor = ConsoleColor.Red;
            if (seaded.KahesMängija || seaded.OnBot)
                Console.WriteLine($"MÄNG LÄBI! M1: {skoor1}  M2/Bot: {skoor2}");
            else
                Console.WriteLine($"MÄNG LÄBI! Skoor: {skoor1}");
            Console.ResetColor();

        }

        catch (Exception e)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Viga: " + e.Message);
            Console.WriteLine(e.StackTrace);
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}