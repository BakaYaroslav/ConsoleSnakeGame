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
        // 1. Mängu algus ja seadistamine
        Console.CursorVisible = false;
        Console.Clear();
        Console.Write("Vali raskus (1-3): ");
        if (!int.TryParse(Console.ReadLine(), out int tase)) tase = 1;

        MänguSeaded seaded = new MänguSeaded(tase);
        Console.SetWindowSize(seaded.Laius * Kaart.CW, seaded.Kõrgus + 3);
        Console.SetBufferSize(seaded.Laius * Kaart.CW, seaded.Kõrgus + 3);

        Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);
        Uss uss = new Uss(10, 10, 3);
        Toit toit = new Toit(seaded.Laius, seaded.Kõrgus);
        int skoor = 0;

        kaart.Joonista();
        // Joonistame kaardi (välisseinad)
        kaart.Joonista();

        toit.LooUusToit(uss.Keha);
        // 2. Mängu peatsükkel (Game Loop)
        while (true)
        {
            // Sisendi lugemine (kas kasutaja vajutas nooleklahvi?)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo klahv = Console.ReadKey(true);
                // Muudame suunda, aga väldime tagurdamist
                if (klahv.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla)
                    uss.PraeguneSuund = Suund.Üles;
                else if (klahv.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles)
                    uss.PraeguneSuund = Suund.Alla;
                else if (klahv.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale)
                    uss.PraeguneSuund = Suund.Vasakule;
                else if (klahv.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule)
                    uss.PraeguneSuund = Suund.Paremale;
            }

            // Objektide uuendamine
            uss.Liigu();
            Punkt pea = uss.HangiPea();

            // Mängu lõpu kontroll
            if (kaart.OnSein(pea.X, pea.Y) || uss.KasHammustasEnnast())
            {
                break;
            }



            // Toidu söömine
            if (pea.X == toit.Asukoht.X && pea.Y == toit.Asukoht.Y)
            {
                skoor += 10;
                uss.Kasva();
                toit.LooUusToit(uss.Keha); // Uus toit luuakse ainult SIIN, kui uss reaalselt sõi
            }

            // Kuvame skoori reaalajas (akna ülanurgas raami sees)
            Console.SetCursorPosition(2, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" Skoor: {skoor} ");
            Console.ResetColor();

            // Mängu kiirus vastavalt seadetele
            Thread.Sleep(seaded.KiirusMS);
        }

        // 3. Pärast mängu lõppu
        Console.Clear();
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"MÄNG LÄBI! Sinu lõplik skoor: {skoor}");
        Console.ResetColor();

        Console.Write("Sisesta oma nimi edetabeli jaoks: ");
        string nimi = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nimi)) nimi = "Mängija";

        // Salvestamine ja edetabeli kuvamine
        Edetabel.Salvesta(nimi, skoor);
        Edetabel.KuvaEdetabel();

        Console.ReadLine();
    }
}