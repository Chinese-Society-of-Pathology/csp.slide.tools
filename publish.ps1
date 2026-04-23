# 四平台一键发布脚本。发布完成后将各平台输出目录打包为 zip。
param(
    # 仅发布指定平台，不传则发布全部。示例: .\publish.ps1 -Targets win-x64,linux-x64
    [string[]]$Targets = @('win-x64', 'linux-x64', 'linux-arm64', 'osx-arm64')
)

$ErrorActionPreference = 'Stop'

$ProjectPath = Join-Path $PSScriptRoot 'src\Csp.Slide.Tools\Csp.Slide.Tools.csproj'
$PublishRoot  = Join-Path $PSScriptRoot 'src\Csp.Slide.Tools\bin\publish'
$ZipRoot      = Join-Path $PSScriptRoot 'publish'

# 每个平台对应的 pubxml 文件名。
$ProfileMap = @{
    'win-x64'    = 'windows'
    'linux-x64'  = 'linux-x64'
    'linux-arm64'= 'linux-arm64'
    'osx-arm64'  = 'osx-arm64'
}

# 从 csproj 读取版本号。
$Version = ([xml](Get-Content $ProjectPath)).Project.PropertyGroup.VersionPrefix | Where-Object { $_ } | Select-Object -First 1
if (-not $Version) { $Version = '0.0.0' }
Write-Host "版本号: $Version" -ForegroundColor DarkGray

if (-not (Test-Path $ZipRoot)) {
    New-Item -ItemType Directory -Path $ZipRoot | Out-Null
}

foreach ($rid in $Targets) {
    if (-not $ProfileMap.ContainsKey($rid)) {
        Write-Warning "未知平台 '$rid'，已跳过。"
        continue
    }

    $profile    = $ProfileMap[$rid]
    $publishDir = Join-Path $PublishRoot $rid
    $zipPath    = Join-Path $ZipRoot "sstools-$Version-$rid.zip"

    Write-Host "`n>>> 发布 $rid ..." -ForegroundColor Cyan
    dotnet publish $ProjectPath `
        -p:PublishProfile=$profile `
        --nologo

    if ($LASTEXITCODE -ne 0) {
        Write-Error "$rid 发布失败，已中止。"
        exit $LASTEXITCODE
    }

    Write-Host ">>> 打包 $rid -> $zipPath" -ForegroundColor Cyan
    if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
    Compress-Archive -Path "$publishDir\*" -DestinationPath $zipPath

    Write-Host ">>> $rid 完成。" -ForegroundColor Green
}

Write-Host "`n全部平台发布完成，zip 文件位于: $ZipRoot" -ForegroundColor Yellow
