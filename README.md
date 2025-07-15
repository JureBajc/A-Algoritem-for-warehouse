# GoSoftDrive – Warehouse Pathfinding Simulator

A C# console application that demonstrates advanced pathfinding and route optimization in a simulated warehouse environment using ASCII maps. The project implements an A* (A-star) search algorithm to find the shortest path between storage locations, supporting multiple product pickups and various visualization modes.

## 🚀 Features

- **ASCII Warehouse Map**: Visualizes the warehouse grid with obstacles, products, and paths
- **Configurable Product Locations**: Products are defined with coordinates and names
- **A* Pathfinding**: Efficient search algorithm for finding optimal routes
- **Multiple Modes**:
  - Step-by-step backtracking collection (`OptSekvencaBacktrack`)
  - Verbose, detailed path explanation (`OptSekvencaKoraki`)
  - Single-product pathfinding
- **Route Summary**: Displays product pickup order, path length, estimated time, and more
- **Custom Visualization**: Colored output in the console with legend and step indices
- **Localized Variable Names**: Example uses Slovenian for variables, but all core logic is in C#

## 🔧 How It Works

### 1. Map Initialization
The warehouse is represented as a 2D grid (`grid`), initialized from an ASCII-art string array. Cells can be free, obstacles, or products, each with a movement cost.

### 2. Product List
Products are defined by `(x, y, name)` tuples and stored in a list.

### 3. Pathfinding Algorithms
- **AStar2D.FindPath**: Finds the shortest path from start to goal using the A* algorithm
- **OptSekvencaBacktrack**: Greedily visits the nearest uncollected product, simulating a backtracking route
- **OptSekvencaKoraki**: Verbose mode; prints detailed decision-making at each step

### 4. User Interaction
The user selects which algorithm/mode to run.

### 5. Visualization
The program prints the final warehouse map, showing the path, product pickup order, and summary statistics.

## 📦 Key Classes

### `Node`
Represents a position in the grid with pathfinding costs and links to the parent node.

### `AStar2D`
Implements the pathfinding logic and multi-product route optimization.

## 💻 Example Output

```
=== GoDrive Sistem Poti ===
Choose algorithm:
1 - Show step trace
2 - Show all steps
Enter choice: 1

Start: (0, 0)
1. Laptop (9, 19)
   +28 steps
2. Bluetooth Speaker (10, 4)
   +33 steps
...

=== Skladišče ===
     0  1  2  ...
00:  S  .  . ...
01:  .  X  . ...
...
```

## 🚀 Running the Code

### Prerequisites
- .NET SDK

### Compilation and Execution
```bash
dotnet run
```

Follow the prompts in the console to select your preferred pathfinding mode.

## 🗺️ Map Legend

| Symbol | Description |
|--------|-------------|
| `S` | Start position |
| `1,2,3,...` | Product pickup order |
| `*` | Path |
| `.` | Free space |
| `#` | Product location |
| `X` | Obstacle |

## ⚙️ Customization

### Products
Add or modify products in the `izdelki` list:
```csharp
var izdelki = new List<(int x, int y, string name)>
{
    (9, 19, "Laptop"),
    (10, 4, "Bluetooth Speaker"),
    // Add more products here
};
```

### Warehouse Map
Change the ASCII-art `mapa` array to simulate different warehouse layouts:
```csharp
string[] mapa = {
    "S.......#.....",
    ".X....X.......",
    "..............#",
    // Modify layout here
};
```

### Pathfinding Modes
Modify or extend algorithms in `AStar2D` class as needed for custom pathfinding behaviors.

## 🎯 Algorithm Options

1. **Step Trace Mode**: Shows the pathfinding process step by step
2. **All Steps Mode**: Displays detailed decision-making at each pathfinding step
3. **Single Product Mode**: Find path to a specific product only

## 📊 Performance Features

- Efficient A* implementation with heuristic optimization
- Greedy nearest-neighbor product collection
- Real-time path visualization
- Step count and time estimation
- Memory-efficient grid representation

---

*This project demonstrates practical applications of graph algorithms in logistics and warehouse management scenarios.*
