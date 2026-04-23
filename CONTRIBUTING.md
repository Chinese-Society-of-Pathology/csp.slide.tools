# Contributing to Csp Slide Tools

Thank you for your interest in contributing. This document outlines the process for reporting issues, proposing changes, and submitting pull requests.

---

## Reporting Issues

Before opening an issue, please:

1. Search existing issues to avoid duplicates.
2. Include the following information in your report:
   - **sstools version** (`sstools --version`)
   - **Operating system and architecture** (e.g., Windows x64, Ubuntu 22.04 x64)
   - **Steps to reproduce** — a minimal command that triggers the problem
   - **Expected behavior** vs. **actual behavior**
   - **Error output** (paste the full terminal output)

Use the **Bug Report** issue template when available.

---

## Suggesting Features

Open a **Feature Request** issue and describe:

- The problem you are trying to solve
- Your proposed solution or behavior
- Any alternatives you have considered

---

## Pull Requests

### Workflow

1. Fork the repository and create a feature branch:

   ```bash
   git checkout -b feature/your-feature-name
   ```

2. Make your changes. Follow the coding standards described below.

3. Add or update tests for any modified logic.

4. Ensure all tests pass:

   ```bash
   dotnet test
   ```

5. Commit with a clear, imperative message (e.g., `Fix color correction crash on Linux ARM64`).

6. Open a Pull Request against the `main` branch. Fill in the PR template with a summary of changes and a test plan.

---

## Local Development Setup

### Prerequisites

| Requirement | Version |
|-------------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8) | 8.x |


### Build

```bash
# Restore dependencies
dotnet restore

# Build (Debug)
dotnet build

# Build (Release)
dotnet build -c Release
```

### Run Tests

```bash
dotnet test
```

### Publish All Platforms

```powershell
.\publish.ps1
```

Artifacts are written to the `publish/` directory.

---

## Coding Standards

- Follow the rules in [`.editorconfig`](.editorconfig).
- Use 4 spaces for indentation.
- Private fields use `_camelCase` naming.
- Public members use `PascalCase`.
- Write XML doc comments (`///`) for all public types and members.
- Code comments must be complete sentences with punctuation.

---

## License

By contributing, you agree that your contributions will be licensed under the [Apache License 2.0](LICENSE).
