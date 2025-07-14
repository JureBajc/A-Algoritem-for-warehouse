# 🛒 GoDrive - Sistem za Optimizacijo Poti v Skladišču

**GoDrive** je simulacijska C# aplikacija, ki uporablja **A* algoritme** za optimizacijo poti delavca po skladišču pri pobiranju izdelkov. Sistem omogoča izbiro različnih strategij iskanja poti, vključno z **basic optimizacijo**, **backtracking logiko** in **podrobnim prikazom korakov algoritma**.

## ✨ Funkcionalnosti

- Vizualizacija skladišča na osnovi ASCII mape
- Iskanje poti do izdelkov z A* algoritmom
- Možnost izbire strategije iskanja poti:
  - Brez vračanja (originalna logika)
  - Z vračanjem na prejšnjo pozicijo (backtrack)
  - Podrobni prikaz korakov algoritma z razdaljami
- Statistika poti:
  - Skupno število korakov
  - Povprečna dolžina poti do izdelka
  - Ocenjen čas hoje

## 🧠 Algoritmi

Uporabljen je **A\*** (A-Star) algoritmi za iskanje najkrajše poti na mreži, kjer:

- `.` predstavlja prazno polje (prehodno)
- `#` predstavlja izdelek (cilj poti)
- Pot se izračuna od začetne točke `(0, 0)` do vseh izdelkov v čim bolj optimalnem vrstnem redu

Strategije optimizacije:

1. **Originalna logika**: Delavec se ne vrača, samo gre od enega izdelka do naslednjega.
2. **Z backtrack logiko**: Delavec se po vsakem pobiranju vrne na predhodno pozicijo.
3. **Podrobni koraki**: Vsak korak algoritma je prikazan z razdaljami in opisom poti.

## 🖥️ Uporaba

### Zahteve

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (npr. .NET 6 ali novejši)
- Podprt sistem Windows/Linux/macOS

### Zagon programa

1. Kloniraj repozitorij:

```bash
git clone https://github.com/JureBajc/A-Algoritem-for-warehouse.git
cd GoDrive
