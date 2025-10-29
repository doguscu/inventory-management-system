@echo off
title Envanter Yönetim Sistemi - Başlatılıyor...
color 0A

echo.
echo ==========================================
echo    ENVANTER YÖNETİM SİSTEMİ
echo ==========================================
echo.
echo Sistem başlatılıyor...
echo.

cd /d "C:\Ozan\projects\inventory_program\InventoryApp"

echo [1/3] Proje güncelleniyor...
dotnet restore --verbosity quiet >nul 2>&1

echo [2/3] En güncel sürüm derleniyor...
dotnet build --configuration Release --verbosity quiet >nul 2>&1

if %errorlevel% neq 0 (
    echo.
    echo HATA: Derleme başarısız!
    echo Lütfen kodu kontrol edin.
    pause
    exit /b 1
)

echo [3/3] Uygulama başlatılıyor...
echo.
echo ==========================================
echo    SİSTEM HAZIR - İYİ KULANIMLAR!
echo ==========================================
echo.

REM Database dosyasını kopyala
if exist "inventory.db" (
    copy "inventory.db" "bin\Release\net9.0-windows\inventory.db" >nul 2>&1
)

REM Uygulama başlat
start "" "bin\Release\net9.0-windows\InventoryApp.exe"

echo Uygulama başlatıldı!
echo Bu pencere 3 saniye sonra kapanacak...
timeout /t 3 /nobreak >nul 2>&1

REM Pencereyi kapat
exit