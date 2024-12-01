using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ResourceMonitor;

public class UsageGraph
{
    private static Int16 m_iCanvasBarHeight = 80;//GraphCanvas.Actualm_iBarHeight;
    private static Int16 m_iBarStartX = 15;
    private static Int16 m_iBarWidth = 10;
    private static Int16 m_iBarHeight = 100;
    private static Int16 m_iBarOffset = 30;
    private static Int16 m_iBarOffsetParam = 2;
    private static Int16 m_iBarHeightchangepoint = 8;
    private static double m_dBarHeightchangeRatio = 1.5;
    private static Int16 m_iOnebarvalue = (Int16)(Define.VALUE_HUNDRED / Define.CPUGPU_GAGE_COUNT);
    private static double m_dAngle = Math.PI / 6; // 30度の角度（ラジアン）

    /// <summary>
    /// グラフの下地を描画する
    /// </summary>
    /// <param name="_Targetcavas">描画対象のcanvas</param>
    public static void BaseDraw_Usage(Canvas _Targetcavas)
    {

        for (Int16 i = 0; i < Define.CPUGPU_GAGE_COUNT; i++)
        {
            int x = m_iBarStartX + i * (m_iBarWidth + m_iBarOffsetParam); // 配置間隔を調整

            // 平行四辺形の頂点を定義
            double dCurrentBarHeight = m_iBarHeight;
            double dCurrentBarOffset = m_iBarOffset;
            if (m_iBarHeightchangepoint < i)
            {
                dCurrentBarHeight = m_iBarHeight * m_dBarHeightchangeRatio;
                dCurrentBarOffset = m_iBarOffset * m_dBarHeightchangeRatio;
            }
            var points = new PointCollection
            {
                //ポイントは、左下 -> 右下 -> 右上 -> 左上 の順番
                new Point(x,                                                        m_iCanvasBarHeight),
                new Point(x + m_iBarWidth,                                          m_iCanvasBarHeight),
                new Point(x + m_iBarWidth + dCurrentBarOffset * Math.Cos(m_dAngle), m_iCanvasBarHeight - dCurrentBarHeight * Math.Sin(m_dAngle)),
                new Point(x + dCurrentBarOffset * Math.Cos(m_dAngle),               m_iCanvasBarHeight - dCurrentBarHeight * Math.Sin(m_dAngle))
            };

            // 平行四辺形を作成
            SolidColorBrush Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2a4e68"));
            var polygon = new Polygon
            {
                Points = points,
                Fill   = Color,
            };
            // キャンバスに追加
            _Targetcavas.Children.Add(polygon);

            if (m_iOnebarvalue * (i+1) % Define.VALUE_TEN == 0)
            {
                // TextBlockの生成
                TextBlock scaletextBlock = new TextBlock
                {
                    Text = (m_iOnebarvalue * (i+1)).ToString(),
                    FontSize   = 5,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff")),
                };
                // 位置を設定
                Canvas.SetLeft(scaletextBlock, x + m_iBarWidth - 5);
                Canvas.SetTop(scaletextBlock,  m_iCanvasBarHeight);
                // Canvasに追加
                _Targetcavas.Children.Add(scaletextBlock);
            }
        }
    }

    /// <summary>
    /// 使用率に応じたグラフを描画する
    /// </summary>
    /// <param name="_Targetcavas">描画対象のcanvas</param>
    /// <param name="_dUsagePercentage">使用率</param>
    /// <param name="_strTitle">表示するタイトル</param>
    public static void Draw_Usage(Canvas _Targetcavas, double _dUsagePercentage, string _strTitle)
    {
        _Targetcavas.Children.Clear();
        bool bEndFlag = false;
        for (Int16 i = 0; i < Define.CPUGPU_GAGE_COUNT; i++)
        {
            int x = m_iBarStartX + i * (m_iBarWidth + m_iBarOffsetParam); // 配置間隔を調整

            // 平行四辺形の頂点を定義
            double dCurrentBarWidth  = m_iBarWidth;
            double dCurrentBarHeight = m_iBarHeight;
            double dCurrentBarOffset = m_iBarOffset;
            if (m_iBarHeightchangepoint < i)
            {
                dCurrentBarHeight = m_iBarHeight * m_dBarHeightchangeRatio;
                dCurrentBarOffset = m_iBarOffset * m_dBarHeightchangeRatio;
            }
            if (_dUsagePercentage < m_iOnebarvalue * (i + 1))
            {
                bEndFlag = true;
                dCurrentBarWidth = m_iBarWidth * (_dUsagePercentage - m_iOnebarvalue * i) / (double)m_iOnebarvalue;
            }

            var points = new PointCollection
            {
                //ポイントは、左下 -> 右下 -> 右上 -> 左上 の順番
                new Point(x,                                                             m_iCanvasBarHeight),
                new Point(x + dCurrentBarWidth,                                          m_iCanvasBarHeight),
                new Point(x + dCurrentBarWidth + dCurrentBarOffset * Math.Cos(m_dAngle), m_iCanvasBarHeight - dCurrentBarHeight * Math.Sin(m_dAngle)),
                new Point(x + dCurrentBarOffset * Math.Cos(m_dAngle),                    m_iCanvasBarHeight - dCurrentBarHeight * Math.Sin(m_dAngle))
            };
            var polygon = new Polygon
            {
                Points = points,
                Fill   = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2752f9")),
            };
            // キャンバスに追加
            _Targetcavas.Children.Add(polygon);
            if (bEndFlag == true)
            {
                break;
            }
        }

        // TextBlock(タイトル部分)の生成
        TextBlock textBlock1 = new TextBlock
        {
            Text       = _strTitle,
            FontSize   = 15,
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b7c939")),
        };
        // 位置を設定
        Canvas.SetLeft(textBlock1, m_iBarStartX);
        Canvas.SetTop(textBlock1,  0);
        // Canvasに追加
        _Targetcavas.Children.Add(textBlock1);

        // 最初の TextBlock の実際の幅を取得するために測定
        textBlock1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        double textBlock1Width = textBlock1.DesiredSize.Width;

        // 2つ目のTextBlock(使用率部分)の生成
        TextBlock textBlock2 = new TextBlock
        {
            Text          = $"{_dUsagePercentage:F1}%",
            FontSize      = 15,
            Width         = 45,
            TextAlignment = TextAlignment.Right,
            Foreground    = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b7c939")),
        };

        // 位置を最初の TextBlock の右端に設定
        Canvas.SetLeft(textBlock2, m_iBarStartX + textBlock1Width);
        Canvas.SetTop(textBlock2, 0);

        // Canvas に追加
        _Targetcavas.Children.Add(textBlock2);
    }
}

