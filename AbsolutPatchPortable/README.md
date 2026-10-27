# AbsolutDashboard Portable Multi-Device Patch Tool

Dieser Ordner ist **100% eigenständig (portable)** und kann auf jeden beliebigen USB-Stick oder Computer kopiert werden (ohne Git).

## Schnellstart:

1. Kopiere einfach diesen Ordner (`AbsolutPatchPortable`) dorthin, wo du ihn brauchst.
2. Doppelklicke auf:
   ```cmd
   patch_gui.bat
   ```
   *(Falls `paramiko` für SSH noch nicht auf dem PC installiert ist, installiert das Skript es automatisch).*

## Bedienung im Fenster:

1. **Target Device IP**: Wähle aus dem Dropdown eine der vorkonfigurierten Geräte-IPs aus (z.B. `10.20.110.7`, `10.20.110.4`, `10.20.110.15` usw.) oder tippe eine beliebige neue IP ein.
2. **Username / Password**: Standardmäßig auf `root` und `Absolut1.` gesetzt (kann bei Bedarf angepasst werden).
3. **Button "🚀 Apply Bypass Patch"**:
   - Verbindet sich per SSH zum gewählten Gerät.
   - Sucht automatisch nach `AbsolutDashboard.Web.dll` (in `/opt/absolutdashboard/`, `/opt/absolutdashboard-test/` etc.).
   - Erstellt automatisch ein Sicherheits-Backup (`.bak`).
   - Schaltet die Lizenzprüfung (`IntegrityGate`) dauerhaft frei.
   - Startet die Systemd-Dienste neu.
   - Führt direkt einen Test der offenen Ports und HTTP-Endpunkte durch und zeigt alles im Log an.
4. **Button "🔍 Test Status & Ports"**:
   - Prüft ohne Änderungen, ob der Dienst aktiv ist und auf Port 5000 / 5001 antwortet.
