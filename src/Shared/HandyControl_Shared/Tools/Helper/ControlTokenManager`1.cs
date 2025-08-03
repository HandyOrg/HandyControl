using System;
using System.Collections.Generic;
using System.Windows;

namespace HandyControl.Tools;

internal class ControlTokenManager<T>(
    Action<string, T> registerCallback = null,
    Action<string, T> unregisterCallback = null
)
    where T : FrameworkElement
{
    private static readonly Dictionary<string, WeakReference<T>> ControlDict = new();

    public void Register(string token, T control)
    {
        if (string.IsNullOrEmpty(token) || control == null)
        {
            return;
        }

        ControlDict[token] = new WeakReference<T>(control);
        registerCallback?.Invoke(token, control);
    }

    public void Unregister(string token, T control)
    {
        if (string.IsNullOrEmpty(token) || control == null)
        {
            return;
        }

        if (!ControlDict.TryGetValue(token, out var reference))
        {
            return;
        }

        if (!reference.TryGetTarget(out var target))
        {
            return;
        }

        if (!ReferenceEquals(target, control))
        {
            return;
        }

        ControlDict.Remove(token);
        unregisterCallback?.Invoke(token, control);
    }

    public void Unregister(T control)
    {
        if (control == null)
        {
            return;
        }

        string unregisteredToken = null;
        foreach (var item in ControlDict)
        {
            if (!item.Value.TryGetTarget(out var target) || !ReferenceEquals(control, target))
            {
                continue;
            }

            unregisteredToken = item.Key;
            break;
        }

        if (unregisteredToken == null)
        {
            return;
        }

        ControlDict.Remove(unregisteredToken);
        unregisterCallback?.Invoke(unregisteredToken, control);
    }

    public void Unregister(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        if (!ControlDict.TryGetValue(token, out var reference))
        {
            return;
        }

        ControlDict.Remove(token);

        if (!reference.TryGetTarget(out var target))
        {
            return;
        }

        unregisterCallback?.Invoke(token, target);
    }

    public bool TryGetControl(string token, out T control)
    {
        control = null;

        if (string.IsNullOrEmpty(token) || !ControlDict.TryGetValue(token, out var reference))
        {
            return false;
        }

        return reference.TryGetTarget(out control);
    }

    public void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not T control)
        {
            return;
        }

        if (e.NewValue == null)
        {
            Unregister(control);
        }
        else
        {
            Register(e.NewValue.ToString(), control);
        }
    }
}
