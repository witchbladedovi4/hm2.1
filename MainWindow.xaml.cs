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
            new Thread(() => 
            {
                Dispatcher.Invoke(new Action(async () =>
                {
                    for (int i = 0; i < 101; i++)
                    {
                        await Task.Delay(30);
                        progress.Value = i;
                    }
                }));
                
            }).Start();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Thread highProiotity = new Thread(M) 
            {
                Name = "highProiotity",
                Priority = ThreadPriority.Highest

            };
            highProiotity.Start();
        }

        private void M()
        {
            UpdateUI($"Поток запущен с приоритетом: {Thread.CurrentThread.Priority}");

            Thread.Sleep(1000);

            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            UpdateUI($"Поток запущен с приоритетом: {Thread.CurrentThread.Priority}");
            Thread.Sleep(1000);
        }

        private void UpdateUI(string mes)
        {
            Dispatcher.Invoke(() =>
            {
                if (progres2 != null)
                {
                    progres2.Value += 10;
                    textBlock.Text = mes;
                }
            });


        }

        private void Button_Click_2Zadanie(object sender, RoutedEventArgs e)
        {
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
                    });
                }
                
            }
        }
    }
}
