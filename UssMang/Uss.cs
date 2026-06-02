using System.Collections.Generic;
using System.Linq;

public class Uss
{
    private List<Punkt> keha = new List<Punkt>();
    public Suund PraeguneSuund { get; set; }
    public List<Punkt> Keha => keha;
    private string colorHead;
    private string colorBody;

    public Uss(int algX, int algY, int pikkus, string headColor = "\x1b[48;2;200;220;255m", string bodyColor = "\x1b[48;2;80;120;220m")
    {
        colorHead = headColor;
        colorBody = bodyColor;
        PraeguneSuund = Suund.Paremale;
        for (int i = 0; i < pikkus; i++)
        {
            Punkt p = new Punkt(algX - i * 2, algY, "██");
            keha.Add(p);
            Console.SetCursorPosition(p.X, p.Y);
            Console.Write(colorBody + "  \x1b[0m");
        }
    }


    private string GetSnakeColor(int index, int total)
    {
        int t = total <= 1 ? 0 : index * 100 / total;
        int r = 30 + (170 * (100 - t) / 100);
        int g = 100 + (50 * (100 - t) / 100);
        int b = 255;
        return $"\x1b[48;2;{r};{g};{b}m";
    }

    private string GetBgColor(int x, int y)
    {
        if ((x / 2 + y) % 2 == 0)
            return "\x1b[48;2;85;139;47m";  // тёмно-зелёный
        else
            return "\x1b[48;2;100;160;55m"; // светло-зелёный
    }

   

    private void JoonistaMlõik()
    {
        for (int i = 0; i < keha.Count; i++)
        {
            Console.SetCursorPosition(keha[i].X, keha[i].Y);
            Console.Write(GetSnakeColor(i, keha.Count) + "  \x1b[0m");
        }
    }

    public void Liigu()
    {
        Punkt pea = keha.First();
        Punkt uusPea = new Punkt(pea.X, pea.Y, "██");

        switch (PraeguneSuund)
        {
            case Suund.Paremale: uusPea.X += 2; break;
            case Suund.Vasakule: uusPea.X -= 2; break;
            case Suund.Alla:     uusPea.Y += 1; break;
            case Suund.Üles:     uusPea.Y -= 1; break;
        }

        keha.Insert(0, uusPea);

        // Рисуем всю змейку с градиентом
        JoonistaMlõik();

        // Стираем хвост
        Punkt saba = keha.Last();
        Console.SetCursorPosition(saba.X, saba.Y);
        Console.Write(GetBgColor(saba.X, saba.Y) + "  \x1b[0m");
        keha.Remove(saba);
    }

    public Punkt HangiPea() => keha.First();

    public void Kasva()
    {
        keha.Add(new Punkt(keha.Last().X, keha.Last().Y, "██"));
    }

    public bool KasHammustasEnnast()
    {
        Punkt pea = HangiPea();
        for (int i = 1; i < keha.Count; i++)
            if (pea.X == keha[i].X && pea.Y == keha[i].Y)
                return true;
        return false;
    }
}