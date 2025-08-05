using System.ComponentModel;

public class Card : INotifyPropertyChanged
{
    public string Title { get; set; }
    public string IpAddress { get; set; }

    private int brightness;
    public int Brightness
    {
        get => brightness;
        set { brightness = value; OnPropertyChanged(nameof(Brightness)); }
    }

    private int saturation;
    public int Saturation
    {
        get => saturation;
        set { saturation = value; OnPropertyChanged(nameof(Saturation)); }
    }

    private int color;
    public int Color
    {
        get => color;
        set { color = value; OnPropertyChanged(nameof(Color)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
