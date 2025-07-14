using System;
using System.Collections.Generic;
using System.Linq;

internal class GoSoftDrive
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== GoDrive Sistem Poti ===\n");

        string[] asciiMapa = {
            ". . . . . . . . . . . . .",
            ". . # . # . # . # . # . .",
            ". . # . # . # . # . # . .",
            ". . # . # . # . # . # . .",
            ". . # . # . # . # . # . .",
            ". . # . # . # . # . # . .",
            ". . . . . . . . . . . . .",
            ". . # # # # # # # # # . .",
            ". . # # # # # # # # # . .",
            ". . . . . . . . . . . . .",
            ". . # # # # # # # # # . .",
            ". . # # # # # # # # # . .",
            ". . . . . . . . . . . . .",
            ". . # # # # # # # # # . .",
            ". . # # # # # # # # # . .",
            ". . . . . . . . . . . . .",
            ". . . . . . . . . . . . ."
        };

        int visina = asciiMapa.Length;
        int sirina = asciiMapa[0].Split(' ').Length;
        int[,] mreza = new int[sirina, visina];

        for (int y = 0; y < visina; y++)
        {
            var vrstica = asciiMapa[y].Split(' ');
            for (int x = 0; x < sirina; x++)
                mreza[x, y] = vrstica[x] == "#" ? 4 : 0;
        }

        var izdelki = new List<(int x, int y, string naziv)>
        {
            (4, 1, "Produkt B"),
            (4, 3, "Produkt B"),
            (4, 2, "Produkt B"),
            (4, 4, "Produkt B"),
            (4, 5, "Produkt B"),
            (2, 5, "Produkt B"),
            (2, 2, "Produkt B"),
            (2, 1, "Produkt B"),
            (2, 4, "Produkt B"),
            (2, 3, "Produkt B"),
            (6, 1, "Produkt B"),
            (6, 3, "Produkt B"),
            (6, 2, "Produkt B"),
            (6, 4, "Produkt B"),
            (6, 5, "Produkt B"),
            (8, 5, "Produkt B"),
            (8, 2, "Produkt B"),
            (8, 1, "Produkt B"),
            (8, 4, "Produkt B"),
            (8, 7, "Produkt B"),
            (8, 8, "Produkt B"),
            (8, 10, "Produkt B"),
        };

        var zacetek = new Node(0, 0);

        var cilji = izdelki.Select(i => new Node(i.x, i.y)).ToList();
        var imenaCiljev = izdelki.ToDictionary(i => (i.x, i.y), i => i.naziv);
        var ciljniSet = new HashSet<(int, int)>(cilji.Select(n => (n.X, n.Y)));
        var astar = new AStar2D(mreza);

        Console.WriteLine("inicializacija");
        Console.WriteLine($"velikost: {sirina} x {visina}");
        Console.WriteLine($"izdelki: {cilji.Count}");
        Console.WriteLine($"start: ({zacetek.X}, {zacetek.Y})\n");

        //izbira algoritma
        Console.WriteLine("Izberi algoritem:");
        Console.WriteLine("1 - Prikaz sledi korakov");
        Console.WriteLine("2 - Prikaz vseh korakov");
        Console.Write("Vnesi izbiro: ");

        string izbira = Console.ReadLine();
        bool useBacktrack = izbira == "1";
        bool showSteps = izbira == "2";

        var zacetekCas = DateTime.Now;
        List<Node> pot = astar.FindPath(zacetek, cilji.First());
        ;

        if (showSteps)
        {
            pot = astar.OptSekvencaKoraki(zacetek, cilji, imenaCiljev);
        }
        else if(useBacktrack)
        {
            pot = astar.OptSekvencaBacktrack(zacetek, cilji);
        }
        else if(pot.Count == 0)
        {
            Console.WriteLine("ni poti do vseh izdelkov.");
            return;
        }
        else
            pot = astar.FindPath(zacetek, cilji.First());

        var konecCas = DateTime.Now;

        var vrstniRedObiskov = new Dictionary<(int, int), int>();
        int stevec = 1;
        foreach (var n in pot)
            if (ciljniSet.Contains((n.X, n.Y)) && !vrstniRedObiskov.ContainsKey((n.X, n.Y)))
                vrstniRedObiskov[(n.X, n.Y)] = stevec++;

        Node prejsnji = zacetek;
        int st = 1;
        Console.WriteLine($"Start: ({zacetek.X}, {zacetek.Y})\n");
        foreach (var n in pot)
        {
            if (imenaCiljev.ContainsKey((n.X, n.Y)))
            {
                Console.WriteLine($"{st++}. {imenaCiljev[(n.X, n.Y)]} ({n.X}, {n.Y})");
                if (prejsnji != null)
                    Console.WriteLine($"   +{Math.Abs(n.X - prejsnji.X) + Math.Abs(n.Y - prejsnji.Y)} korakov");
                prejsnji = n;
            }
        }

        bool[,] potArr = new bool[sirina, visina];
        foreach (var n in pot)
            if (n.X >= 0 && n.X < sirina && n.Y >= 0 && n.Y < visina)
                potArr[n.X, n.Y] = true;

        Console.WriteLine("\n=== Skladišče ===");
        Console.Write("     ");
        for (int x = 0; x < sirina; x++)
            Console.Write($" {x % 10}");
        Console.WriteLine();

        for (int y = 0; y < visina; y++)
        {
            Console.Write($"{y:D2}: ");
            for (int x = 0; x < sirina; x++)
            {
                string znak;
                if (x == zacetek.X && y == zacetek.Y)
                    znak = " S";
                else if (potArr[x, y] && vrstniRedObiskov.ContainsKey((x, y)))
                    znak = vrstniRedObiskov[(x, y)].ToString("D2");
                else if (potArr[x, y])
                    znak = " *";
                else if (mreza[x, y] == 0)
                    znak = " .";
                else if (mreza[x, y] == 4)
                    znak = " #";
                else
                    znak = " ?";
                Console.Write($"{znak} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nLEGENDA:");
        Console.WriteLine("S = začetek");
        Console.WriteLine("1,2,3,4 = vrstni red pobranih izdelkov");
        Console.WriteLine("* = pot");
        Console.WriteLine(". = cesta");
        Console.WriteLine("# = produkt");
        Console.WriteLine($"\nšt. izdelkov: {cilji.Count}");
        Console.WriteLine($"skupaj poti: {pot.Count} korakov");
        var ms = (konecCas - zacetekCas).TotalMilliseconds;
        Console.WriteLine($"čas računanja: {ms:F2}ms");
        double skupnaRazdaljaVMetr = pot.Count * 3;
        double casVMinutah = skupnaRazdaljaVMetr / 1.4/60;
        Console.WriteLine($"Pribl. čas poti: {casVMinutah:F1} min");

    }

    public class Node
    {
        public int X, Y;
        public double G, H;
        public Node Parent;
        public double F => G + H;
        public Node(int x, int y) { X = x; Y = y; }
        public override bool Equals(object obj) =>
            obj is Node node && X == node.X && Y == node.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }

    public class AStar2D
    {
        private int[,] grid;
        private int maxX, maxY;
        public AStar2D(int[,] g) { grid = g; maxX = g.GetLength(0); maxY = g.GetLength(1); }

        public List<Node> FindPath(Node start, Node goal)
        {
            var open = new List<Node>();
            var closed = new HashSet<(int, int)>();
            var allNodes = new Dictionary<(int, int), Node>();
            Node get(int x, int y)
            {
                var k = (x, y);
                if (!allNodes.TryGetValue(k, out var n))
                {
                    n = new Node(x, y);
                    n.G = double.MaxValue;
                    n.H = 0;
                    n.Parent = null;
                    allNodes[k] = n;
                }
                return n;
            }

            var s = get(start.X, start.Y);
            var kGoal = get(goal.X, goal.Y);

            s.G = 0;
            s.H = Math.Abs(s.X - kGoal.X) + Math.Abs(s.Y - kGoal.Y);
            s.Parent = null;
            open.Add(s);

            int iter = 0;
            while (open.Any() && iter < 10000)
            {
                iter++;
                var curr = open.OrderBy(n => n.F).ThenBy(n => n.H).First();
                open.Remove(curr);

                if (curr.X == kGoal.X && curr.Y == kGoal.Y)
                    return ReconstructPath(curr);

                closed.Add((curr.X, curr.Y));
                foreach (var (dx, dy) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
                {
                    int nx = curr.X + dx, ny = curr.Y + dy;
                    if (nx < 0 || ny < 0 || nx >= maxX || ny >= maxY)
                        continue;
                    if (closed.Contains((nx, ny)))
                        continue;
                    int cell = grid[nx, ny];
                    if (cell != 0 && !(nx == kGoal.X && ny == kGoal.Y))
                        continue;

                    var nei = get(nx, ny);
                    double tG = curr.G + 1;
                    if (tG < nei.G)
                    {
                        nei.Parent = curr;
                        nei.G = tG;
                        nei.H = Math.Abs(nei.X - kGoal.X) + Math.Abs(nei.Y - kGoal.Y);
                        if (!open.Contains(nei))
                            open.Add(nei);
                    }
                }
            }
            return new List<Node>();
        }
        public List<Node> OptSekvencaBacktrack(Node start, List<Node> goals)
        {
            var current = start;
            var rezultat = new List<Node>();
            var preostali = new List<Node>(goals);

            while (preostali.Any())
            {
                var best = preostali.OrderBy(g =>
                {
                    var pot = FindPath(current, g);
                    return pot != null && pot.Count > 0 ? pot.Count : int.MaxValue;
                }).First();

                var potDo = FindPath(current, best);
                if (potDo != null && potDo.Count > 1)
                {
                    rezultat.AddRange(potDo.Skip(1));

                    if (potDo.Count > 1)
                    {
                        var backtrackPosition = potDo[potDo.Count - 2]; 
                        rezultat.Add(backtrackPosition);
                        current = backtrackPosition;
                    }
                    else
                    {
                        current = best;
                    }
                }
                else
                {
                    current = best;
                }

                preostali.Remove(best);
            }

            return rezultat;
        }
        public List<Node> OptSekvencaKoraki(Node start, List<Node> goals, Dictionary<(int, int), string> imenaCiljev)
        {
            var current = start;
            var rezultat = new List<Node>();
            var preostali = new List<Node>(goals);
            int korak = 1;

            Console.WriteLine("\n=== PODROBNI PRIKAZ ALGORITMSKIH KORAKOV ===");
            Console.WriteLine($"Začetna pozicija: ({current.X}, {current.Y})");
            Console.WriteLine($"Skupaj ciljev: {preostali.Count}\n");

            while (preostali.Any())
            {
                Console.WriteLine($"--- KORAK {korak} ---");
                Console.WriteLine($"Trenutna pozicija: ({current.X}, {current.Y})");
                Console.WriteLine($"Preostali cilji: {preostali.Count}");

                var razdalje = new List<(Node cilj, int razdalja, List<Node> pot)>();
                foreach (var cilj in preostali)
                {
                    var pot = FindPath(current, cilj);
                    int razdalja = pot != null && pot.Count > 0 ? pot.Count - 1 : int.MaxValue;
                    razdalje.Add((cilj, razdalja, pot));
                }

                Console.WriteLine("Razdalje do preostalih ciljev:");
                foreach (var (cilj, razdalja, pot) in razdalje.OrderBy(x => x.razdalja))
                {
                    string naziv = imenaCiljev.ContainsKey((cilj.X, cilj.Y)) ? imenaCiljev[(cilj.X, cilj.Y)] : "Neznano";
                    if (razdalja == int.MaxValue)
                        Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): NEDOSTOPNO");
                    else
                        Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): {razdalja} korakov");
                }

                //najblizji cilj
                var best = razdalje.OrderBy(x => x.razdalja).First();
                Console.WriteLine($"Izbran cilj: {imenaCiljev[(best.cilj.X, best.cilj.Y)]} ({best.cilj.X}, {best.cilj.Y}) - {best.razdalja} korakov");

                if (best.pot != null && best.pot.Count > 1)
                {
                    Console.WriteLine("Pot do cilja:");
                    for (int i = 0; i < best.pot.Count; i++)
                    {
                        var node = best.pot[i];
                        if (i == 0)
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y}) - START");
                        else if (i == best.pot.Count - 1)
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y}) - CILJ (produkt)");
                        else
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y})");
                    }
                    rezultat.AddRange(best.pot.Skip(1));
                    Console.WriteLine($"POBIRANJE: Delavec pobere produkt na ({best.cilj.X}, {best.cilj.Y})");

                    if (best.pot.Count > 1)
                    {
                        var backtrackPosition = best.pot[best.pot.Count - 2]; 
                        Console.WriteLine($"VRNITEV: Delavec se vrne na zadnjo pozicijo v poti ({backtrackPosition.X}, {backtrackPosition.Y})");
                        rezultat.Add(backtrackPosition);
                        current = backtrackPosition;
                    }
                    else
                    {
                        Console.WriteLine("OPOZORILO: Pot je prekratka za vrnitev");
                        current = best.cilj;
                    }
                }
                else
                {
                    Console.WriteLine("OPOZORILO: Ni poti do cilja");
                    current = best.cilj;
                }

                preostali.Remove(best.cilj);

                Console.WriteLine($"Nova pozicija: ({current.X}, {current.Y})");
                Console.WriteLine($"Skupaj korakov do sedaj: {rezultat.Count}");
                Console.WriteLine();

                korak++;
            }

            Console.WriteLine("=== ALGORITEM KONČAN ===");
            Console.WriteLine($"Skupaj korakov: {rezultat.Count}");
            Console.WriteLine($"Obiščenih ciljev: {goals.Count}");
            Console.WriteLine();

            return rezultat;
        }

        private List<Node> ReconstructPath(Node koncni)
        {
            var path = new List<Node>();
            for (var curr = koncni; curr != null; curr = curr.Parent)
                path.Add(curr);
            path.Reverse();
            return path;
        }
    }
}
