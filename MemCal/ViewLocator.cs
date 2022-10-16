namespace MemCal;

using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MemCal.ViewModels;

/// <summary>
/// Utility class to locate the appropriate View for a given ViewModel.
/// </summary>
public class ViewLocator : IDataTemplate
{
    /// <summary>
    /// The IControl implementation.
    /// </summary>
    /// <param name="data">The data being loaded.</param>
    /// <returns>The match IControl instances, or a TextBlock with an error message.</returns>
    public IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    /// <summary>
    /// Returns true if the data is an instance of a ViewModelBase.
    /// </summary>
    /// <param name="data">The object to compare.</param>
    /// <returns>True if match, else false.</returns>
    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}