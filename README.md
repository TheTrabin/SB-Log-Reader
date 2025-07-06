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
    Core => System => Run A Program:
   - Type: `Execute Program`
   - Path: `apps\Log Viewer\StreamerBotLogViewer.exe`
   - Working Directory: `apps\Log Viewer`
5. Trigger: Core => Streamer.bot Started

Or just import this
```
U0JBRR+LCAAAAAAABACNVU1zmzAQvWcm/4HhXDEG4wC5pW2SHnroJBlfSg4LWogmIFEhbGcy/u+V+LDBOG2O2vf0VrtvWd4vLyzLLlGBfW29m4M+cihRH+1HBVJZP0VuPSBQlPaXngCNehHSUJ5e8ElCwvgB26CsmeAGdJ2FszgAFOtUskr1YCteW4Jbj1+tQgAdq4uHht+kPZU3RTFgJeOsbMr1IYkBDbZvGTaFSSXQatQ68ruLWAPUwoyalyQI3jLMIpJ4rkd8iAKSrDIglHqQLiFMAj8bHtde+9Ngg9OHtXHkkBRoNJVscILs0qKheCdF+YPVSsg3TcqgqD9i/UJOGc/Psf7nTkvKpWgqw7qhumUWsW43yJVRHLOg2MJbrXt9Lo8ETkV5cGGGp4KnjZRa9hyqJMtz7dK49SftH1ngL9DNAt8j3ioIiL/yQhJChARpFLmpH4RhOLGgS/FWmUYEi6tT5EMjjm2uh7l4HqP74+F50qj5HJ0rJhVlqbtmKrq/juNaSYQyEcqNY6iqOo6NW2uGW5RxrMf+aJ6DO5wVCDJvSt1fk9eeoVshX7Wh35n8TL7ZdeSbNbT+vO9n0sDUnZC3O2a8XZzCnWfR8irLkjQkmRdmxI9oQGAZpSQK3WzpJmmCq3lJW2T5SyvqzGR7P2duDrN88rn1VfzLacYp7kyyz3t8b5J1gzEd9qKAqkY6wge4Fxz43XqYSAyDMQ1uMalF+orqEeWm/1Tm4LeC9SNwBBUrB/5o9x03r+t2EdxVQiqkZqMYyxaO54SdJ/NN2qI+SfS/wLmyLy/2fwFvot39HAYAAA==
```
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
