﻿open System
open Gtk

let () =
    Application.Init()
    let app = new Application("io.github.zero918nobita", GLib.ApplicationFlags.None)
    app.Register(GLib.Cancellable.Current) |> ignore

    let menu = new GLib.Menu()
    let item = new GLib.MenuItem("Quit", "app.quit")
    item.SetActionAndTargetValue("app.quit", new GLib.Variant(true))
    menu.AppendItem(item)
    app.AppMenu <- menu

    let quit_action = new GLib.SimpleAction("quit", GLib.VariantType.Boolean)
    quit_action.Activated.AddHandler(
        new GLib.ActivatedHandler(fun _ _ ->
            Application.Quit()))
    app.AddAction(quit_action)

    let window = new Window("My first GTK# Application")
    window.Resize(width = 400, height = 300)
    window.Destroyed.AddHandler(new EventHandler(fun _ _ -> Application.Quit()))
    
    let pane = new VPaned()
    pane.Pack1(child = new Label("Vertical Pane"), resize = true, shrink = false)
    pane.Pack2(child = new Button("Click me!"), resize = false, shrink = false)
    window.Add(pane)

    app.AddWindow(window)
    window.ShowAll()
    Application.Run()
