using System;
using System.Collections.Generic;
using System.Text;

namespace UssMang
{
    public class UssBot : Uss
    {
        public UssBot(int algX, int algY, int pikkus)
            : base(algX, algY, pikkus,
                   "\x1b[48;2;255;80;80m",
                   "\x1b[48;2;150;20;20m")
        {
            PraeguneSuund = Suund.Vasakule;
        }

        // Bot liigub automaatselt toidu poole
        public void TeraAI(Punkt toit, Kaart kaart)
        {
            Punkt pea = HangiPea();

            int dx = toit.X - pea.X;
            int dy = toit.Y - pea.Y;

            // Eelistame liikuda toidu suunas
            Suund soovitud = Math.Abs(dx) > Math.Abs(dy)
                ? (dx > 0 ? Suund.Paremale : Suund.Vasakule)
                : (dy > 0 ? Suund.Alla : Suund.Üles);

            // Proovime suundi järjekorras kuni leiame ohutu
            var jarjekord = new List<Suund> { soovitud, Suund.Üles, Suund.Alla, Suund.Vasakule, Suund.Paremale };
            foreach (var s in jarjekord)
            {
                if (OnOhutu(s, kaart))
                {
                    PraeguneSuund = s;
                    break;
                }
            }
        }

        private bool OnOhutu(Suund s, Kaart kaart)
        {
            Punkt pea = HangiPea();
            int nx = pea.X + (s == Suund.Paremale ? 2 : s == Suund.Vasakule ? -2 : 0);
            int ny = pea.Y + (s == Suund.Alla ? 1 : s == Suund.Üles ? -1 : 0);
            if (kaart.OnSein(nx, ny)) return false;
            if (Keha.Any(p => p.X == nx && p.Y == ny)) return false;
            return true;
        }
    }
}
