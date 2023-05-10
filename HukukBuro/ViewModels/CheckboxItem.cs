namespace HukukBuro.ViewModels;

#pragma warning disable CS8618

public class CheckboxItem<TKey>
{
    public TKey Value { get; set; }

    public bool Checked { get; set; }

    public string Text { get; set; } = string.Empty;
}

public class CheckboxItem : CheckboxItem<int> { }
