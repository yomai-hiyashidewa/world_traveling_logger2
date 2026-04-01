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
    /// SideView.xaml の相互作用ロジック
    /// </summary>
    public partial class SideView : UserControl
    {
        public SideView()
        {
            InitializeComponent();
            
        }

        public void SetVM(SideViewModel vm)
        {
            this.countryList.SetVM(vm.GetCountryListViewModel());
            this.DataContext = vm;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
