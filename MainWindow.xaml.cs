using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace PraiseAnimation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitImages();
        }

        private IList<string> images = new List<string>();

        private IList<PathGeometry> pathGeometries = new List<PathGeometry>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PraiseAnimation();
        }

        private void PraiseAnimation()
        {
            var story = new Storyboard();

            //随机选择一张图片
            Random random = new Random((int)DateTime.Now.Ticks);
            int index = random.Next(3);
            var uri = new Uri(images[index]);
            BitmapImage bitmapImage = new BitmapImage(uri);
            var image = new Image() { Source = bitmapImage, Stretch = Stretch.Uniform, Width = 30, Height = 30, Opacity = 0 };
            var transform = new TranslateTransform();
            image.RenderTransform = transform;

            //随机选择一个动画路径
            int geometryIndex = random.Next(pathGeometries.Count);
            PathGeometry path = pathGeometries[geometryIndex];

            var animation1 = new DoubleAnimationUsingPath()
            {
                Duration = new TimeSpan(0, 0, 5),
                Source = PathAnimationSource.X,
                PathGeometry = path,
                AutoReverse = false,
            };
            Storyboard.SetTarget(animation1, image);
            Storyboard.SetTargetProperty(animation1, new PropertyPath("RenderTransform.X"));
            story.Children.Add(animation1);

            var animation2 = new DoubleAnimationUsingPath()
            {
                Duration = new TimeSpan(0, 0, 5),
                Source = PathAnimationSource.Y,
                PathGeometry = path,
                AutoReverse = false,
            };
            Storyboard.SetTarget(animation2, image);
            Storyboard.SetTargetProperty(animation2, new PropertyPath("RenderTransform.Y"));
            story.Children.Add(animation2);

            var animation3 = new DoubleAnimation()
            {
                Duration = new TimeSpan(0, 0, 5),
                From = 1,
                To = 0,
                AutoReverse = false,
            };
            Storyboard.SetTarget(animation3, image);
            Storyboard.SetTargetProperty(animation3, new PropertyPath(Image.OpacityProperty));
            story.Children.Add(animation3);

            paint.Children.Add(image);

            //动画完成后将图片从UI中移除
            story.Completed += new EventHandler((o, e) =>
            {
                paint.Dispatcher.Invoke(() =>
                {
                    paint.Children.Remove(image);
                });
            });

            //开始动画
            story.Begin(image);
        }

        /// <summary>
        /// 初始化图片
        /// </summary>
        private void InitImages()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            images.Add(Path.Combine(directory,"Resources/star.png"));
            images.Add(Path.Combine(directory, "Resources/zan.png"));
            images.Add(Path.Combine(directory,"Resources/heart.png"));

            foreach (DictionaryEntry item in this.Resources)
            {
                if(item.Key.ToString().StartsWith("praise_"))
                {
                    pathGeometries.Add(item.Value as PathGeometry);
                }
            }
        }
    }
}
