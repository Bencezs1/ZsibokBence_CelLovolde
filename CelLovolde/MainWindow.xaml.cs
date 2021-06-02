using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace CelLovolde
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageBrush hatter = new ImageBrush();
        ImageBrush szellem = new ImageBrush();
        
        DispatcherTimer mozgat = new DispatcherTimer();
        DispatcherTimer szellemmozgat = new DispatcherTimer();
        
        int felulszamlal = 0;
        int allszamlal = 0;
        public int Talalat { get; set; }
        public int Hibazas { get; set; }

        List<int> felsohely;
        List<int> alsoely;
        List<Rectangle> eltavolit = new List<Rectangle>();

        Random r = new Random();


        public MainWindow()
        {
            InitializeComponent();

            Ter.Focus();

            this.Cursor = Cursors.None;

            hatter.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/background.png"));
            hatter.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/background.png"));
            Ter.Background = hatter;


            Kurzorkep.Source = new BitmapImage(new Uri("pack://application:,,,/kepek/sniper-aim.png"));


            szellem.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/ghost.png"));


            mozgat.Tick += DummyMoveTick;
            mozgat.Interval = TimeSpan.FromMilliseconds(r.Next(800, 2000));
            mozgat.Start();


            szellemmozgat.Tick += GhostAnimation;
            szellemmozgat.Interval = TimeSpan.FromMilliseconds(20);
            szellemmozgat.Start();


            felsohely = new List<int> { 23, 270, 540, 23, 270, 540 };

            alsoely = new List<int> { 138, 128, 678, 138, 128, 678 };
        }

        private void GhostAnimation(object sender, EventArgs e)
        {
            Talal.Content = "Talált: " + Talalat; 
            Melle.Content = "Mellé: " + Hibazas; 



            foreach (var x in Ter.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "szellem")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 5);

                    if (Canvas.GetTop(x) < -180)
                    {
                        eltavolit.Add(x);

                    }
                }
            }


            foreach (Rectangle y in eltavolit)
            {
                Ter.Children.Remove(y);
            }
        }

        private void DummyMoveTick(object sender, EventArgs e)
        {
            eltavolit.Clear();

            foreach (var i in Ter.Children.OfType<Rectangle>())
            {
                if ((string)i.Tag == "felső" || (string)i.Tag == "alsó")
                {
                    eltavolit.Add(i); 

                    felulszamlal--; 
                    allszamlal--;
                    Hibazas++;
                }
            }


            if (felulszamlal < 3)
            {
                Babumutat(felsohely[r.Next(0, 5)], 35, r.Next(1, 4), "felső");
                felulszamlal++; 
            }


            if (allszamlal < 3)
            {
                Babumutat(alsoely[r.Next(0, 5)], 230, r.Next(1, 4), "alsó");
                allszamlal++; 
            }
        }


        private void Mozgat(object sender, MouseEventArgs e)
        {
            Point pozicio = e.GetPosition(this);

            double px = pozicio.X;
            double py = pozicio.Y;

            Canvas.SetLeft(Kurzorkep, px - (Kurzorkep.Width / 2));
            Canvas.SetTop(Kurzorkep, py - (Kurzorkep.Height / 2));


        }

        private void Babumutat(int x, int y, int kinezet, string tag)
        {
            ImageBrush celhatter = new ImageBrush();

            switch (kinezet)
            {
                case 1:
                    celhatter.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/dummy01.png"));
                    break;
                case 2:
                    celhatter.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/dummy02.png"));
                    break;
                case 3:
                    celhatter.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/dummy03.png"));
                    break;
                case 4:
                    celhatter.ImageSource = new BitmapImage(new Uri("pack://application:,,,/kepek/dummy04.png"));
                    break;
            }

            Rectangle rec = new Rectangle
            {
                Tag = tag,
                Width = 80,
                Height = 155,
                Fill = celhatter
            };

            Canvas.SetTop(rec, y); 
            Canvas.SetLeft(rec, x); 

            Ter.Children.Add(rec); 
        }

        private void Loves(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle activeRec = (Rectangle)e.OriginalSource;


                if ((string)activeRec.Tag == "felső" || (string)activeRec.Tag == "alsó")
                {
                    Ter.Children.Remove(activeRec);

                    Talalat++;

                    Rectangle ghostRec = new Rectangle
                    {
                        Width = 60,
                        Height = 100,
                        Fill = szellem,
                        Tag = "szellem"
                    };

                    Canvas.SetLeft(ghostRec, Mouse.GetPosition(Ter).X - 40);
                    Canvas.SetTop(ghostRec, Mouse.GetPosition(Ter).Y - 60);

                    Ter.Children.Add(ghostRec);
                }

                if ((string)activeRec.Tag == "felső")
                {
                    felulszamlal--;
                }
                else if ((string)activeRec.Tag == "alsó")
                {
                    allszamlal--;
                }


            }
        }
    }
}
