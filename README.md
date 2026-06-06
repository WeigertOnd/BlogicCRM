# BlogicCRM

BlogicCRM je webová aplikace pro správu klientů, poradců a smluv. Aplikace vznikla jako řešení zadání Blogic CRM v jazyce C#.

Cílem aplikace je vytvořit přehledný systém, ve kterém je možné evidovat klienty, poradce a smlouvy. Každá smlouva má svého klienta, správce smlouvy a může mít také další účastníky. Správce smlouvy je automaticky brán jako jeden z účastníků, takže ho uživatel nemusí zadávat dvakrát.

Aplikace je vytvořena pomocí ASP.NET Core MVC. Uživatelské rozhraní je vytvořeno pomocí Razor Views a Bootstrapu. Databáze je řešena přes Entity Framework Core a MS SQL Server Express.

## Hlavní funkce aplikace

Aplikace umožňuje pracovat s klienty, poradci a smlouvami. U každé z těchto částí je možné záznamy zobrazit, přidat, upravit, smazat a otevřít jejich detail.

U klientů a poradců se eviduje jméno, příjmení, e-mail, telefon, rodné číslo a věk. U smluv se zaznamenává evidenční číslo, instituce, klient, správce smlouvy, datum uzavření, datum platnosti a datum ukončení.

Součástí aplikace je také přehledová stránka, která zobrazuje základní počty klientů, poradců a smluv. Uživatel tak hned po přihlášení vidí základní stav systému.

Aplikace obsahuje filtrování. Klienty a poradce je možné filtrovat podle jména, příjmení, e-mailu, telefonu, rodného čísla nebo věku. Smlouvy je možné filtrovat podle evidenčního čísla, instituce, klienta, správce a stavu smlouvy.

## Stav smlouvy

Stav smlouvy se určuje podle data ukončení. Pokud datum ukončení není vyplněné nebo je v budoucnosti, smlouva je aktivní. Pokud je datum ukončení dnešní datum nebo datum v minulosti, smlouva je ukončená.

Na detailu smlouvy je možné smlouvu ukončit nebo znovu obnovit. Při ukončení se nastaví datum ukončení na aktuální datum. Při obnovení se datum ukončení smaže a smlouva je znovu aktivní.

## Přihlášení a registrace

Aplikace obsahuje jednoduché přihlášení uživatelů. Bez přihlášení není možné zobrazit hlavní data aplikace.

Pro rychlé otestování je možné použít demo účet. E-mail je `admin@blogiccrm.cz` a heslo je `Admin123`.

Uživatel si může vytvořit také vlastní účet přes registrační stránku. Při registraci musí zadat e-mail, potvrzení e-mailu, heslo a potvrzení hesla. Heslo musí mít alespoň 10 znaků, alespoň jedno velké písmeno a alespoň jedno číslo.

Aplikace neukládá hesla v čitelné podobě. Do databáze se ukládá pouze jejich zabezpečená podoba, takže skutečné heslo uživatele není v databázi viditelné.

## Export dat

Aplikace umožňuje exportovat data do CSV souborů. Export je dostupný pro klienty, poradce a smlouvy.

Exportovat lze i aktuálně vyfiltrovaný seznam záznamů. Pokud si uživatel nastaví filtr, exportuje se pouze to, co odpovídá aktuálnímu filtru. Z detailu klienta, poradce nebo smlouvy je možné exportovat také jeden konkrétní záznam.

Aplikace obsahuje také samostatnou stránku Export dat. Na této stránce si uživatel může vybrat, jestli chce exportovat klienty, poradce, smlouvy nebo více částí najednou. Pokud vybere více částí, stáhne se ZIP soubor se samostatnými CSV soubory.

CSV export neobsahuje interní databázové Id, protože pro běžného uživatele není důležité. Export obsahuje pouze přehledná uživatelská data.

## Databáze

Aplikace používá MS SQL Server Express. Připojení k databázi je nastavené v souboru `appsettings.json`.

Použitá databáze se jmenuje `BlogicCRMDb`. Connection string používá instanci `.\SQLEXPRESS`.

Databáze se vytvoří pomocí migrací Entity Framework Core. Po vytvoření databáze se vloží také připravená testovací data, aby aplikace po spuštění nebyla prázdná.

## Spuštění aplikace

Projekt se otevírá ve Visual Studiu pomocí souboru `BlogicCRM.sln`.

Před spuštěním je potřeba mít nainstalovaný a spuštěný MS SQL Server Express. Použitá instance je `.\SQLEXPRESS`.

Databáze se vytvoří přes Package Manager Console ve Visual Studiu. Použije se příkaz:

```powershell
Update-Database -Context BlogicCRM.Data.ApplicationDbContext
```

Po vytvoření databáze je možné aplikaci spustit zeleným tlačítkem ve Visual Studiu. Po spuštění se aplikace otevře v prohlížeči.

## Struktura projektu

Projekt je rozdělený do několika částí. `Controllers` obsahují logiku pro práci s jednotlivými stránkami aplikace. `Models` obsahují hlavní datové entity. `ViewModels` slouží hlavně pro formuláře a předávání dat do zobrazení.

Složka `Data` obsahuje databázový kontext, nastavení vztahů a počáteční data. `Services` obsahuje pomocné služby, například tvorbu CSV exportu. `Views` obsahují Razor stránky, které tvoří uživatelské rozhraní. `wwwroot` obsahuje statické soubory, hlavně CSS.

## Použité technologie

Aplikace je vytvořena v C# pomocí ASP.NET Core MVC. Pro uživatelské rozhraní jsou použity Razor Views, HTML, CSS a Bootstrap. Pro práci s databází je použit Entity Framework Core a MS SQL Server Express.

## Poznámka k projektu

Aplikace je vytvořena jako demonstrační CRM systém. Slouží jako ukázka práce s ASP.NET Core MVC, databází, formuláři, autentizací, filtrováním a exportem dat.
