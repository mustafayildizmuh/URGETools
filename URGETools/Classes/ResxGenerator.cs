using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Xml.Linq;

public static class ResxGenerator
{
    public static void GenerateResx(Form form, string outputFolder)
    {
        var entries = new Dictionary<string, string>();
        ProcessControl(form, entries);

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string resxPath = Path.Combine(outputFolder, $"{form.Name}.resx");
        WriteResx(entries, resxPath);
    }

    private static void ProcessControl(Control control, Dictionary<string, string> entries)
    {
        if (!string.IsNullOrEmpty(control.Text))
        {
            string key = $"{control.Name}_Text";
            if (!entries.ContainsKey(key))
                entries.Add(key, control.Text);
        }

        // Eğer ToolTip varsa
        var tooltip = GetToolTipText(control);
        if (!string.IsNullOrEmpty(tooltip))
        {
            string key = $"{control.Name}_ToolTip";
            if (!entries.ContainsKey(key))
                entries.Add(key, tooltip);
        }

        // Eğer DataGridView kolonları varsa
        if (control is DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!string.IsNullOrEmpty(col.HeaderText))
                {
                    string key = $"{col.Name}_HeaderText";
                    if (!entries.ContainsKey(key))
                        entries.Add(key, col.HeaderText);
                }
            }
        }

        // Eğer TabControl varsa TabPage başlıklarını da al
        if (control is TabControl tabControl)
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                if (!string.IsNullOrEmpty(page.Text))
                {
                    string key = $"{page.Name}_Text";
                    if (!entries.ContainsKey(key))
                        entries.Add(key, page.Text);
                }
            }
        }

        // Alt kontrolleri recursive tara
        foreach (Control child in control.Controls)
        {
            ProcessControl(child, entries);
        }
    }

    private static string GetToolTipText(Control control)
    {
        foreach (Control parent in GetControlHierarchy(control))
        {
            foreach (Component component in parent.Site?.Container?.Components ?? new ComponentCollection(new Component[0]))
            {
                if (component is ToolTip tooltip)
                {
                    string tip = tooltip.GetToolTip(control);
                    if (!string.IsNullOrEmpty(tip))
                        return tip;
                }
            }
        }
        return null;
    }

    private static IEnumerable<Control> GetControlHierarchy(Control control)
    {
        while (control != null)
        {
            yield return control;
            control = control.Parent;
        }
    }

    private static void WriteResx(Dictionary<string, string> entries, string resxPath)
    {
        using (var writer = new ResXResourceWriter(resxPath))
        {
            foreach (var entry in entries)
            {
                writer.AddResource(entry.Key, entry.Value);
            }
        }
    }
}
