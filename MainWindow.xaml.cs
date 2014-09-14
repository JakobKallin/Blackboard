using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blackboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAlignment();
            InitializeActions();
        }

        private FontFamily textFont = new FontFamily("Segoe UI");
        private FontFamily codeFont = new FontFamily("Consolas");

        private SolidColorBrush regularBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush hiddenCaretBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

        private void InitializeAlignment()
        {
            Alignment = Mode.Text;
        }

        private void ToggleMode()
        {
            Alignment = Alignment == Mode.Text ? Mode.Code : Mode.Text;
        }

        private enum Mode { Text, Code }
        private Mode alignment;
        private Mode Alignment
        {
            get { return alignment; }

            set
            {
                alignment = value;
                if ( alignment == Mode.Code )
                {
                    TextBox.Background = regularBrush;
                    TextBox.FontFamily = codeFont;
                }
                else
                {
                    TextBox.Background = hiddenCaretBrush;
                    TextBox.FontFamily = textFont;
                }
            }
        }

        private bool CtrlIsDown
        {
            get
            {
                return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            }
        }

        private delegate void Action();
        private Dictionary<Key, Action> actions;
        private Dictionary<Key, Action> ctrlActions;
        private void InitializeActions()
        {
            actions = new Dictionary<Key, Action>()
            {
                { Key.Tab, () => ToggleMode() },
                { Key.Escape, () => Application.Current.Shutdown() }
            };

            ctrlActions = new Dictionary<Key,Action>()
            {
                { Key.OemPlus, () => {
                    TextBox.FontSize = Math.Max(1, TextBox.FontSize + 10);
                } },
                { Key.OemMinus, () => {
                    TextBox.FontSize = Math.Max(1, TextBox.FontSize - 10);
                } }
            };
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            Action action = null;

            if ( CtrlIsDown && ctrlActions.ContainsKey(key) )
            {
                action = ctrlActions[key];
            }
            else if ( actions.ContainsKey(key) )
            {
                action = actions[key];
            }

            if ( action != null )
            {
                action();
                e.Handled = true;
            }
        }
    }
}
