using System.Collections.Generic;
using System.Linq;

public class Uss
{
    private List<Punkt> keha = new List<Punkt>();
    public Suund PraeguneSuund { get; set; }
    public List<Punkt> Keha => keha;
    private string colorHead;
    private string colorBody;

    public Uss(int algX, int algY, int pikkus,
    string headColor = "\x1b[48;2;100;180;255m",
    string bodyColor = "\x1b[48;2;20;60;180m")
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


   

    private string GetBgColor(int x, int y)
    {
        if ((x / 2 + y) % 2 == 0)
            return "\x1b[48;2;85;139;47m";  // тёмно-зелёный
        else
            return "\x1b[48;2;100;160;55m"; // светло-зелёный
    }



    private int[] ParseAnsi(string ansi)
    {
        var s = ansi.Replace("\x1b[48;2;", "").Replace("m", "");
        var parts = s.Split(';');
        return new int[] { int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]) };
    }

    private string GetSnakeColor(int index, int total)
    {
        int t = total <= 1 ? 0 : index * 100 / total;
        int[] head = ParseAnsi(colorHead);
        int[] body = ParseAnsi(colorBody);
        int r = head[0] + (body[0] - head[0]) * t / 100;
        int g = head[1] + (body[1] - head[1]) * t / 100;
        int b = head[2] + (body[2] - head[2]) * t / 100;
        return $"\x1b[48;2;{r};{g};{b}m";
    }

    public Punkt HangiPea() => keha.First();

    public void Kasva()
    {
        keha.Add(new Punkt(keha.Last().X, keha.Last().Y, "██"));
    }

    public bool KasHammustasEnnast()
    {
        if (keha.Count < 4) return false; // liiga lühike, et hammustada
        Punkt pea = HangiPea();
        for (int i = 1; i < keha.Count; i++)
            if (pea.X == keha[i].X && pea.Y == keha[i].Y)
                return true;
        return false;
    }


    private void JoonistaSilmad()
    {
        Punkt pea = keha.First();
        Console.SetCursorPosition(pea.X, pea.Y);
        Console.Write(colorHead);
        string silmad = PraeguneSuund switch
        {
            Suund.Paremale => " ◉",
            Suund.Vasakule => "◉ ",
            Suund.Üles => "◉◉",
            Suund.Alla => "◉◉",
        };
        Console.Write(colorHead + "\x1b[38;2;0;0;0m" + silmad + "\x1b[0m");
    }



    public void Liigu()
    {
        Punkt pea = keha.First();
        Punkt uusPea = new Punkt(pea.X, pea.Y, "██");

        switch (PraeguneSuund)
        {
            case Suund.Paremale: uusPea.X += 2; break;
            case Suund.Vasakule: uusPea.X -= 2; break;
            case Suund.Alla: uusPea.Y += 1; break;
            case Suund.Üles: uusPea.Y -= 1; break;
        }


        Punkt saba = keha.Last();
        Console.SetCursorPosition(saba.X, saba.Y);
        Console.Write(GetBgColor(saba.X, saba.Y) + "  \x1b[0m");
        keha.Remove(saba);

        keha.Insert(0, uusPea);


        for (int i = 0; i < keha.Count; i++)
        {
            Console.SetCursorPosition(keha[i].X, keha[i].Y);
            Console.Write(GetSnakeColor(i, keha.Count) + "  \x1b[0m");
        }
        JoonistaSilmad();
    }

}