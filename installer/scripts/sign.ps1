param (
    [string]$msiPath,
    [string]$pfxPath,
    [string]$pfxPassword
)

if (-not (Test-Path $pfxPath)) {
    Write-Error "PFX file not found: $pfxPath"
    exit 1
}

# Sign the MSI
& "C:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe" sign `
    /tr http://timestamp.digicert.com `
    /td sha256 `
    /fd sha256 `
    /a `$msiPath `
    /p $pfxPassword `
    /f $pfxPath

if ($LASTEXITCODE -eq 0) {
    Write-Host "MSI signed successfully!"
} else {
    Write-Error "MSI signing failed with exit code: $LASTEXITCODE"
    exit 1
}
