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
using WorldTravelLogger.Models;
using WorldTravelLogger.ViewModels;

namespace WorldTravelLogger.Views.Parts
{
    /// <summary>
    /// RouteRegionsViewPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class RouteRegionsViewPanel : UserControl
    {

        public RouteRegionsViewPanel()
        {
            InitializeComponent();
        }

        public void SetVM(RouteRegionViewModel vm)
        {
            regionMiniPanel.SetVM(vm.MiniVM);
            this.DataContext = vm;
        }




        
    }
}
