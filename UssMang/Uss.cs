using System.Collections.Generic;
using System.Linq;

public class Uss
{
    private List<Punkt> keha = new List<Punkt>();
    public Suund PraeguneSuund { get; set; }
    public List<Punkt> Keha => keha;

    public Uss(int algX, int algY, int pikkus)
    {
        PraeguneSuund = Suund.Paremale; // Alguses liigub paremale

        // Loome ussi algse keha
        for (int i = 0; i < pikkus; i++)
        {
            Punkt p = new Punkt(algX - i, algY, '*');
            keha.Add(p);
            p.Joonista();

        }
    }

    public void Liigu()
    {
        // 1. Leiame praeguse pea asukoha
        Punkt pea = keha.First();
        Punkt uusPea = new Punkt(pea.X, pea.Y, '*');

        // 2. Arvutame uue pea asukoha vastavalt suunale
        switch (PraeguneSuund)
        {
            case Suund.Paremale: uusPea.X++; break;
            case Suund.Vasakule: uusPea.X--; break;
            case Suund.Alla: uusPea.Y++; break;
            case Suund.Üles: uusPea.Y--; break;
        }

        // 3. Lisame uue pea listi algusesse ja joonistame
        keha.Insert(0, uusPea);
        uusPea.Joonista();

        // 4. Kustutame sabaotsa (viimase elemendi), et simuleerida liikumist
        Punkt saba = keha.Last();
        saba.Kustuta();
        keha.Remove(saba);
    }

    public Punkt HangiPea()
    {
        return keha.First();
    }

    public void Kasva()
    {
        // Kasvamine on lihtne - lisame saba lõppu lihtsalt uue nähtamatu punkti, 
        // mis järgmise liikumise ajal asendab päris saba.
        // Lihtsuse mõttes võime lihtsalt järgmisel liikumisel saba mitte kustutada!
        // Siin lisame lihtsalt ajutise koopia viimasest elemendist.
        keha.Add(new Punkt(keha.Last().X, keha.Last().Y, '*'));
    }

    // Kontrollib, kas ussi pea põrkas kokku mõne tema kehaosaga
    public bool KasHammustasEnnast()
    {
        Punkt pea = HangiPea();

        // Jätame esimene elemendi (pea) vahele ja kontrollime ülejäänud kehaosi
        for (int i = 1; i < keha.Count; i++)
        {
            if (pea.X == keha[i].X && pea.Y == keha[i].Y)
            {
                return true; // Uss hammustas ennast
            }
        }
        return false; // Kõik on korras, uss ei hammustanud ennast
    }
}