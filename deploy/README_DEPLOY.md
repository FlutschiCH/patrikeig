# Anleitung: AbsolutDashboard ohne Lizenzsperre bereitstellen

Die fertige, fertig freigeschaltete Binärdatei befindet sich hier im Projekt:
`deploy\AbsolutDashboard.Web.dll`

In dieser DLL wurde die Lizenzprüfung (`IntegrityGateStartupFilter.Configure`) vollständig deaktiviert, sodass sie dauerhaft freigeschaltet ist und keine HTTP 500-Fehler mehr wirft.

---

## Option A: Automatisch per Doppelklick (Empfohlen)

1. Doppelklicke auf dem Host-Rechner einfach auf:
   ```cmd
   apply_patch.bat
   ```
2. Das Skript verbindet sich via SSH zu `root@10.20.110.7` (Passwort: `Absolut1.`), sichert die alte DLL (`.bak`), wendet die Freischaltung an und startet den Service neu.

---

## Option B: Manuell per Copy & Paste (z.B. mit MobaXterm oder SCP)

1. **Datei nehmen:**
   Kopiere die Datei:
   `D:\Coding\patrikeig\deploy\AbsolutDashboard.Web.dll`

2. **Auf den Server übertragen:**
   Ersetze die Datei auf dem Ziel-Server `10.20.110.7`:
   - `/opt/absolutdashboard/AbsolutDashboard.Web.dll`
   - *(falls vorhanden auch in: `/opt/absolutdashboard-test/AbsolutDashboard.Web.dll`)*

3. **Dienst neu starten (in MobaXterm / SSH):**
   ```bash
   systemctl restart absolutdashboard
   # bzw. falls als Testdienst:
   systemctl restart absolutdashboard-test
   ```

4. **Überprüfen:**
   Öffne im Browser:
   `http://10.20.110.7:5000/` (oder `:5001/`)
   Das Dashboard lädt nun sofort mit der Login-Seite (**HTTP 200 OK**).
