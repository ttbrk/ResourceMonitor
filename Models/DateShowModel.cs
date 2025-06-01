using System.Windows.Threading;
using ResourceMonitor.ViewModels;

namespace ResourceMonitor.Models;

// インタフェース定義
public interface IDateShowModel
{
    public void StartDate() {}
}

// 具象クラス定義
public class DateShowModel : IDateShowModel
{
    private IMainWindowViewModel _csMainWindowVeiwModel;
    private DispatcherTimer timer;

    public DateShowModel(IMainWindowViewModel ViewModel)
    {
        _csMainWindowVeiwModel = ViewModel;
        timer = new DispatcherTimer();
    }
    public void StartDate()
    {
        // タイマーの初期化
        timer.Interval = TimeSpan.FromMilliseconds(Define.DATE_UPDATE_INTERVAL);
        timer.Tick += Timer_Tick;
        timer.Start();
        // 初期表示
        UpdateTime();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        // タイマーごとに時刻を更新
        UpdateTime();
    }

    private void UpdateTime()
    {
        // 現在時刻を取得してTextBlockに表示
        _csMainWindowVeiwModel.strDate = DateTime.Now.ToString("yyyy年MM月dd日 dddd HH時mm分ss秒");
    }
}