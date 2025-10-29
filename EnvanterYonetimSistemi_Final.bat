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
dotnet build --verbosity quiet >nul 2>&1

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
echo Uygulama açılıyor...

REM Uygulama başlat (yeni pencerede)
start "Envanter Yönetim Sistemi" dotnet run

echo.
echo ✅ Uygulama başlatıldı!
echo ✅ Artık güvenle bu pencereyi kapatabilirsiniz.
echo.
echo Bu pencere 5 saniye sonra otomatik kapanacak...
timeout /t 5 /nobreak >nul 2>&1

exit