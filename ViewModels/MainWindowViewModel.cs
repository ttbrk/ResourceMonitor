using System.ComponentModel;

namespace ResourceMonitor.ViewModels;

// インタフェース定義
public interface IMainWindowViewModel
{
    public string strDate { get; set; }
}

// 具象クラス定義
public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string _strDate;

    public MainWindowViewModel()
    {
        _strDate =string.Empty;
    }
    public string strDate {
        get{ return _strDate; }
        set
        {
            if (_strDate != value)
            {
                _strDate = value;
                OnPropertyChanged(nameof(strDate));
            }
        }
    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}