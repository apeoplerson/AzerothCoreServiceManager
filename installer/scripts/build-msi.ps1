


Param([string]$Version = "1.0.0")

# Sanitizer function to remove BOM and leading whitespace
function Remove-BomAndLeadingWhitespace {
  param([string]$Path)
  if (-not (Test-Path $Path)) { return }
  $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
  $text = Get-Content -LiteralPath $Path -Raw
  $text = $text -replace '^\uFEFF', ''                # strip BOM
  $text = $text -replace '^\s+(?=<\?xml)', ''         # strip whitespace before xml decl
  [System.IO.File]::WriteAllText($Path, $text, $utf8NoBom)
  $bytes = [System.IO.File]::ReadAllBytes($Path)[0..2]
  if ($bytes[0] -ne 0x3C -or $bytes[1] -ne 0x3F -or $bytes[2] -ne 0x78) {
    throw "XML declaration is not the first bytes in $Path"
  }
}

# Sanitize .wxs files
$wxsA = Join-Path $PSScriptRoot "..\wix\AzerothCoreManager.wxs"
$wxsF = Join-Path $PSScriptRoot "..\wix\Files.wxs"
Remove-BomAndLeadingWhitespace -Path $wxsA
Remove-BomAndLeadingWhitespace -Path $wxsF

# Validate publish output exists
if (-not (Test-Path "..\..\dist\win-x64\AzerothCoreManager\AzerothCoreManager.Service.exe")) {
  Write-Host "Publish output missing. Run dotnet publish first or call the publish step here."
  exit 1
}

# WiX v4 build via MSBuild (this will be executed on Windows)
Write-Host "Building MSI with WiX v4..."
dotnet build ".\installer\wix\AzerothCoreManager.wixproj" -c Release `
  -p:Version=$Version `
  -p:ProductVersion=$Version `
  -p:Platform=x64 `
  -p:AppRoot="..\..\dist\win-x64\AzerothCoreManager"

# Copy MSI to dist\msi
New-Item -Force -ItemType Directory ".\dist\msi" | Out-Null
Get-ChildItem ".\installer\wix\bin\Release\*.msi" | Copy-Item -Destination ".\dist\msi" -Force

Write-Host "MSI build completed successfully!"

