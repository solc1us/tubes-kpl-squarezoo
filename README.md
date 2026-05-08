# Sexual Violence Reporting System (SVRS)

Sistem pengaduan kekerasan seksual berbasis C# yang fokus pada fungsionalitas CRUD, integritas data, dan manajemen alur status menggunakan *State Machine*. Project ini dibuat untuk memenuhi tugas mata kuliah Konstruksi Perangkat Lunak dengan penekanan pada penerapan teknik desain perangkat lunak yang *robust* dan *testable*.

## 🚀 Core Technologies & Techniques

Project ini mengimplementasikan minimal 2 teknik pada setiap class utama untuk menjamin *low coupling* dan *high cohesion*:

| Class | Teknik Utama | Deskripsi Singkat |
| --- | --- | --- |
| **User** | Table-driven Construction | Menggunakan `Dictionary` untuk *permission-based access control*. |
| **Evidence** | Parameterization / Generics | Menangani berbagai tipe bukti (*testimony, media, document*) secara *type-safe*. |
| **Report** | Automata + Table-driven | Mengelola transisi status laporan (State Machine) via *transition table*. |
| **ReportService** | API + Runtime Configuration | Menyediakan programmatic interface untuk CRUD dan konfigurasi *file path* dinamis. |
| **AdminManager** | API + Code Reuse | Mengorkestrasi aksi administratif dengan memanfaatkan *reusable logic* dari service layer. |

## 📂 Project Structure
tubes-kpl-squarezoo/
├── tubes-kpl-squarezoo/
│   ├── Data/             # Persistent storage (JSON files)
│   ├── Enums/            # Definisi State & Type (ReportStatus, EvidenceType)
│   ├── Interfaces/       # Kontrak abstraksi (IEvidence)
│   ├── Models/           # Core logic & Entities (User, Evidence, Report)
│   ├── Services/         # Data Access & File I/O (JSON)
│   └── Managers/         # Business Logic Orchestrator (AdminManager)
└── tests/                # Unit Testing (Logic validation)

## 🛠️ Installation & Setup

1. **Clone Repository**
```bash
   git clone https://github.com/solc1us/tubes-kpl-squarezoo.git
   cd tubes-kpl-squarezoo
   ```

2. **Restore Dependencies**
```bash
   dotnet restore
   ```

3. **Build Project**
```bash
   dotnet build
   ```

4. **Run Application**
```bash
   dotnet run --project src/ReportingSystem.csproj
   ```

## 🧪 Unit Testing

Testing dilakukan secara terisolasi pada level *logic* (terutama pada *state transition* dan *permission lookup*) tanpa harus menjalankan seluruh sistem.

```bash
dotnet test
```

## 📝 Functional Objectives

- **CRUD Operations:** Create, Read, Update, dan Delete laporan secara persisten ke dalam file JSON.
- **State Integrity:** Laporan tidak dapat berpindah status secara ilegal (misal: dari *Draft* langsung ke *Resolved*) berkat implementasi *Automata*.
- **Type Safety:** Penggunaan *Generics* pada class `Evidence` memastikan konten bukti divalidasi sesuai tipenya masing-masing.
- **Permission Validation:** Setiap aksi user divalidasi melalui *permission table* sebelum dieksekusi.
