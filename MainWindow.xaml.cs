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
    }
}
