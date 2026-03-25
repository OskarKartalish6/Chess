using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.PromotionWindow
{
    public class PromWindBase : Window
    {
        public string? SelectedFigure { get; protected set; }
        public PromWindBase() { }
    }
}

