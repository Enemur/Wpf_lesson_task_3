using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using ex_3.Annotations;

namespace ex_3
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public Matrix FirstMatrix { get; set; }
        public Matrix SecondMatrix { get; set; }
        public Matrix ResultMatrix
        {
            get => _resMatrix;
            set
            {
                _resMatrix = value;
                OnPropertyChanged("ResultMatrix");
            } }

        private Matrix _resMatrix;

        public ObservableCollection<string> Actions { get; set; }
        public string SelectedItem { get; set; } 

        private List<Func<Matrix, Matrix, Matrix>> _actions;

        public ICommand ResultCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }

        public AppViewModel()
        {
            FirstMatrix = new Matrix();
            SecondMatrix = new Matrix();
            ResultMatrix = new Matrix();

            Actions = new ObservableCollection<string>()
            {
                "Addition",
                "Substraction",
                "Multiplication"
            };
            _actions = new List<Func<Matrix, Matrix, Matrix>>()
            {
                Matrix.Addition,
                Matrix.Substraction,
                Matrix.Multiplication
            };

            ResultCommand = new Command(x =>
            {
                if (SelectedItem != null)
                {
                    var result = _actions[Actions.IndexOf(SelectedItem)].Invoke(FirstMatrix, SecondMatrix);
                    ResultMatrix = result.Clone() as Matrix;
                }
            });

            OpenFileCommand = new Command(OpenFile);
        }

        private void OpenFile(object o)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "txt files (*.txt) | *.txt";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    var file = fileDialog.FileName;
                    ReadMtrixs(file);
                }
            }
        }

        public void ReadMtrixs(string file)
        {
            var countReadedLine = 0;
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    countReadedLine++;
                    if (countReadedLine % 2 == 1)
                    {
                        var size = int.Parse(line);

                        if (countReadedLine == 1)
                            FirstMatrix.Resize(size);
                        else if (countReadedLine == 3)
                            SecondMatrix.Resize(size);
                    }
                    else
                    {
                        var coefficients = line.Split(' ');
                        var cof = new List<double>();
                        Matrix matrix = null;

                        if (countReadedLine == 2)
                            matrix = FirstMatrix;
                        else if (countReadedLine == 4)
                            matrix = SecondMatrix;
                        var index = 0;
                        for (int i = 0; i < matrix.Size; i++)
                        for (int j = 0; j < matrix.Size; j++)
                            cof.Add(double.Parse(coefficients[index++]));

                        matrix.NewValues(cof);

                        ResultMatrix = new Matrix();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}