# Quick diagnosis script for image upload issues
Write-Host "=== Cinema Booking System - Image Upload Diagnosis ===" -ForegroundColor Cyan
Write-Host ""

# Check folder structure
Write-Host "1. Checking folder structure..." -ForegroundColor Yellow
$folders = @("wwwroot\uploads\actor", "wwwroot\uploads\producer", "wwwroot\uploads\movie", "wwwroot\uploads\cinema", "wwwroot\images")

foreach ($folder in $folders) {
    if (Test-Path $folder) {
        Write-Host "   ✅ $folder exists" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $folder missing" -ForegroundColor Red
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
        Write-Host "   ✅ Created $folder" -ForegroundColor Green
    }
}

Write-Host ""

# Check default images
Write-Host "2. Checking default images..." -ForegroundColor Yellow
$defaultImages = @("default-actor.png", "default-producer.png", "default-movie.png", "default-cinema.png")

foreach ($image in $defaultImages) {
    $imagePath = "wwwroot\images\$image"
    if (Test-Path $imagePath) {
        Write-Host "   ✅ $image exists" -ForegroundColor Green
    } else {
        Write-Host "   ❌ $image missing" -ForegroundColor Red
    }
}

Write-Host ""

# Check permissions
Write-Host "3. Checking write permissions..." -ForegroundColor Yellow
try {
    $testFile = "wwwroot\uploads\test.txt"
    "test" | Out-File $testFile
    Remove-Item $testFile
    Write-Host "   ✅ Write permissions OK" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Write permission issue: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Check Views for enctype
Write-Host "4. Checking Views for enctype..." -ForegroundColor Yellow
$views = Get-ChildItem -Path "Views" -Recurse -Filter "*.cshtml" | Where-Object { $_.Name -match "(Create|Edit)" }
$missingEnctype = @()

foreach ($view in $views) {
    $content = Get-Content $view.FullName -Raw
    if ($content -match '<form.*asp-action="(Create|Edit)"' -and $content -notmatch 'enctype="multipart/form-data"') {
        $missingEnctype += $view.FullName
    }
}

if ($missingEnctype.Count -eq 0) {
    Write-Host "   ✅ All Create/Edit forms have correct enctype" -ForegroundColor Green
} else {
    Write-Host "   ❌ Missing enctype in:" -ForegroundColor Red
    foreach ($file in $missingEnctype) {
        Write-Host "      - $file" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== Diagnosis Complete ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Run: .\create-default-images.ps1 (to create default images)"
Write-Host "2. Build and run the project"
Write-Host "3. Test image upload on any Create page"
Write-Host "4. Check browser Network tab for 404 errors on image requests"
