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
using System.Windows.Shapes;

namespace Chess.PromotionWindow
{
    public partial class PromotionWindBlack : PromWindBase
    {
        public PromotionWindBlack()
        {
            InitializeComponent();
        }
        private void SelectFigure(object sender, RoutedEventArgs e)
        {
            var img = sender as Image;
            SelectedFigure = img?.Name;
            DialogResult = true;
            Close();
        }
    }
}

