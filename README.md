## 🎮 [Streamer.bot Log Viewer](https://github.com/TheTrabin/SB-Log-Reader) — Real-Time Log Monitoring for Streamers

A Windows Forms companion app that streams logs from your [Streamer.bot](Streamer.bot) setup live—complete with filters, alerts, pinning, profiles, and more.

---

## 🧩 Why This Exists
If you're using Streamer.bot, you've probably noticed that keeping an eye on logs (errors, warnings, events) while live streaming can be a hassle. That’s where the Log Viewer comes in: a dedicated Windows app that hooks into `Streamer.bot/data/logs/` to provide a sleek, real-time log display with powerful filtering, alerting, and organization features.

---

## 🗂️ Folder Setup
To get started, make sure you drop the Log Viewer files into your [Streamer.bot](Streamer.bot) apps folder like this:

```
 Streamer.bot/
 ├── data/
 │   └── logs/
 └── apps/
     └── Log Viewer/
         ├── StreamerBotLogViewer.exe
         ├── StreamerBotLogViewer.dll
         ├── logviewer.jpg    ← splash screen
         └── (other dependencies)
```
`This structure lets the app automatically detect logs and show them live.`

---

## 🚀 Automatic Launch on [Streamer.bot](Streamer.bot) Startup
Forget manually opening it each time. Auto-launch Log Viewer right when your [Streamer.bot](Streamer.bot) starts:

  * Open [Streamer.bot](Streamer.bot).
  * Go to Actions → New Action.
  * Name it something like “Start Log Viewer”.

  * Add a new Sub-Action:

-   1. Choose: Core → System → Run A Program
-   2. Type: Execute Program
-   3. Path: apps\Log Viewer\StreamerBotLogViewer.exe
-   4. Working directory: apps\Log Viewer

  * Set the Trigger to: Core → [Streamer.bot](Streamer.bot) Started

Or just import this

```
U0JBRR+LCAAAAAAABACNVU1zmzAQvWcm/4HhXDEG4wC5pW2SHnroJBlfSg4LWogmIFEhbGcy/u+V+LDBOG2O2vf0VrtvWd4vLyzLLlGBfW29m4M+cihRH+1HBVJZP0VuPSBQlPaXngCNehHSUJ5e8ElCwvgB26CsmeAGdJ2FszgAFOtUskr1YCteW4Jbj1+tQgAdq4uHht+kPZU3RTFgJeOsbMr1IYkBDbZvGTaFSSXQatQ68ruLWAPUwoyalyQI3jLMIpJ4rkd8iAKSrDIglHqQLiFMAj8bHtde+9Ngg9OHtXHkkBRoNJVscILs0qKheCdF+YPVSsg3TcqgqD9i/UJOGc/Psf7nTkvKpWgqw7qhumUWsW43yJVRHLOg2MJbrXt9Lo8ETkV5cGGGp4KnjZRa9hyqJMtz7dK49SftH1ngL9DNAt8j3ioIiL/yQhJChARpFLmpH4RhOLGgS/FWmUYEi6tT5EMjjm2uh7l4HqP74+F50qj5HJ0rJhVlqbtmKrq/juNaSYQyEcqNY6iqOo6NW2uGW5RxrMf+aJ6DO5wVCDJvSt1fk9eeoVshX7Wh35n8TL7ZdeSbNbT+vO9n0sDUnZC3O2a8XZzCnWfR8irLkjQkmRdmxI9oQGAZpSQK3WzpJmmCq3lJW2T5SyvqzGR7P2duDrN88rn1VfzLacYp7kyyz3t8b5J1gzEd9qKAqkY6wge4Fxz43XqYSAyDMQ1uMalF+orqEeWm/1Tm4LeC9SNwBBUrB/5o9x03r+t2EdxVQiqkZqMYyxaO54SdJ/NN2qI+SfS/wLmyLy/2fwFvot39HAYAAA==
```

Let it run itself, hassle-free.

----

✨ What’s New – Latest Enhancements
  * Jump to latest post — Instantly scroll to the newest log entries.
  * Larger buttons — See labels clearly at a glance.
  * Faster load times — The app now starts up much more quickly.
  * Caching & position memory — Retains log position and filters across sessions.
  * Pause on scroll for Auto‑Scroll — Stops auto-scrolling when you scroll manually.
  * Context menu — Right-click log entries to:
-   Copy selected text (all highlighted)
-   Copy a single line
-   Set text as a Keyword and auto-jump to the next
-   Set text as a Keyword Alert
-   Set text as a Filter
  * Improved profiles UI — Profiles dropdown relocated to the bottom along with its label, making it much closer to the “Apply Filter” and “Save Profile” buttons.

----
 
## 🛠️ Core Features

  * 🔍 Live Log Streaming — reads from `Streamer.bot/data/logs/` in real-time
  * 🧰 Filters — by text, regex, warnings, or errors
  * 🚨 Alerts & Keywords — define alert words and tag them with profiles
  * ✍️ Pinning — mark important lines and export them later
  * 📂 Profiles — save/load filter/keyword sets for different stream setups
  * ⬆️ Scroll Control — auto-scroll or freeze scroll to review logs
  * 🔍 Jump to Keyword — instant-find key entries
  * 🧩 GUI Tools — tray icon, minimize/hide functionality
  * 🎨 Splash Screen Support — place a logviewer.jpg file to customize startup visuals

----

## ⚙️ Requirements
  * Platform: Windows (with [Streamer.bot](Streamer.bot) installed)
  * Runtime: .NET 6+ (unless use-shared runtime binary)
  * Include logviewer.jpg in the same folder as the .exe or accept default splash behavior

----

## 💻 Getting Started

  * Drop the Log Viewer folder into `Streamer.bot/apps/`
  * Add the "Start Log Viewer" action in [Streamer.bot](Streamer.bot).
  * Launch [Streamer.bot](Streamer.bot)—Log Viewer will spring to life next to your logs.
  * Customize filters, alerts, and profiles to suit your stream setup.

----  

## 📝 Developer Notes
  * Multiprofile support lets you switch quickly between different filter/alert setups
  * Pinned logs give you persistence during a long stream—for post-stream analysis
  * Splash-screen image is optional—but recommended for branding
  * Built in .NET 6 (Windows Forms) for familiar UI and minimal overhead

----

## 🧑‍💻 Who Made This?
[TheTrabin](https://trabin.space) ([@Cobalt Rogues](https://discord.gg/rmAkrErjXt)), 2025 — part of the `Cobalt Rogues development series`.

  * Clicking name will go to [Https://trabin.space](https://trabin.space), home of [`Ashworn`](https://ashworn.42web.io/)

⚡ The Final Word
The [Streamer.bot](Streamer.bot) Log Viewer bridges the gap between raw logs and actionable insights—live. If you're building a modern stream with automation, widgets, chat commands, and more, this app gives you real-time control over your log output. No more endless scrolling or missed errors—just clear, focused monitoring.
