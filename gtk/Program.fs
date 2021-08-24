module Program

open System
open Gtk

[<EntryPoint>]
let main _ =
    Application.Init()

    let app =
        new Application("io.github.zero918nobita", GLib.ApplicationFlags.None)

    app.Register(GLib.Cancellable.Current) |> ignore

    let menu = new GLib.Menu()
    let item = new GLib.MenuItem("Quit", "app.quit")
    item.SetActionAndTargetValue("app.quit", new GLib.Variant(true))
    menu.AppendItem(item)
    app.AppMenu <- menu

    let quitAction =
        new GLib.SimpleAction("quit", GLib.VariantType.Boolean)

    quitAction.Activated.AddHandler(new GLib.ActivatedHandler(fun _ _ -> Application.Quit()))
    app.AddAction(quitAction)

    let window = new Window("My first GTK# Application")
    window.Resize(width = 400, height = 300)
    window.Destroyed.AddHandler(new EventHandler(fun _ _ -> Application.Quit()))

    let area = new DrawingArea()

    area.Drawn.AddHandler(
        new DrawnHandler(fun _ args ->
            let cr = args.Cr
            cr.SetSourceColor(Cairo.Color(0., 0., 0.))
            cr.Rectangle(50., 50., 150., 100.)
            cr.Stroke()
            cr.MoveTo(75., 100.)
            cr.SetFontSize(18.)
            cr.ShowText("Hello, Cairo!"))
    )

    let pane = new VPaned()
    pane.Pack1(child = area, resize = true, shrink = false)
    pane.Pack2(child = new Button("Click me!"), resize = false, shrink = false)
    window.Add(pane)

    app.AddWindow(window)
    window.ShowAll()
    Application.Run()
    0
