internal class GoSoftDrive
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== GoDrive Sistem Poti ===\n");

        // ASCII mapa skladišča
        string[] mapa = {
            ". . . . . . . . . . . . .",
            ". . X . X . X . X . X . .",
            ". X # . # . # . # . # X .",
            ". X # . # . # . # . # X .",
            ". X # . # . # . # . # X .",
            ". X # . # . # . # . # X .",
            ". X # . # . # . # . # X .",
            ". X # . # . # . # . # X .",
            ". . X . X . X . X . X . .",
            ". . . . . . . . . . . . .",
            ". . . . . . . . . . . . .",
            ". X # # # # # # # # # X .",
            ". X # # # # # # # # # X .",
            ". . . . . . . . . . . . .",
            ". X # # # # # # # # # X .",
            ". X # # # # # # # # # X .",
            ". . . . . . . . . . . . .",
            ". . . . . . . . . . . . .",
            ". X # # # # # # # # # X .",
            ". X # # # # # # # # # X .",
            ". . . . . . . . . . . . .",
            ". . . . . . . . . . . . ."
        };

        int visina = mapa.Length;
        int sirina = mapa[0].Split(' ').Length;

        int[,] grid = new int[sirina, visina];

        // Inicializacija mreže
        for (int y = 0; y < visina; y++)
        {
            var vrstica = mapa[y].Split(' ');
            for (int x = 0; x < sirina; x++)
            {
                grid[x, y] = vrstica[x] switch
                {
                    "#" => 5,
                    "X" => 9,
                    _ => 1
                };
            }
        }

        // seznam izdelkov v (x,y)
        var izdelki = new List<(int x, int y, string naziv)>
        {
            (9, 19, "Laptop"),
            (10, 4, "Bluetooth Speaker"),
            (6, 3, "Wireless Mouse"),
            (2, 6, "HDMI Cable"),
            (10, 11, "Barcode Scanner"),
            (4, 18, "Winter Jacket"),
            (5, 11, "Notebook Set"),
            (3, 12, "Smartphone Case"),
            (7, 11, "Steel Water Bottle"),
            (8, 3, "LED Desk Lamp"),
            (6, 7, "Office Chair"),
            (9, 12, "Cardboard Box Set"),
            (2, 5, "Noise Cancelling Headphones"),
            (6, 19, "Laptop Stand"),
            (2, 3, "Backpack"),
            (9, 18, "Shoe Box - Size 42"),
            (6, 15, "Wireless Charger"),
            (4, 5, "USB Power Bank"),
            (3, 14, "Yoga Mat"),
            (10, 5, "Helmet"),
            (8, 11, "First Aid Kit"),
            (8, 5, "Portable Heater")};

        var start = new Node(0, 0);
        var targeti = izdelki.Select(i => new Node(i.x, i.y)).ToList();
        var imena = izdelki.ToDictionary(i => (i.x, i.y), i => i.naziv);
        var pozicijeCiljev = new HashSet<(int, int)>(targeti.Select(t => (t.X, t.Y)));

        var pot = new List<Node>();
        var pathfinder = new AStar2D(grid);

        Console.WriteLine("inicializacija");
        Console.WriteLine($"velikost: {sirina} x {visina}");
        Console.WriteLine($"izdelki: {targeti.Count}");
        Console.WriteLine($"start: ({start.X}, {start.Y})\n");

        Console.WriteLine("Izberi algoritem:");
        Console.WriteLine("1 - Prikaz sledi korakov");
        Console.WriteLine("2 - Prikaz vseh korakov");
        Console.Write("Vnesi izbiro: ");
        string izbira = Console.ReadLine();

        bool backtrack = izbira == "1";
        bool verboseSteps = izbira == "2";

        var startTime = DateTime.Now;

        if (verboseSteps)
            pot = pathfinder.OptSekvencaKoraki(start, targeti, imena);
        else if (backtrack)
            pot = pathfinder.OptSekvencaBacktrack(start, targeti);
        else
            pot = pathfinder.FindPath(start, targeti.First());

        if (pot.Count == 0){
            Console.WriteLine("ni poti do vseh izdelkov.");
            return;
        }

        var endTime = DateTime.Now;
        var obiski = new Dictionary<(int, int), int>();
        int ix = 1;

        foreach (var n in pot){
            if (pozicijeCiljev.Contains((n.X, n.Y)) && !obiski.ContainsKey((n.X, n.Y)))
                obiski[(n.X, n.Y)] = ix++;
        }

        Node zadnji = start;
        int idx = 1;
        Console.WriteLine($"Start: ({start.X}, {start.Y})\n");

        foreach (var n in pot){
            if (imena.ContainsKey((n.X, n.Y)))
            {
                Console.WriteLine($"{idx++}. {imena[(n.X, n.Y)]} ({n.X}, {n.Y})");
                Console.WriteLine($"   +{Math.Abs(n.X - zadnji.X) + Math.Abs(n.Y - zadnji.Y)} korakov");
                zadnji = n;
            }
        }

        bool[,] potArr = new bool[sirina, visina];
        foreach (var n in pot){
            if (n.X >= 0 && n.X < sirina && n.Y >= 0 && n.Y < visina)
                potArr[n.X, n.Y] = true;
        }

        // prikaz zemljevida
        Console.WriteLine("\n=== Skladišče ===");
        Console.Write("     ");
        for (int x = 0; x < sirina; x++) Console.Write($"{x % 10} ".PadLeft(2));
        Console.WriteLine();

        for (int y = 0; y < visina; y++){
            Console.Write($"{y:D2}: ");
            for (int x = 0; x < sirina; x++){
                string simbol;
                var barva = Console.ForegroundColor;

                if (x == start.X && y == start.Y){
                    simbol = " S";
                    Console.ForegroundColor = ConsoleColor.Green;
                }else if (potArr[x, y] && obiski.ContainsKey((x, y)))
                {
                    simbol = obiski[(x, y)].ToString("D2");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (potArr[x, y]){
                    simbol = " *";
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (grid[x, y] == 1){
                    simbol = " .";
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (grid[x, y] == 5){
                    simbol = " #";
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (grid[x, y] == 9){
                    simbol = " X";
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else{
                    simbol = " ?";
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                Console.Write($"{simbol} ");
                Console.ForegroundColor = barva;
            }
            Console.WriteLine();
        }

        // povzetek
        Console.WriteLine("\nLEGENDA:");
        Console.WriteLine("S = začetek");
        Console.WriteLine("1,2,3,... = vrstni red izdelkov");
        Console.WriteLine("* = pot");
        Console.WriteLine(". = cesta");
        Console.WriteLine("# = produkt");
        Console.WriteLine("X = blokada");
        Console.WriteLine($"\nšt. izdelkov: {targeti.Count}");
        Console.WriteLine($"skupaj poti: {pot.Count} korakov");
        var ms = (endTime - startTime).TotalMilliseconds;
        Console.WriteLine($"čas računanja: {ms:F2}ms");
        double potVMetr = pot.Count * 3;
        double trajanjeVMin = potVMetr / 1.4 / 60;
        Console.WriteLine($"Pribl. čas poti: {trajanjeVMin:F1} min");
    }
}

public class Node{
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
    private double GetMovementCost(int cellValue){
        switch (cellValue){
            case 1:
                return 1.0;
            case 5:
                return 2.0;
            case 9:
                return double.MaxValue;
            default:
                return 1.0;
    }
}
    public List<Node> FindPath(Node start, Node goal){
        var open = new List<Node>();
        var closed = new HashSet<(int, int)>();
        var allNodes = new Dictionary<(int, int), Node>();

        Node get(int x, int y){
            var k = (x, y);
            if (!allNodes.TryGetValue(k, out var n)){
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
        while (open.Any() && iter < 10000){
            iter++;
            var curr = open.OrderBy(n => n.F).ThenBy(n => n.H).First();
            open.Remove(curr);

            if (curr.X == kGoal.X && curr.Y == kGoal.Y)
                return ReconstructPath(curr);

            closed.Add((curr.X, curr.Y));

            foreach (var (dx, dy) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }){
                int nx = curr.X + dx, ny = curr.Y + dy;
                if (nx < 0 || ny < 0 || nx >= maxX || ny >= maxY)
                    continue;
                if (closed.Contains((nx, ny)))
                    continue;
                int cell = grid[nx, ny];
                if (cell == 9)
                    continue;
                if (cell == 5 && !(nx == kGoal.X && ny == kGoal.Y))
                    continue;
                var nei = get(nx, ny);

                double movementCost = GetMovementCost(cell);
                double tG = curr.G + movementCost;

                if (tG < nei.G){
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

    public List<Node> OptSekvencaBacktrack(Node start, List<Node> goals){
        var current = start;
        var rezultat = new List<Node>();
        var preostali = new List<Node>(goals);

        while (preostali.Any()){
            var best = preostali.OrderBy(g =>{
                var pot = FindPath(current, g);
                return pot != null && pot.Count > 0 ? pot.Count : int.MaxValue;
            }).First();

            var potDo = FindPath(current, best);
            if (potDo != null && potDo.Count > 1){
                rezultat.AddRange(potDo.Skip(1));

                if (potDo.Count > 1){
                    var backtrackPosition = potDo[potDo.Count - 2];
                    rezultat.Add(backtrackPosition);
                    current = backtrackPosition;
                }
                else{
                    current = best;
                }
            }
            else{
                current = best;
            }

            preostali.Remove(best);
        }
        return rezultat;
    }

    public List<Node> OptSekvencaKoraki(Node start, List<Node> goals, Dictionary<(int, int), string> imenaCiljev){
        var current = start;
        var rezultat = new List<Node>();
        var preostali = new List<Node>(goals);
        int korak = 1;

        Console.WriteLine("\n=== PODROBNI PRIKAZ ALGORITMSKIH KORAKOV ===");
        Console.WriteLine($"Začetna pozicija: ({current.X}, {current.Y})");
        Console.WriteLine($"Skupaj ciljev: {preostali.Count}\n");

        while (preostali.Any()){
            Console.WriteLine($"--- KORAK {korak} ---");
            Console.WriteLine($"Trenutna pozicija: ({current.X}, {current.Y})");
            Console.WriteLine($"Preostali cilji: {preostali.Count}");

            var razdalje = new List<(Node cilj, int razdalja, List<Node> pot)>();
            foreach (var cilj in preostali){
                var pot = FindPath(current, cilj);
                int razdalja = pot != null && pot.Count > 0 ? pot.Count - 1 : int.MaxValue;
                razdalje.Add((cilj, razdalja, pot));
            }

            Console.WriteLine("Razdalje do preostalih ciljev:");
            foreach (var (cilj, razdalja, pot) in razdalje.OrderBy(x => x.razdalja)){
                string naziv = imenaCiljev.ContainsKey((cilj.X, cilj.Y)) ? imenaCiljev[(cilj.X, cilj.Y)] : "Neznano";
                if (razdalja == int.MaxValue)
                    Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): NEDOSTOPNO");
                else
                    Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): {razdalja} korakov");
            }

            var best = razdalje.OrderBy(x => x.razdalja).First();
            Console.WriteLine($"Izbran cilj: {imenaCiljev[(best.cilj.X, best.cilj.Y)]} ({best.cilj.X}, {best.cilj.Y}) - {best.razdalja} korakov");

            if (best.pot != null && best.pot.Count > 1){
                Console.WriteLine("Pot do cilja:");
                for (int i = 0; i < best.pot.Count; i++){
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

                if (best.pot.Count > 1){
                    var backtrackPosition = best.pot[best.pot.Count - 2];
                    Console.WriteLine($"VRNITEV: Delavec se vrne na zadnjo pozicijo v poti ({backtrackPosition.X}, {backtrackPosition.Y})");
                    rezultat.Add(backtrackPosition);
                    current = backtrackPosition;
                }
                else{
                    Console.WriteLine("OPOZORILO: Pot je prekratka za vrnitev");
                    current = best.cilj;
                }
            }
            else{
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

    private List<Node> ReconstructPath(Node koncni){
        var path = new List<Node>();
        for (var curr = koncni; curr != null; curr = curr.Parent)
            path.Add(curr);
        path.Reverse();
        return path;
    }
}
