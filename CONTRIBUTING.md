# Contributing

## Development setup

Use Windows with the stable .NET 10 SDK. The `global.json` in the repository selects .NET 10 rather than a preview SDK.

```powershell
dotnet restore MiniCleanerTool.sln
dotnet build MiniCleanerTool.sln --configuration Release --warnaserror
```

Before opening a pull request, build the solution and describe what you verified. The application performs destructive operations, so changes to cleanup targets, registry handling, update behavior, or privilege requirements must explain the safety impact and include focused tests where practical. Never test destructive cleanup against personal or production data.

## Pull requests

- Keep changes scoped and explain user-visible behavior changes.
- Do not suppress compiler, analyzer, or security warnings to make CI pass.
- Do not commit `bin/`, `obj/`, local publish output, credentials, or personal logs.
- Preserve third-party attribution and licenses for UI assets and dependencies.
- Include screenshots for visible UI changes and the exact build/test commands run.
