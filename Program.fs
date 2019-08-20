open Gtk

let () =
    Application.Init()
    let window = new Window("My first GTK# Application")
    window.Add(new Label("Hello, world!"))
    window.Resize(400, 300)
    window.ShowAll()
    Application.Run()
