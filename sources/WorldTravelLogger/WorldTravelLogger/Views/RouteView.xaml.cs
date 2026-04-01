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

namespace WorldTravelLogger.Views
{
    /// <summary>
    /// RouteView.xaml の相互作用ロジック
    /// </summary>
    public partial class RouteView : UserControl
    {
        public RouteView()
        {
            InitializeComponent();
        }

        public void SetVM(RouteViewModel vm)
        {
            arrivals.SetVM(vm.GetRouteCountryViewModel(true));
            regions.SetVM(vm.GetRegionsViewModel());
            departures.SetVM(vm.GetRouteCountryViewModel(false));
            this.DataContext = vm;
        }
    }
}
