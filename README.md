# GoDrive – Sistem za optimizacijo poti v skladišču

GoDrive je simulacijska C# konzolna aplikacija, ki uporablja **A\*** algoritme za optimizacijo poti skozi skladišče z več ciljnimi točkami (izdelki). Cilj je minimizirati skupno razdaljo, ki jo mora delavec prehoditi, hkrati pa omogočiti fleksibilne pristope za iskanje poti, vključno z možnostjo vračanja (backtracking).

---

## 🛠️ Tehnične značilnosti

- **Jezik**: C# (.NET 6 ali novejši)  
- **Algoritem**: A\* za iskanje poti po dvodimenzionalni mreži  
- **Način simulacije**: ASCII mreža, kjer je vsaka celica definirana z znaki:
  - `.` (prazno)
  - `#` (izdelek)
  - `*` (pot)
  - `S` (začetek)
- **Uporabniški vmesnik**: Besedilni meni z izborom algoritma  
- **Podatkovne strukture**:
  - `Node` – točka na mreži z atributi za A\* iskanje
  - `AStar2D` – logika iskanja poti in optimizacije
- **Strategije iskanja poti**:
  1. **Brez vračanja** – osnovna optimizacija (gre le naprej)
  2. **Z vračanjem (backtrack)** – po pobiranju izdelka se delavec vrne na predhodno lokacijo
  3. **Korak po koraku** – interaktivna diagnostika poti in izračunov

---

## ⚙️ Zahteve

- .NET SDK 6.0 ali novejši  
- Ukazna vrstica ali terminal (Windows CMD, PowerShell, bash)

---

## 🔧 Namestitev in zagon

```bash
git clone https://github.com/<tvoje-uporabniško-ime>/GoDrive.git
cd GoDrive
dotnet run
