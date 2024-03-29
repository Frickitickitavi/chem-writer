﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemWriter
{
    internal class RealElement : Element
    {
        public RealElement(UInt64 atomicNumber, string symbol, string name)
        {
            Symbol = symbol;
            Name = name;
            AtomicNumber = atomicNumber;
        }

        public bool IsNeutronium { get; set; }

        public static List<RealElement> Table = new List<RealElement> {
            new RealElement(1, "H", "Hydrogen"),
            new RealElement(2, "He", "Helium"),
            new RealElement(3, "Li", "Lithium"),
            new RealElement(4, "Be", "Beryllium"),
            new RealElement(5, "B", "Boron"),
            new RealElement(6, "C", "Carbon"),
            new RealElement(7, "N", "Nitrogen"),
            new RealElement(8, "O", "Oxygen"),
            new RealElement(9, "F", "Fluorine"),
            new RealElement(10, "Ne", "Neon"),
            new RealElement(11, "Na", "Sodium"),
            new RealElement(12, "Mg", "Magnesium"),
            new RealElement(13, "Al", "Aluminum"),
            new RealElement(14, "Si", "Silicon"),
            new RealElement(15, "P", "Phosphorus"),
            new RealElement(16, "S", "Sulfur"),
            new RealElement(17, "Cl", "Chlorine"),
            new RealElement(18, "Ar", "Argon"),
            new RealElement(19, "K", "Potassium"),
            new RealElement(20, "Ca", "Calcium"),
            new RealElement(21, "Sc", "Scandium"),
            new RealElement(22, "Ti", "Titanium"),
            new RealElement(23, "V", "Vanadium"),
            new RealElement(24, "Cr", "Chromium"),
            new RealElement(25, "Mn", "Manganese"),
            new RealElement(26, "Fe", "Iron"),
            new RealElement(27, "Co", "Cobalt"),
            new RealElement(28, "Ni", "Nickel"),
            new RealElement(29, "Cu", "Copper"),
            new RealElement(30, "Zn", "Zinc"),
            new RealElement(31, "Ga", "Gallium"),
            new RealElement(32, "Ge", "Germanium"),
            new RealElement(33, "As", "Arsenic"),
            new RealElement(34, "Se", "Selenium"),
            new RealElement(35, "Br", "Bromine"),
            new RealElement(36, "Kr", "Krypton"),
            new RealElement(37, "Rb", "Rubidium"),
            new RealElement(38, "Sr", "Strontium"),
            new RealElement(39, "Y", "Yttrium"),
            new RealElement(40, "Zr", "Zirconium"),
            new RealElement(41, "Nb", "Nobelium"),
            new RealElement(42, "Mo", "Molybdenum"),
            new RealElement(43, "Tc", "Technetium"),
            new RealElement(44, "Ru", "Ruthenium"),
            new RealElement(45, "Rh", "Rhodium"),
            new RealElement(46, "Pd", "Palladium"),
            new RealElement(47, "Ag", "Silver"),
            new RealElement(48, "Cd", "Cadmium"),
            new RealElement(49, "In", "Indium"),
            new RealElement(50, "Sn", "Tin"),
            new RealElement(51, "Sb", "Antimony"),
            new RealElement(52, "Te", "Tellerium"),
            new RealElement(53, "I", "Iodine"),
            new RealElement(54, "Xe", "Xenon"),
            new RealElement(55, "Cs", "Caesium"),
            new RealElement(56, "Ba", "Barium"),
            new RealElement(57, "La", "Lanthanum"),
            new RealElement(58, "Ce", "Cerium"),
            new RealElement(59, "Pr", "Praseodymium"),
            new RealElement(60, "Nd", "Neodymium"),
            new RealElement(61, "Pm", "Promethium"),
            new RealElement(62, "Sm", "Samarium"),
            new RealElement(63, "Eu", "Europium"),
            new RealElement(64, "Gd", "Gadolinium"),
            new RealElement(65, "Tb", "Terbium"),
            new RealElement(66, "Dy", "Dysprosium"),
            new RealElement(67, "Ho", "Holmium"),
            new RealElement(68, "Er", "Erbium"),
            new RealElement(69, "Tm", "Thulium"),
            new RealElement(70, "Yb", "Ytterbium"),
            new RealElement(71, "Lu", "Lutetium"),
            new RealElement(72, "Hf", "Hafnium"),
            new RealElement(73, "Ta", "Tantalum"),
            new RealElement(74, "W", "Tungsten"),
            new RealElement(75, "Re", "Rhenium"),
            new RealElement(76, "Os", "Osmium"),
            new RealElement(77, "Ir", "Iridium"),
            new RealElement(78, "Pt", "Platinum"),
            new RealElement(79, "Au", "Gold"),
            new RealElement(80, "Hg", "Mercury"),
            new RealElement(81, "Tl", "Thallium"),
            new RealElement(82, "Pb", "Lead"),
            new RealElement(83, "Bi", "Bismuth"),
            new RealElement(84, "Po", "Polonium"),
            new RealElement(85, "At", "Astatine"),
            new RealElement(86, "Rn", "Radon"),
            new RealElement(87, "Fr", "Francium"),
            new RealElement(88, "Ra", "Radium"),
            new RealElement(89, "Ac", "Actinium"),
            new RealElement(90, "Th", "Thorium"),
            new RealElement(91, "Pa", "Protactinium"),
            new RealElement(92, "U", "Uranium"),
            new RealElement(93, "Np", "Neptunium"),
            new RealElement(94, "Pu", "Plutonium"),
            new RealElement(95, "Am", "Americium"),
            new RealElement(96, "Cm", "Curium"),
            new RealElement(97, "Bk", "Berkelium"),
            new RealElement(98, "Cf", "Californium"),
            new RealElement(99, "Es", "Einsteinium"),
            new RealElement(100, "Fm", "Fermium"),
            new RealElement(101, "Md", "Mendelevium"),
            new RealElement(102, "No", "Nobelium"),
            new RealElement(103, "Lr", "Lawrencium"),
            new RealElement(104, "Rf", "Rutherfordium"),
            new RealElement(105, "Db", "Dubnium"),
            new RealElement(106, "Sg", "Seaborgium"),
            new RealElement(107, "Bh", "Bohrium"),
            new RealElement(108, "Hs", "Hassium"),
            new RealElement(109, "Mt", "Meitnerium"),
            new RealElement(110, "Ds", "Darmstadtium"),
            new RealElement(111, "Rg", "Roentgenium"),
            new RealElement(112, "Cn", "Copernicium"),
            new RealElement(113, "Nh", "Nihonium"),
            new RealElement(114, "Fl", "Flerovium"),
            new RealElement(115, "Mc", "Moscovium"),
            new RealElement(116, "Lv", "Livermorium"),
            new RealElement(117, "Ts", "Tennessine"),
            new RealElement(118, "Og", "Oganesson")
        };
    }
}
