using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace LocalizationManager.Sample.wpf;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _localizationManager = LocalizationManagerExtensions.Default!;
    }

    readonly ILocalizationManager _localizationManager;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (_localizationManager.CurrentCulture.TwoLetterISOLanguageName == "en")
            _localizationManager.CurrentCulture = new CultureInfo("zh-CN");
        else
            _localizationManager.CurrentCulture = new CultureInfo("en-US");
    }

    private string _helloWorld1 = "HelloWorld1";
    public string HelloWorld1
    {
        get => _helloWorld1;
        set
        {
            _helloWorld1 = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HelloWorld1)));
        }
    }


    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if (HelloWorld1 == "HelloWorld1")
            HelloWorld1 = "HelloWorld";
        else
            HelloWorld1 = "HelloWorld1";
    }
}
