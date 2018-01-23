using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Autoverseny
{
    
    class Program
    {

        static byte round;
        
        public static Random vel = new Random();

        public static StreamReader sr;


        static void helyCsere(List<Versenyzo> pilotak, byte startIndex, byte pilots, byte xCount)
        {
            ///byte pLength = (byte)pilotak.Where(p => p.getVersenyben()).Count();   --> xCount !!

            byte xPos = (byte)(xCount - startIndex);
            byte nCount = xCount;

            Console.WriteLine($"\n{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }..");

            for (int i = startIndex; i > startIndex - pilots && xPos > 0; i--)
            {
                pilotak[pilotak.FindIndex(y => y.getRank() == i)].setBox(pilotak, (byte)(xPos + 1), nCount);
                Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }");

                nCount -= 1;    // => A kiesés miatt előzőleg hátrább kerültek mögé már nem lehet kerülni a sorban!! :)

            }
            Console.WriteLine();

        }

        protected class Auto
        {

            string fuel;
            byte tank;

            public Auto(string _fuel, byte _tank)
            {
                fuel = _fuel;

                if (_tank > 100) tank = 100;
                else tank = _tank;

            }

            public void setTank(byte fogyasztas)
            {
                if (fogyasztas == 0) tank = 100;
                else if (tank >= fogyasztas) tank -= fogyasztas;
                else tank = 0;

            }

            public byte getTank()
            {
                return tank;
            }

            public string getFuel()
            {
                return fuel;
            }
        
        }

        protected abstract class Pilota
        {
            protected byte elozesKor;
            protected double elozesiEsely;
            protected byte boxFuel;

            public byte getElozesKor()
            {
                return elozesKor;
            }

            public double getElozesiEsely()
            {
                return elozesiEsely;
            }

            public byte getBoxFuel()
            {
                return boxFuel;
            }

        }

        protected class AgresszivPilota : Versenyzo
        {
            public AgresszivPilota(string _nev, bool _versenyben, byte _rank, Auto _auto) : this()
            {
                nev = _nev;
                versenyben = _versenyben;
                rank = _rank;

                auto = _auto;

            }

            public AgresszivPilota()
            {
                elozesKor = 2;
                elozesiEsely = 100 / 3.0;
                boxFuel = 10;

            }
            
        }

        protected class LenduletesPilota : Versenyzo
        {
            public LenduletesPilota(string _nev, bool _versenyben, byte _rank, Auto _auto) : this()
            {
                nev = _nev;
                versenyben = _versenyben;
                rank = _rank;

                auto = _auto;

            }

            public LenduletesPilota()
            {
                elozesKor = 5;
                elozesiEsely = 100 / 2.0;
                boxFuel = 20;

            }

        }

        protected class VeszelyesPilota : Versenyzo
        {
            public VeszelyesPilota(string _nev, bool _versenyben, byte _rank, Auto _auto) : this()
            {
                nev = _nev;
                versenyben = _versenyben;
                rank = _rank;

                auto = _auto;

            }

            public VeszelyesPilota()
            {
                elozesKor = 4;
                elozesiEsely = 100 / 4.0;
                boxFuel = 5;
                
            }

        }

        protected class OvatosPilota : Versenyzo
        {
            public OvatosPilota(string _nev, bool _versenyben, byte _rank, Auto _auto) : this()
            {
                nev = _nev;
                versenyben = _versenyben;
                rank = _rank;

                auto = _auto;

            }
    
            public OvatosPilota()
            {
                elozesKor = 0;
                elozesiEsely = 0;
                boxFuel = 20;

            }

        }

        protected abstract class Versenyzo : Pilota
        {
            protected string nev;
            protected bool versenyben;
            protected byte rank;
            
            protected Auto auto;
            

            public Versenyzo()
            {
                nev = "";
                versenyben = true;
                rank = Byte.MaxValue;
                auto = new Auto("benzin", 100);

            }

            public string getNev()
            {
                return nev;
            }

            public bool getVersenyben()
            {
                return versenyben;
            }

            public byte getRank()
            {
                return rank;
            }

            public byte getCarFuel()
            {
                return auto.getTank();
            }

            public void setVersenyben()
            {
                versenyben = !versenyben;
            }

            public void setRank(short rankMod)
            {
                if (rankMod < 0) rank -= (byte)Math.Abs(rankMod);
                else rank += (byte)Math.Abs(rankMod);

            }

            public void setRound()
            {
                auto.setTank(5);                

            }

            public void setBox(List<Versenyzo> pilotak, byte lossPos, byte xCount)
            {
                int n = 0;
                while (n < lossPos && rank + 1 <= xCount)
                {

                    pilotak[pilotak.FindIndex(y => y.getRank() == rank + 1)].setRank(-1);
                    setRank(1);   // ==>  lényegében ugyanaz mint: rank += 1
                    n++;

                }

                auto.setTank(0);

            }

            public void setCross(List<Versenyzo> pilotak)
            {
                
                if (esemeny(getElozesiEsely()))
                {
                    int x = getRank();
                    x -= 1;

                    byte xCount = (byte)pilotak.Where(p => p.getVersenyben()).Count();

                    //Baleset történik-e? ->

                    double rand = vel.Next(100000) / 1000.0;

                    if ((elozesKor != 4 && rand < 4) || (elozesKor == 4 && rand < 8))
                    {
                        versenyben = false;
                        
                        Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {nev}. KIESETT a(z) {x}. helyen álló {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} -nak a beelőzési kísérlete közbeni baleset miatt...");

                        //A kiesett versenyző(k) mögötti, játékban maradt versenyzők "helyezésének" csökkentése ->


                        if (x + 1 < xCount) helyCsere(pilotak, (byte)(x + 1), 1, xCount);  // A feltétel vizsgálata arra vonatkozik hogy a "leghátsó kieső"
                                                                                           // pilota mögött volt(ak)-e versenyben levő pilota/pilóták....


                    }
                    else if ((elozesKor != 4 && rand >= 4 && rand < 8) || (elozesKor == 4 && rand >= 8 && rand < 16))
                    {
                        versenyben = false;
                        pilotak[pilotak.FindIndex(y => y.getRank() == x)].setVersenyben();

                        Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {nev}. KIESETT a(z) {x}. helyen álló {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} -nak a beelőzési kísérlete közbeni baleset miatt...");
                        Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()}. KIESETT amikor {nev} a(z) {x}. helyért történő beelőzési kísérlete közben összeütköztek...");

                        //A kiesett versenyző(k) mögötti, játékban maradt versenyzők "helyezésének" csökkentése ->

                        if (x + 1 < xCount) helyCsere(pilotak, (byte)(x + 1), 2, xCount);
                        
                    }
                    else if ((elozesKor != 4 && rand >= 8 && rand < 10) || (elozesKor == 4 && rand >= 16 && rand < 20))
                    {
                        byte diff = 2;
                        string kiesettek = "";

                        Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {nev} KIESETT amikor a(z) {x}. helyen álló {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} -nak a beelőzési kísérlete közben tömegbalesetet okozott...");
                        Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()}. KIESETT amikor {nev} a(z) {x}. helyért történő beelőzési kísérlete közben tömegbalesetet okozott...");

                        
                        if (x - 1 >= 1)
                        {
                            kiesettek += '1';  //pilotak[pilotak.FindIndex(y => y.getRank() == x - 1)].setVersenyben();
                            diff += 1;

                            Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {pilotak[pilotak.FindIndex(y => y.getRank() == x - 1)].getNev()}. KIESETT amikor {nev} a(z) {x}. helyen álló {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} -nak a beelőzési kísérlete közben tömegbalesetet okozott...");

                        }
                        if (x + 2 <= pilotak.Where(p => p.getVersenyben()).Count())
                        {
                            kiesettek += '2';  //pilotak[pilotak.FindIndex(y => y.getRank() == x + 2)].setVersenyben();
                            diff += 1;

                            Console.WriteLine($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {pilotak[pilotak.FindIndex(y => y.getRank() == x + 2)].getNev()}. KIESETT amikor {nev} a(z) {x}. helyen álló {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} -nak a beelőzési kísérlete közben tömegbalesetet okozott...");

                        }


                        versenyben = false;
                        pilotak[pilotak.FindIndex(y => y.getRank() == x)].setVersenyben();

                        if (kiesettek.Contains('1')) pilotak[pilotak.FindIndex(y => y.getRank() == x - 1)].setVersenyben();
                        if (kiesettek.Contains('2')) pilotak[pilotak.FindIndex(y => y.getRank() == x + 2)].setVersenyben();


                        //A kiesett versenyző(k) mögötti, játékban maradt versenyzők "helyezésének" csökkentése ->

                        
                        if (kiesettek.Contains('1') && !kiesettek.Contains('2') && x + 1 < xCount)
                        {
                            helyCsere(pilotak, (byte)(x + 1), 3, xCount);

                        }
                        else if (!kiesettek.Contains('1') && kiesettek.Contains('2') && x + 2 < xCount)
                        {
                            helyCsere(pilotak, (byte)(x + 2), 3, xCount);

                        }
                        else if (kiesettek.Contains('1') && kiesettek.Contains('2') && x + 2 < xCount) helyCsere(pilotak, (byte)(x + 2), 4, xCount);


                    }
                    else
                    {

                        Console.Write($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t{round}. körben {nev} előzi a(z) {x}. helyen {pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} versenyzőt");
                        Console.Write($"\t\t{pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} >> {pilotak[pilotak.FindIndex(y => y.getRank() == x + 1)].getNev()}");
                        
                        pilotak[pilotak.FindIndex(y => y.getRank() == x)].setRank(1);
                        setRank(-1);

                        Console.WriteLine($"\t{pilotak[pilotak.FindIndex(y => y.getRank() == x)].getNev()} >> {pilotak[pilotak.FindIndex(y => y.getRank() == x + 1)].getNev()}\t\t{pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }");
                    }



                }

                auto.setTank(4);
                
            }


            protected bool esemeny(double esely)
            {
                double rand = vel.Next(100000) / 1000.0;

                return rand < esely;

            }

        }



        static void Main(string[] args)
        {

            Console.Write("Kérem a versenyfájl nevét: ");
            string fajl = Console.ReadLine();

            char vege = 'n';

            while(vege.ToString().ToLower() != "y")
            {

                sr = new StreamReader(fajl);


                string[] adatok = sr.ReadLine().Split();

                int korok;
                int.TryParse(adatok[0], out korok);

                int versenyzok;
                int.TryParse(adatok[1], out versenyzok);


                string[] indulok = new string[versenyzok];

                for (int i = 0; i < versenyzok; i++)
                {

                    indulok[i] = sr.ReadLine();

                }


                byte n = 1;
                List<Versenyzo> pilotak = new List<Versenyzo>();

                indulok.ToList().ForEach(pilota => {

                    string[] Adatok = pilota.Split();

                    switch (Adatok[1])
                    {
                        case "1": pilotak.Add(new AgresszivPilota(Adatok[0], true, n, new Auto("benzin", 100))); break;
                        case "2": pilotak.Add(new LenduletesPilota(Adatok[0], true, n, new Auto("benzin", 100))); break;
                        case "3": pilotak.Add(new VeszelyesPilota(Adatok[0], true, n, new Auto("benzin", 100))); break;
                        case "4": pilotak.Add(new OvatosPilota(Adatok[0], true, n, new Auto("benzin", 100))); break;
                        default: break;
                    }

                    n++;

                });


                round = 1;
                byte xCount = 8;

                for (int i = 1; i <= korok && xCount > 1; i++)
                {

                    byte x = 0;

                    pilotak.ForEach(p => {

                        //Versenyző(k) kiállása a boxba ->                    

                        if (p.getVersenyben() && p.getCarFuel() < p.getBoxFuel())
                        {
                            Console.Write($"{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }\t\t");

                            x = p.getRank();
                            byte exTank = p.getCarFuel();

                            p.setBox(pilotak, 5, (byte)pilotak.Where(P => P.getVersenyben()).Count());
                            Console.WriteLine($"{round}. körben {p.getNev()} ({exTank} < {p.getBoxFuel()}) kiállt a boxba, ezért a(z) {x}. helyről {p.getRank()}. helyre csúszott...\t\t{ pilotak[0].getRank()}_{ pilotak[1].getRank() }_{ pilotak[2].getRank() }_{ pilotak[3].getRank() }_{ pilotak[4].getRank() }_{ pilotak[5].getRank() }_{ pilotak[6].getRank()}_{ pilotak[7].getRank() }");


                        }


                        //Az előzések vizsgálata ->

                        if (p.getVersenyben() && p.getElozesiEsely() != 0 && (round) % p.getElozesKor() == 0 && p.getRank() > 1 && xCount > 1)
                        {
                            
                            p.setCross(pilotak);

                            //Az esetleges ütközések utáni versenyben maradó pilóták számának frissítése... =>

                            xCount = (byte)pilotak.Where(P => P.getVersenyben()).Count();

                        }



                        //A körönkénti üzemanyag-fogyasztás ->

                        p.setRound();


                        //A balesetek "vizsgálata" ->  LÁSD A 'VERSENYZO' OSZTÁLY DEKLARÁCIÓJÁNÁL...
                        

                        ///x++;

                    });


                    // A még játékban lévő versenyzők sorrendje a kör végén =>

                    Console.Write($"{round}. kör sorrendje: ");

                    for (int j = 1; j <= pilotak.Where(p => p.getVersenyben()).Count(); j++)
                    {
                        if (j > 1) Console.Write($", {pilotak[pilotak.FindIndex(y => y.getRank() == j)].getNev()}");
                        else Console.Write($" {pilotak[pilotak.FindIndex(y => y.getRank() == j)].getNev()}");
                        
                    }

                    Console.WriteLine();

                    round++;

                }

                if (xCount == 1) Console.WriteLine($"Az egyedüli pilóta, aki versenyben maradt és ezáltal megnyerte a futamot: { pilotak[pilotak.FindIndex(y => y.getRank() == 1)].getNev() } !!!");
                else if (xCount > 3)
                {
                    Console.WriteLine("A verseny dobogós helyezettjei sorrendben:");
                    Console.WriteLine($"1. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 1)].getNev() }");
                    Console.WriteLine($"2. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 2)].getNev() }");
                    Console.WriteLine($"3. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 3)].getNev() }");

                }
                else if (xCount == 3)
                {
                    Console.WriteLine("A versenyt egyedül megnyerő 3 pilóta sorrendben:");
                    Console.WriteLine($"1. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 1)].getNev() }");
                    Console.WriteLine($"2. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 2)].getNev() }");
                    Console.WriteLine($"3. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 3)].getNev() }");

                }
                else if (xCount == 2)
                {
                    Console.WriteLine("A versenyt egyedül megnyerő 2 pilóta sorrendben:");
                    Console.WriteLine($"1. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 1)].getNev() }");
                    Console.WriteLine($"2. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 2)].getNev() }");

                }
                else if (xCount == 0)
                {
                    Console.WriteLine("A versenyen induló összes pilóta kiesett, ezért az utolsó állás szerint az első 3 helyen \"befutó\" pilóta:");
                    Console.WriteLine($"1. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 1)].getNev() }");
                    Console.WriteLine($"2. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 2)].getNev() }");
                    Console.WriteLine($"3. helyezett: { pilotak[pilotak.FindIndex(y => y.getRank() == 3)].getNev() }");
                }

                vege = Console.ReadKey().KeyChar;
                Console.WriteLine("\n");

            }

        }

    }

}
