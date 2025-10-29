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
dotnet restore --verbosity quiet

echo [2/3] En güncel sürüm derleniyor...
dotnet build --configuration Debug --verbosity quiet

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
    copy "inventory.db" "bin\Debug\net9.0-windows\inventory.db" >nul 2>&1
)

REM Uygulama çalıştır
cd "bin\Debug\net9.0-windows"
start "" "InventoryApp.exe"
cd /d "C:\Ozan\projects\inventory_program\InventoryApp"

timeout /t 2 /nobreak >nul

echo Uygulama başlatıldı!
echo Bu pencere 5 saniye sonra kapanacak...
timeout /t 5 /nobreak >nul

exit