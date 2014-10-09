using System;
using System.Linq.Expressions;
using System.Windows;
using ExpressionCompiler;

namespace ExpressionLanguage
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();


            SourceCode.Text = 
@"var xx;
xx=2;
if(xx>6)
{
    xx=1;
}
else
{
    xx=2;
}
2+xx;";
        }

        private void OnParseClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OutPut.Text = Compiler.Compile(SourceCode.Text).ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}