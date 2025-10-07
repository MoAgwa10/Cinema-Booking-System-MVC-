# PowerShell script to create simple default images
# Run this script to generate default images for the Cinema Booking System

$imagesPath = "wwwroot\images"

# Create images directory if it doesn't exist
if (!(Test-Path $imagesPath)) {
    New-Item -ItemType Directory -Path $imagesPath -Force
}

# Function to create a simple colored image with text
function Create-DefaultImage {
    param(
        [string]$Name,
        [string]$Color,
        [string]$FilePath
    )
    
    # Create a simple SVG image
    $svgContent = @"
<svg width="200" height="200" xmlns="http://www.w3.org/2000/svg">
    <rect width="200" height="200" fill="$Color"/>
    <text x="100" y="100" text-anchor="middle" dominant-baseline="middle" 
          font-family="Arial" font-size="24" font-weight="bold" fill="white">$Name</text>
</svg>
"@
    
    $svgContent | Out-File -FilePath "$FilePath.svg" -Encoding UTF8
    Write-Host "Created $FilePath.svg"
}

# Create default images
Create-DefaultImage -Name "Actor" -Color "#4CAF50" -FilePath "$imagesPath\default-actor"
Create-DefaultImage -Name "Producer" -Color "#2196F3" -FilePath "$imagesPath\default-producer"
Create-DefaultImage -Name "Movie" -Color "#FF9800" -FilePath "$imagesPath\default-movie"
Create-DefaultImage -Name "Cinema" -Color "#9C27B0" -FilePath "$imagesPath\default-cinema"

Write-Host ""
Write-Host "Default images created successfully!"
Write-Host "Note: These are SVG files. For PNG files, you can:"
Write-Host "1. Use an online SVG to PNG converter"
Write-Host "2. Or replace with your own PNG images"
Write-Host ""
Write-Host "Required files:"
Write-Host "- wwwroot/images/default-actor.png"
Write-Host "- wwwroot/images/default-producer.png"
Write-Host "- wwwroot/images/default-movie.png"
Write-Host "- wwwroot/images/default-cinema.png"
