using System;
using System.Collections.Generic;
using System.Text;

namespace UssMang
{
  

    public class Uss2 : Uss
    {
        public Uss2(int algX, int algY, int pikkus)
            : base(algX, algY, pikkus,
                    "\x1b[48;2;255;80;80m",   
                    "\x1b[48;2;160;20;20m")  
        {
            PraeguneSuund = Suund.Vasakule;
        }
    }
    
}

