using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            butProgressBar.IsEnabled = false;
            new Thread(() =>
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    for (int i = 0; i < 101; i++)
                    {
                        await Task.Delay(30);
                        progress.Value = i;
                    }
                    butProgressBar.IsEnabled = true;
                }));

            }).Start();
            
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                progres2.Value = 0;
                if (progres2.Value < 100)
                {
                    Thread highPriority = new Thread(() =>
                    {
                        // Начальное состояние
                        Dispatcher.Invoke(() =>
                        {
                            
                            button2.IsEnabled = false;
                            textBlock.Text = $"Поток запущен с приоритетом: {ThreadPriority.Highest}";
                        });

                        // Устанавливаем высокий приоритет
                        Thread.CurrentThread.Priority = ThreadPriority.Highest;

                        // Заполняем первую половину прогресс-бара
                        for (int i = 0; i <= 50; i++)
                        {
                            Thread.Sleep(30); // Меньшая задержка для плавности
                            Dispatcher.Invoke(() =>
                            {
                                progres2.Value = i;
                            });
                        }

                        Thread.Sleep(1000); // Пауза как в оригинальном коде

                        // Меняем приоритет на нормальный
                        Thread.CurrentThread.Priority = ThreadPriority.Normal;

                        Dispatcher.Invoke(() =>
                        {
                            textBlock.Text = $"Приоритет изменен на: {ThreadPriority.Normal}";
                        });

                        Thread.Sleep(1000); // Пауза как в оригинальном коде

                        // Заполняем вторую половину прогресс-бара
                        for (int i = 51; i <= 100; i++)
                        {
                            Thread.Sleep(30); // Меньшая задержка для плавности
                            Dispatcher.Invoke(() =>
                            {
                                progres2.Value = i;
                            });
                        }


                        // Завершение
                        Dispatcher.Invoke(() =>
                        {
                            textBlock.Text = "Завершено!";
                            button2.IsEnabled = true;
                        });


                    })

                    {
                        Name = "highProiotity",
                        Priority = ThreadPriority.Highest
                    };

                    highPriority.Start();


                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                button2.IsEnabled = false;
            }
           
            
        }

      

        private void Button_Click_2Zadanie(object sender, RoutedEventArgs e)
        {
            ss.IsEnabled = false;
            Thread lowpr = new Thread(() => CountNum("Lower", ThreadPriority.Lowest, textBlock1, progressBar1));
            Thread normpr = new Thread(() => CountNum("Lower", ThreadPriority.Normal, textBlock2, progressBar2));
            Thread highpr = new Thread(() => CountNum("Lower", ThreadPriority.Highest, textBlock3, progressBar3));

            lowpr.Start();
            normpr.Start();
            highpr.Start();

            new Thread(() =>
            {
                lowpr.Join();
                normpr.Join();
                highpr.Join();
            }).Start();
            

        }
        private void CountNum(string threadName, ThreadPriority priority, TextBlock textBlock, ProgressBar progressBar)
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(100);
                Dispatcher.Invoke(() =>
                {
                    textBlock.Text = $"{threadName} {priority} {i}";
                    progressBar.Value = i;
                });
                if (i == 100)
                {
                    Dispatcher.Invoke(() =>
                    {
                        textBlock.Text = $"{threadName} {priority} Зaвершено";
                        ss.IsEnabled = true;
                    });
                }
                
            }
        }

        private void button2WitchoutDispather_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                try
                {

                    textBlock.Text = "Изменено из потока ";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }).Start();
          

        }
    }
}
