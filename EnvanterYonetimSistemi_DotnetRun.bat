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

echo [1/2] En güncel sürüm derleniyor...
dotnet build --verbosity quiet

if %errorlevel% neq 0 (
    echo.
    echo HATA: Derleme başarısız!
    echo Lütfen kodu kontrol edin.
    pause
    exit /b 1
)

echo [2/2] Uygulama başlatılıyor...
echo.
echo ==========================================
echo    SİSTEM HAZIR - İYİ KULANIMLAR!
echo ==========================================
echo.

REM Uygulama başlat ve hemen çık
start /b "" dotnet run

echo Uygulama başlatıldı!
echo Bu pencere 3 saniye sonra kapanacak...
timeout /t 3 /nobreak >nul 2>&1

REM Pencereyi kapat
exit