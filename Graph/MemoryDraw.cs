﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ResourceMonitor;

public class MemoryGraph
{
    private static Int16 m_iGageCount = 50;

    //円部分関連変数
    private static Int16 m_iCircleStartAngle = 240;
    private static Int16 m_iCircleEndAngle = 90;
    private static double m_dCircleBarWidth = 2.5;
    private static Int16 m_iCircleBarOffset = 6;
    private static Int16 m_iCenterX = 170;
    private static Int16 m_iCenterY = 200;

    private static Int16 m_iInRadius = 100;
    private static Int16 m_iOutRadius = 150;
    private static double m_dRadiusMagnificationRate = 0.01;
    private static Int16 m_iLineStartAngle = 245;
    private static Int16 m_iLineleEndAngle = 90;
    private static Int16 m_iLineInRadius = 70;
    private static Int16 m_iLineOutRadius = 160;


    private static Int16 m_iBarWidth = 15;
    // private static Int16 m_iBarHeight = 0;
    private static double m_dXLTop = 0;
    private static double m_dXLBottom = 0;
    private static double m_dXRTop = 0;
    private static double m_dXRBottom = 0;


    private static Int16 m_iBarOffset = 30;
    private static Int16 m_iBarHeightchangepoint = 40;
    private static double m_dBarHeightchangeRatio = 0.7;
    private static Int16 m_iOnebarvalue = (Int16)(100 / m_iGageCount);

    /// <summary>
    /// グラフの下地を描画する
    /// </summary>
    /// <param name="_Targetcavas">描画対象のcanvas</param>
    public static void BaseDraw_Memory(Canvas _Targetcavas)
    {
        bool bCircleEndFlg  = false;
        Int16 iCircleEndCnt = 0;
        double iOutRadius   = 0;
        double dBarHeight;

        for (Int16 i = 0; i < m_iGageCount; i++)
        {
            dBarHeight = iOutRadius - m_iInRadius;
            // 平行四辺形の頂点を定義
            double dCurrentBarHeight = dBarHeight;
            double dCurrentBarOffset = m_iBarOffset;
            if (m_iBarHeightchangepoint < i)
            {
                dCurrentBarHeight = dBarHeight * m_dBarHeightchangeRatio;
                dCurrentBarOffset = m_iBarOffset * m_dBarHeightchangeRatio;
            }

            var points = new PointCollection{};
            if (m_iCircleStartAngle - m_iCircleBarOffset * i > m_iCircleEndAngle)
            {
                iOutRadius = (double)(m_iOutRadius + m_iOutRadius * m_dRadiusMagnificationRate * i);
                points = new PointCollection
                {
                    new Point(m_iCenterX + m_iInRadius * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - m_iInRadius * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180)),
                    new Point(m_iCenterX + iOutRadius  * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - iOutRadius  * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180)),
                    new Point(m_iCenterX + iOutRadius  * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i - m_dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - iOutRadius  * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i - m_dCircleBarWidth) * Math.PI / 180)),
                    new Point(m_iCenterX + m_iInRadius * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i - m_dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - m_iInRadius * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i - m_dCircleBarWidth) * Math.PI / 180)),
                };
            }
            else 
            {
                // double x;
                //円形図形描画終了後の初回時のiを保存
                if (bCircleEndFlg == false)
                {
                    bCircleEndFlg = true;
                    iCircleEndCnt = 1;
                    m_dXLTop      = m_iCenterX - 8;
                    m_dXLBottom   = m_iCenterX - 5;
                    m_dXRTop      = m_dXLTop + 5 + m_iBarWidth;
                    m_dXRBottom   = m_dXLBottom  + m_iBarWidth;
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(m_dXLTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRBottom, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(m_dXLBottom, m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                    m_dXLTop    = m_dXRTop;
                    m_dXLBottom = m_dXRBottom;
                }
                if (iCircleEndCnt == 2)
                {
                    m_dXLTop    = m_dXLTop + 2;
                    m_dXLBottom = m_dXLBottom + 2;
                    m_dXRTop    = m_dXLTop  + m_iBarWidth + 10;
                    m_dXRBottom = m_dXLBottom + m_iBarWidth - 5;
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(m_dXLTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRBottom, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(m_dXLBottom, m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                    m_dXLTop    = m_dXRTop;
                    m_dXLBottom = m_dXRBottom;
                } else if (iCircleEndCnt == 3)
                {
                    m_dXLTop    = m_dXLTop + 2;
                    m_dXLBottom = m_dXLBottom + 2;
                    m_dXRTop    = m_dXLTop + m_iBarWidth + 7;
                    m_dXRBottom = m_dXLBottom + m_iBarWidth - 5;
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(m_dXLTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRBottom, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(m_dXLBottom, m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                    m_dXLTop    = m_dXRTop;
                    m_dXLBottom = m_dXRBottom;
                } else if (iCircleEndCnt != 1)
                {
                    double x = m_iCenterX - 2  + iCircleEndCnt * (m_iBarWidth + 2);
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(x,                                   m_iCenterY - iOutRadius),
                        new Point(x + m_iBarWidth,                     m_iCenterY - iOutRadius),
                        new Point(x + m_iBarWidth - dCurrentBarOffset, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(x - dCurrentBarOffset,               m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                }
                iCircleEndCnt ++;
            }

            // 平行四辺形を作成
            SolidColorBrush Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2a4e68"));
            var polygon = new Polygon
            {
                Points = points,
                Fill   = Color,
            };
            // キャンバスに追加
            _Targetcavas.Children.Add(polygon);


            if ((m_iOnebarvalue * i % 10 == 0) && (iCircleEndCnt <= 1))
            {
                double dangle = m_iCircleStartAngle - m_iCircleBarOffset * i + m_iCircleBarOffset / 2;
                Line Tick;
                Tick = new Line
                {
                    X1 = m_iCenterX + (m_iInRadius - 5) * Math.Cos(dangle * Math.PI / 180),
                    Y1 = m_iCenterY - (m_iInRadius - 5) * Math.Sin(dangle * Math.PI / 180),
                    X2 = m_iCenterX + (iOutRadius + 10) * Math.Cos(dangle * Math.PI / 180),
                    Y2 = m_iCenterY - (iOutRadius + 10) * Math.Sin(dangle * Math.PI / 180),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff")),
                    StrokeThickness = 1
                };
                _Targetcavas.Children.Add(Tick);
            }

            // TextBlockの生成
            if (m_iOnebarvalue * (i+1) % 10 == 0)
            {
                TextBlock scaletextBlock = new TextBlock
                {
                    Text = (m_iOnebarvalue * (i+1)).ToString(),
                    FontSize = 15,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff")),
                };
                if (iCircleEndCnt == 0)
                {
                    // 位置を設定
                    Point TextPoint = points[0];
                    if (m_iOnebarvalue * (i+1) == 10)
                    {
                        Canvas.SetLeft(scaletextBlock, TextPoint.X - 5);
                        Canvas.SetTop(scaletextBlock,  TextPoint.Y - 25);
                    } else if (m_iOnebarvalue * (i+1) == 20)
                    {
                        Canvas.SetLeft(scaletextBlock, TextPoint.X + 5);
                        Canvas.SetTop(scaletextBlock,  TextPoint.Y - 20);
                    } else if (m_iOnebarvalue * (i+1) == 30)
                    {
                        Canvas.SetLeft(scaletextBlock, TextPoint.X + 10);
                        Canvas.SetTop(scaletextBlock,  TextPoint.Y - 15);
                    } else if (m_iOnebarvalue * (i+1) == 40)
                    {
                        Canvas.SetLeft(scaletextBlock, TextPoint.X + 10);
                        Canvas.SetTop(scaletextBlock,  TextPoint.Y - 5);
                    } else if (m_iOnebarvalue * (i+1) == 50)
                    {
                        Canvas.SetLeft(scaletextBlock, TextPoint.X + 10);
                        Canvas.SetTop(scaletextBlock,  TextPoint.Y + 3);
                    }
                } else
                {
                    Point TextPoint = points[3];
                    // 位置を設定
                    Canvas.SetLeft(scaletextBlock, TextPoint.X + 3);
                    Canvas.SetTop(scaletextBlock, TextPoint.Y + 3);
                }
                // Canvasに追加
                _Targetcavas.Children.Add(scaletextBlock);
            }

        }

        // 白線部分を描画
        SolidColorBrush LineColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
        double dCircleLineStarX = m_iCenterX + m_iLineInRadius * Math.Cos(m_iLineStartAngle * Math.PI / 180);
        double dCircleLineStarY = m_iCenterY - m_iLineInRadius * Math.Sin(m_iLineStartAngle * Math.PI / 180);
        double dCircleLineEndX  = m_iCenterX + m_iLineInRadius * Math.Cos(m_iLineleEndAngle * Math.PI / 180);
        double dCircleLineEndY  = m_iCenterY - m_iLineInRadius * Math.Sin(m_iLineleEndAngle * Math.PI / 180);

        Path arc = new Path
        {
            Stroke = LineColor,
            StrokeThickness = 2,
            Data = new PathGeometry(new[] {
                new PathFigure {
                    StartPoint = new Point(dCircleLineStarX, dCircleLineStarY),
                    Segments = new PathSegmentCollection {
                        new ArcSegment {
                            Point = new Point(dCircleLineEndX, dCircleLineEndY),
                            Size = new Size(m_iLineInRadius, m_iLineInRadius),
                            SweepDirection = SweepDirection.Clockwise
                        }
                    }
                }
            })
        };
        _Targetcavas.Children.Add(arc);

        Line frameline;
        frameline = new Line
        {
            X1 = dCircleLineStarX,
            Y1 = dCircleLineStarY,
            X2 = m_iCenterX + m_iLineOutRadius * Math.Cos(m_iLineStartAngle * Math.PI / 180),
            Y2 = m_iCenterY - m_iLineOutRadius * Math.Sin(m_iLineStartAngle * Math.PI / 180),
            Stroke = LineColor,
            StrokeThickness = 2
        };
        _Targetcavas.Children.Add(frameline);

        frameline = new Line
        {
            X1 = dCircleLineEndX,
            Y1 = dCircleLineEndY,
            X2 = dCircleLineEndX + 250,
            Y2 = dCircleLineEndY,
            Stroke = LineColor,
            StrokeThickness = 2
        };
        _Targetcavas.Children.Add(frameline);
    }

    // <summary>
    // 使用率に応じたグラフを描画する
    // </summary>
    // <param name="_Targetcavas">描画対象のcanvas</param>
    // <param name="_dUsagePercentage">使用率</param>
    public static void Draw_Memory(Canvas _Targetcavas, double _dUsagePercentage)
    {
        _Targetcavas.Children.Clear();
        bool bEndFlag       = false;
        bool bCircleEndFlg  = false;
        Int16 iCircleEndCnt = 0;
        double iOutRadius   = 0;
        double dBarHeight;

        for (Int16 i = 0; i < m_iGageCount; i++)
        {
            dBarHeight = iOutRadius - m_iInRadius;
            // 平行四辺形の頂点を定義
            double dCurrentBarWidth  = m_iBarWidth;
            double dCircleBarWidth   = m_dCircleBarWidth;
            double dCurrentBarHeight = dBarHeight;
            double dCurrentBarOffset = m_iBarOffset;
            if (m_iBarHeightchangepoint < i)
            {
                dCurrentBarHeight = dBarHeight * m_dBarHeightchangeRatio;
                dCurrentBarOffset = m_iBarOffset * m_dBarHeightchangeRatio;
            }
            if (_dUsagePercentage < m_iOnebarvalue * (i + 1))
            {
                bEndFlag = true;
                dCurrentBarWidth = m_iBarWidth * (_dUsagePercentage - m_iOnebarvalue * i) / (double)m_iOnebarvalue;
                dCircleBarWidth  = - m_dCircleBarWidth + ( 2* m_dCircleBarWidth * (_dUsagePercentage - m_iOnebarvalue * i) / (double)m_iOnebarvalue);
            }

            var points = new PointCollection{};
            if (m_iCircleStartAngle - m_iCircleBarOffset * i > m_iCircleEndAngle)
            {
                iOutRadius = (double)(m_iOutRadius + m_iOutRadius * m_dRadiusMagnificationRate * i);
                points = new PointCollection
                {
                    new Point(m_iCenterX + m_iInRadius * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - m_iInRadius * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180)),
                    new Point(m_iCenterX + iOutRadius  * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - iOutRadius  * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i + m_dCircleBarWidth) * Math.PI / 180)),
                    new Point(m_iCenterX + iOutRadius  * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i - dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - iOutRadius  * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i - dCircleBarWidth) * Math.PI / 180)),
                    new Point(m_iCenterX + m_iInRadius * Math.Cos((m_iCircleStartAngle - m_iCircleBarOffset * i - dCircleBarWidth) * Math.PI / 180),
                              m_iCenterY - m_iInRadius * Math.Sin((m_iCircleStartAngle - m_iCircleBarOffset * i - dCircleBarWidth) * Math.PI / 180)),
                };
            }
            else 
            {
                // double x;
                //円形図形描画終了後の初回時のiを保存
                if (bCircleEndFlg == false)
                {
                    bCircleEndFlg = true;
                    iCircleEndCnt = 1;
                    m_dXLTop      = m_iCenterX - 8;
                    m_dXLBottom   = m_iCenterX - 5;
                    m_dXRTop      = m_dXLTop + 5 + dCurrentBarWidth;
                    m_dXRBottom   = m_dXLBottom  + dCurrentBarWidth;
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(m_dXLTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRBottom, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(m_dXLBottom, m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                    m_dXLTop    = m_dXRTop;
                    m_dXLBottom = m_dXRBottom;
                }
                if (iCircleEndCnt == 2)
                {
                    m_dXLTop    = m_dXLTop + 2;
                    m_dXLBottom = m_dXLBottom + 2;
                    m_dXRTop    = m_dXLTop  + dCurrentBarWidth + 10;
                    m_dXRBottom = m_dXLBottom + dCurrentBarWidth - 5;
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(m_dXLTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRBottom, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(m_dXLBottom, m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                    m_dXLTop    = m_dXRTop;
                    m_dXLBottom = m_dXRBottom;
                } else if (iCircleEndCnt == 3)
                {
                    m_dXLTop    = m_dXLTop + 2;
                    m_dXLBottom = m_dXLBottom + 2;
                    m_dXRTop    = m_dXLTop + dCurrentBarWidth + 7;
                    m_dXRBottom = m_dXLBottom + dCurrentBarWidth - 5;
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(m_dXLTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRTop,    m_iCenterY - iOutRadius),
                        new Point(m_dXRBottom, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(m_dXLBottom, m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                    m_dXLTop    = m_dXRTop;
                    m_dXLBottom = m_dXRBottom;
                } else if (iCircleEndCnt != 1)
                {
                    double x = m_iCenterX - 2  + iCircleEndCnt * (m_iBarWidth + 2);
                    //ポイントは、左上 -> 右上 -> 右下 -> 左下 の順番
                    points = new PointCollection
                    {
                        new Point(x,                                        m_iCenterY - iOutRadius),
                        new Point(x + dCurrentBarWidth,                     m_iCenterY - iOutRadius),
                        new Point(x + dCurrentBarWidth - dCurrentBarOffset, m_iCenterY - iOutRadius + dCurrentBarHeight),
                        new Point(x - dCurrentBarOffset,                    m_iCenterY - iOutRadius + dCurrentBarHeight)
                    };
                }
                iCircleEndCnt ++;
            }

            // 
            SolidColorBrush Color;
            if (90 < _dUsagePercentage && _dUsagePercentage < m_iOnebarvalue * (i + 1)) {
                Color =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f80002"));
            } else if (80 < _dUsagePercentage && _dUsagePercentage < m_iOnebarvalue * (i + 1))
            {
                Color =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fdfc01"));
            } else
            {
                Color =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#02fa03"));
            }
            var polygon = new Polygon
            {
                Points = points,
                Fill   = Color,
            };
            // キャンバスに追加
            _Targetcavas.Children.Add(polygon);
            if (bEndFlag == true)
            {
                break;
            }
        }
    }
}
