# Engineering Scripts

## Chocolatey Build

The chocolatey build target is incorporated into `.csproj` files so that NuGet
packages can be built with msbuild.

## Generating New C# Entities for a Data Standard

1. Load schema XSD files into an appropriate directory under `Schema`.
1. Copy and modify `Ed-Fi-Ods-Xsd-Generator-v30.xsd` as needed.
1. Copy and modify `GenerateEntities-v30.ps1` as needed.
1. Run `GenerateEntities-v30.ps1` to generate a new `EdFiEntities.cs` file in
   `src/EdFi.SampleDataGenerator.Core/Entities`.
