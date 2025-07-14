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

        // Izbira algoritma
        Console.WriteLine("Izberi algoritem:");
        Console.WriteLine("1 - Originalna logika (brez backtrack)");
        Console.WriteLine("2 - Nova logika (z backtrack)");
        Console.WriteLine("3 - Podrobni prikaz korakov");
        Console.Write("Vnesi izbiro (1, 2 ali 3): ");

        string izbira = Console.ReadLine();
        bool useBacktrack = izbira == "2";
        bool showSteps = izbira == "3";

        var zacetekCas = DateTime.Now;
        List<Node> pot;

        if (showSteps)
        {
            pot = astar.OptimiseSequenceWithSteps(zacetek, cilji, imenaCiljev);
        }
        else
        {
            pot = useBacktrack ?
                astar.OptimiseSequenceWithBacktrack(zacetek, cilji) :
                astar.OptimiseSequence(zacetek, cilji);
        }
        var konecCas = DateTime.Now;

        var vrstniRedObiskov = new Dictionary<(int, int), int>();
        int stevec = 1;
        foreach (var n in pot)
            if (ciljniSet.Contains((n.X, n.Y)) && !vrstniRedObiskov.ContainsKey((n.X, n.Y)))
                vrstniRedObiskov[(n.X, n.Y)] = stevec++;

        if (pot.Count == 0)
        {
            Console.WriteLine("ni poti do vseh izdelkov.");
            return;
        }

        Node prejsnji = zacetek;
        int st = 1;
        string algoritemNaziv = showSteps ? "podrobni prikaz" : (useBacktrack ? "z backtrack" : "originalna logika");
        Console.WriteLine($"=== Pobiranje ({algoritemNaziv}) ===");
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
        Console.Write("    ");
        for (int x = 0; x < sirina; x++)
            Console.Write($"{x % 10} ");
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
                    znak = vrstniRedObiskov[(x, y)].ToString();
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
        Console.WriteLine(". = prazno");
        Console.WriteLine("# = produkt");
        Console.WriteLine($"\nšt. izdelkov: {cilji.Count}");
        Console.WriteLine($"skupaj poti: {pot.Count} korakov");
        Console.WriteLine($"povp. poti: {(double)pot.Count / cilji.Count:F1}");
        var ms = (konecCas - zacetekCas).TotalMilliseconds;
        Console.WriteLine($"čas računanja: {ms:F2}ms");
        double casPoti = pot.Count / 1.5 / 60.0;
        Console.WriteLine($"pribl. čas hoje: {casPoti:F1} min");
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

        // Nova funkcija z backtrack logiko
        public List<Node> OptimiseSequenceWithBacktrack(Node start, List<Node> goals)
        {
            var current = start;
            var rezultat = new List<Node>();
            var preostali = new List<Node>(goals);

            while (preostali.Any())
            {
                // Najdi najblizji cilj
                var best = preostali.OrderBy(g =>
                {
                    var pot = FindPath(current, g);
                    return pot != null && pot.Count > 0 ? pot.Count : int.MaxValue;
                }).First();

                // Najdi pot do tega cilja
                var potDo = FindPath(current, best);
                if (potDo != null && potDo.Count > 1)
                {
                    rezultat.AddRange(potDo.Skip(1)); // Dodaj pot 
                }

                // Premakni se na pozicijo produkta
                current = best;
                preostali.Remove(best);

                // BACKTRACK: Premakni se en korak nazaj za naslednji izračun
                if (preostali.Any()) // Samo ce se obstajajo cilji
                {
                    var backtrackPosition = GetBacktrackPosition(current);
                    if (backtrackPosition != null)
                    {
                        // Dodaj korak nazaj v pot
                        rezultat.Add(backtrackPosition);
                        current = backtrackPosition;
                    }
                }
            }

            return rezultat;
        }

        // Nova funkcija z podrobnim prikazom korakov
        public List<Node> OptimiseSequenceWithSteps(Node start, List<Node> goals, Dictionary<(int, int), string> imenaCiljev)
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

                // Izracunaj razdalje do vseh preostalih ciljev
                var razdalje = new List<(Node cilj, int razdalja, List<Node> pot)>();
                foreach (var cilj in preostali)
                {
                    var pot = FindPath(current, cilj);
                    int razdalja = pot != null && pot.Count > 0 ? pot.Count - 1 : int.MaxValue;
                    razdalje.Add((cilj, razdalja, pot));
                }

                // Prikazi razdalje
                Console.WriteLine("Razdalje do preostalih ciljev:");
                foreach (var (cilj, razdalja, pot) in razdalje.OrderBy(x => x.razdalja))
                {
                    string naziv = imenaCiljev.ContainsKey((cilj.X, cilj.Y)) ? imenaCiljev[(cilj.X, cilj.Y)] : "Neznano";
                    if (razdalja == int.MaxValue)
                        Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): NEDOSTOPNO");
                    else
                        Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): {razdalja} korakov");
                }

                // Izberi najblizji cilj
                var best = razdalje.OrderBy(x => x.razdalja).First();
                Console.WriteLine($"Izbran cilj: {imenaCiljev[(best.cilj.X, best.cilj.Y)]} ({best.cilj.X}, {best.cilj.Y}) - {best.razdalja} korakov");

                // Prikazi pot do cilja
                if (best.pot != null && best.pot.Count > 1)
                {
                    Console.WriteLine("Pot do cilja:");
                    for (int i = 0; i < best.pot.Count; i++)
                    {
                        var node = best.pot[i];
                        if (i == 0)
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y}) - START");
                        else if (i == best.pot.Count - 1)
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y}) - CILJ");
                        else
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y})");
                    }

                    // Dodaj pot v rezultat
                    rezultat.AddRange(best.pot.Skip(1));
                }

                // Premakni se na pozicijo produkta
                current = best.cilj;
                preostali.Remove(best.cilj);

                // BACKTRACK Premakni se en korak nazaj za naslednji izračun
                if (preostali.Any()) // Samo ce se obstajajo cilji
                {
                    var backtrackPosition = GetBacktrackPosition(current);
                    if (backtrackPosition != null && !backtrackPosition.Equals(current))
                    {
                        Console.WriteLine($"BACKTRACK: Premik na ({backtrackPosition.X}, {backtrackPosition.Y})");
                        rezultat.Add(backtrackPosition);
                        current = backtrackPosition;
                    }
                    else
                    {
                        Console.WriteLine("BACKTRACK: Ni mogoč, ostajam na trenutni poziciji");
                    }
                }

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

        // Funkcija za določitev pozicije za backtrack
        private Node GetBacktrackPosition(Node productPosition)
        {
            // Poskusi vse možne smeri za korak nazaj
            var directions = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

            foreach (var (dx, dy) in directions)
            {
                int nx = productPosition.X + dx;
                int ny = productPosition.Y + dy;

                // Preveri, če je pozicija veljavna in prazna
                if (nx >= 0 && ny >= 0 && nx < maxX && ny < maxY && grid[nx, ny] == 0)
                {
                    return new Node(nx, ny);
                }
            }

            // Če ni mogoče narediti koraka nazaj, vrni trenutno pozicijo
            return productPosition;
        }

        // Originalna funkcija ostane za primerjavo
        public List<Node> OptimiseSequence(Node start, List<Node> goals)
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
                    rezultat.AddRange(potDo.Skip(1));
                current = best;
                preostali.Remove(best);
            }
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
