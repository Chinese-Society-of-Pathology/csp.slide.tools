# Csp Slide Tools (`sstools`)

A cross-platform command-line tool for converting pathology whole-slide image (WSI) formats.  
Supports batch conversion, real color correction, de-identification, and tile export.

---

## Features

- Convert `.sdpc` / `.dcm` / `.dcmz` slides to target formats (such as `.csp`)
- Real color correction (`-r`) via camera-specific calibration profiles
- De-identification (`-d`) to strip patient metadata
- Batch processing with configurable parallelism (`-p 0–12`)
- Custom tile size (`-w` / `-h`) and compression quality (`-q`)
- Export a specific pyramid layer as JPEG tiles (`-e`)
- Bilingual output (Chinese / English, auto-detected from OS locale)

## Supported Platforms

| Platform | Architecture |
|----------|-------------|
| Windows  | x64         |
| Linux    | x64, ARM64  |
| macOS    | ARM64 (Apple Silicon) |

---

## Installation

Download the latest pre-built binary for your platform from the [Releases](../../releases) page and extract the archive.

```
sstools-x.x.x-win-x64.zip
sstools-x.x.x-linux-x64.zip
sstools-x.x.x-linux-arm64.zip
sstools-x.x.x-osx-arm64.zip
```

### Platform Initialization (Linux / macOS — run once)

After extracting, run the bundled script to register the native library path:

```bash
# Linux x64, Linux ARM64, macOS ARM64
sudo bash run.sh      # Linux
bash run.sh           # macOS
```

---

## Usage

### Batch Conversion — `scvts`

```bash
# Windows
.\sstools.exe scvts <srcFolder> <destFolder> [options]

# Linux / macOS
./sstools scvts <srcFolder> <destFolder> [options]
```

### Single File Conversion — `scvt`

```bash
.\sstools.exe scvt <srcFile> <destFolder> [options]
./sstools scvt <srcFile> <destFolder> [options]
```

### Options

| Flag | Description |
|------|-------------|
| `-f <ext>` | Target format extension (default: `.svs`) |
| `-r` | Enable real color correction |
| `-p <0-12>` | Parallelism level (default: `12`) |
| `-d` | De-identify (strip patient metadata) |
| `-s` | Hide thumbnail in output |
| `-w <px>` | Tile width in pixels |
| `-h <px>` | Tile height in pixels |
| `-q <0-100>` | JPEG compression quality |
| `-e <level>` | Export a specific pyramid level as JPEG tiles |

### Examples

```bash
# Batch convert with color correction, output as .sdpc
.\sstools.exe scvts C:\slides\src C:\slides\dst -r -f .sdpc

# Batch convert with parallelism 4 and quality 85
./sstools scvts /data/src /data/dst -p 4 -q 85

# Convert a single file with de-identification
./sstools scvt /data/sample.dcm /data/output -d
```

---

## Build from Source

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)

### Steps

```bash
git clone <repo-url>
cd Csp.Slide.Tools

# Restore and build
dotnet build

# Run tests
dotnet test

# Publish for a specific platform
dotnet publish src/Csp.Slide.Tools/Csp.Slide.Tools.csproj \
    -c Release -r win-x64 /p:PublishProfile=windows
```

Alternatively, use the bundled PowerShell script to build all four platforms at once:

```powershell
.\publish.ps1
```

Output archives are placed in the `publish/` directory.

---

## Configuration

Color correction parameters are stored in `param.config` (INI format) alongside the binary.  
Camera model profiles, Gamma values, and CCM matrices can be edited directly in that file.

---

## License

[Apache-2.0](LICENSE) © Csp 2024
