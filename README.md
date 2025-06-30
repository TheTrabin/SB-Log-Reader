# Streamer.bot Log Viewer

A companion Windows Forms application that displays logs from your `Streamer.bot` setup in real-time with filters, alerts, profiles, and pinning functionality.

---

## 📁 Folder Structure & Placement

Place the application folder in the following path:

```
Streamer.bot/
├── data/
│   └── logs/
└── apps/
    └── Log Viewer/
        ├── StreamerBotLogViewer.exe
        ├── logviewer.jpg
        ├── StreamerBotLogViewer.dll
        └── (Other required files)
```

---

## 🚀 How to Launch the Log Viewer Automatically

To launch the app when Streamer.bot starts:

1. **Open Streamer.bot**.
2. Go to **Actions** → **New Action**.
3. Name it something like `Start Log Viewer`.
4. Add a **Sub Action**:
   - Type: `Execute Program`
   - Path: `apps\Log Viewer\StreamerBotLogViewer.exe`
   - Working Directory: `apps\Log Viewer`
5. Go to **Settings** → **Startup** → Add the `Start Log Viewer` action to **"Actions to Run On Startup"**.

---

## 🔧 Application Features

- Reads logs directly from `Streamer.bot/data/logs/`
- Apply filters, including regex, warnings, and errors
- Set alert keywords and group them
- Auto/Manual scrolling
- Save and load keyword profiles
- Pin important lines and export them
- Jump to keywords
- Tray icon support and window hiding

---

## 🖼 Splash Screen & Logo

Include a `logviewer.jpg` image in the same folder as the EXE to display on startup.

---

## 📝 Notes

- Designed for Streamer.bot environments on Windows.
- Requires .NET 6+ runtime if not self-contained.
- Ensure `logviewer.jpg` is present to avoid missing splash image errors.

---

Created by **TheTrabin** – *Cobalt Rogues 2025*
