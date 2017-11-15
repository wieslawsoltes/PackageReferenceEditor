#!/usr/bin/env bash
dotnet restore src/PackageReferenceEditor
dotnet build src/PackageReferenceEditor/PackageReferenceEditor.csproj -c Release -f netstandard2.0

dotnet restore src/PackageReferenceEditor.Avalonia
dotnet build src/PackageReferenceEditor.Avalonia/PackageReferenceEditor.Avalonia.csproj -c Release -f netcoreapp2.0
