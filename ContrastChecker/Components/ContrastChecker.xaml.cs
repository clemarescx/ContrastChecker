using ContrastChecker.ViewModels;

namespace ContrastChecker.Components
{
    /// <summary>
    /// Interaction logic for ContrastChecker.xaml
    /// </summary>
    public partial class ContrastChecker
    {
        public ContrastChecker()
        {
            DataContext = new ContrastCheckerViewModel();
            InitializeComponent();
        }
    }
}
