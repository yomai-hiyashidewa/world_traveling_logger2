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
using WorldTravelLogger.ViewModels;

namespace WorldTravelLogger.Views.Parts
{
    /// <summary>
    /// RouteCountryViewPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class RouteCountryViewPanel : UserControl
    {
        public RouteCountryViewPanel()
        {
            InitializeComponent();
        }

        public void SetVM(RouteCountryViewModel vm)
        {
            this.countryList.SetVM(vm.GetCountryListViewModel());
            this.DataContext = vm;
        }
    }
}
