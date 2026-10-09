# MiniCleanerTool

![Build](https://github.com/LightYagami28/MiniCleanerTool/actions/workflows/dotnet.yml/badge.svg)
![CodeQL](https://github.com/LightYagami28/MiniCleanerTool/actions/workflows/codeql.yml/badge.svg)
[![License](https://img.shields.io/github/license/LightYagami28/MiniCleanerTool)](LICENSE)

> Fork maintained by LightYagami28, based on the [original MiniCleanerTool by MrNico98](https://github.com/MrNico98/MiniCleanerTool). The application is a Windows Forms utility for reviewing large files and cleaning selected Windows temporary data.

## Features

- Inspect large files on the system drive (currently files at least 2 GiB).
- Clean selected temporary and browser-cache locations.
- Optionally empty the Recycle Bin or delete the contents of Downloads.
- Remove missing startup entries, stale SharedDLL references, selected history keys, and empty registry keys after confirmation.
- English and Italian UI.

Some legacy UI labels overstate functionality; the crash-log option is currently analysis-only and disabled. The application does not claim to calculate space reclaimed reliably.

## Requirements and build

- Windows 10 or later; Windows Forms is Windows-only.
- .NET 10 SDK. The repository's `global.json` selects the latest installed stable .NET 10 feature band.

```powershell
dotnet restore MiniCleanerTool.sln
dotnet build MiniCleanerTool.sln --configuration Release --warnaserror
dotnet publish MiniCleanerTool\MiniCleanerTool.csproj --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true --warnaserror
```

The Windows CI performs the warning-as-error build and a self-contained x64 publish. CodeQL analyzes C#; Dependabot checks NuGet and GitHub Actions weekly.

## Safety notes

Cleaning and deleting files or registry values can be irreversible. Review each confirmation and back up important data first. The program currently requests administrator privileges for the whole process; run it only when you trust the code and understand the selected operation. Do not use it as a registry-repair guarantee.

The automatic update path does not install executables. It opens a GitHub release page; verify the publisher and artifact integrity before downloading or running anything. The current update metadata still comes from the upstream MrNico98 repository.

Windows Defender scan data is deliberately excluded from automatic cleanup. Registry services and CLSID entries are not deleted automatically because missing values alone do not establish that a key is obsolete.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) and [SECURITY.md](SECURITY.md). Please include the Windows version, steps to reproduce, and relevant logs; never attach personal files or registry exports containing private data.

## License and attribution

MIT; see [LICENSE](LICENSE). Original project and credits: [MrNico98/MiniCleanerTool](https://github.com/MrNico98/MiniCleanerTool). UI assets retain their original attribution.
