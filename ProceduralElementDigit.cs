using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemWriter
{
    internal struct ProceduralElementDigit
    {        
            public ProceduralElementDigit(int digit)
        {
            Digit = digit;
            Character = Digits[digit].Character;
            Name = Digits[digit].Name;
        }

        private ProceduralElementDigit(int digit, char character, string name)
        {
            Digit = digit;
            Character = character;
            Name = name;
        }

        public char Character { get; set; }
        public int Digit { get; set; }
        public string Name { get; set; }

        public static List<ProceduralElementDigit> Digits = new List<ProceduralElementDigit> {
            new ProceduralElementDigit(0, 'n', "nil"),
            new ProceduralElementDigit(1, 'u', "un"),
            new ProceduralElementDigit(2, 'b', "bi"),
            new ProceduralElementDigit(3, 't', "tri"),
            new ProceduralElementDigit(4, 'q', "quad"),
            new ProceduralElementDigit(5, 'p', "pent"),
            new ProceduralElementDigit(6, 'h', "hex"),
            new ProceduralElementDigit(7, 's', "sept"),
            new ProceduralElementDigit(8, 'o', "oct"),
            new ProceduralElementDigit(9, 'e', "enn")
        };
    }
}
