﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AvaloniaRoguelike.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        this.AttachDevTools();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        Keyboard.Keys.Add(e.Key);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        Keyboard.Keys.Remove(e.Key);
        base.OnKeyUp(e);
    }
}
