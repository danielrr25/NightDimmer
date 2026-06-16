# NightDimmer

A lightweight screen-dimming overlay for Windows — it lets you take your screen
darker than the monitor's own minimum brightness.

## Why I built this

It started on one of those late nights where you just want to keep working, but
your eyes start to itch and burn from the screen. Even at its lowest setting, my
monitor still felt too bright. I went looking for something to push it past that
limit — and while there are a few apps and repos out there, almost all of them
are pretty old and unmaintained. So I built my own.

If you've ever wanted to keep going past the point where the screen starts
hurting your eyes, this is for you.

## Features
- Dims all monitors with an adjustable darkness level
- Click-through overlay — doesn't block clicks or keyboard input
- System tray icon with a brightness slider
- Darkness is capped so the screen never goes fully black

## Requirements
- Windows
- .NET SDK 10 (https://dotnet.microsoft.com/download)

## Run from source
```
dotnet run
```
## Usage
- Left-click the tray icon to open the brightness slider
- Drag to adjust darkness
- Right-click the tray icon → Exit to quit
