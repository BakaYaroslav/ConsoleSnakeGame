// Määrab ära 4 võimalikku liikumissuunda
public enum Suund
{
    Üles, Alla, Vasakule, Paremale
}

public class Punkt
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Sümbol { get; set; } // Näiteks '*' ussi jaoks, '@' toidu jaoks

    public Punkt(int x, int y, string sümbol)
    {
        X = x;
        Y = y;
        Sümbol = sümbol;
    }

    // See meetod joonistab punkti konsooli õigesse kohta
    public void Joonista()
    {
        Console.SetCursorPosition(X, Y);
        if ((X / 2 + Y) % 2 == 0)
            Console.BackgroundColor = ConsoleColor.DarkGreen;
        else
            Console.BackgroundColor = ConsoleColor.Green;
        Console.Write("  "); 
        Console.SetCursorPosition(X, Y);
        Console.Write(Sümbol);
        Console.ResetColor();
    }

    // Puhastab eelmise asukoha (kui uss liigub edasi)
    public void Kustuta()
    {
        Console.SetCursorPosition(X, Y);
        Console.Write(' '); // Kirjutame tühiku peale
    }
}