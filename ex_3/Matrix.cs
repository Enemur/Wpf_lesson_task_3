using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using ex_3.Annotations;

namespace ex_3
{
    public class Matrix : INotifyPropertyChanged, ICloneable
    {
        public ObservableCollection<ObservableCollection<double>> Coefficients { get; set; }
        public int Size { get; set; }

        public Matrix(int size = 0)
        {
            Resize(size);
        }

        public DataTable Data
        {
            get
            {
                _table = new DataTable();

                for (int x = 0; x < Size; x++)
                    _table.Columns.Add(x.ToString());
                

                for (int i = 0; i < Size; i++)
                {
                    var row = new List<object>();
                    for (int j = 0; j < Size; j++)
                    {
                        row.Add(Coefficients[i][j]);
                    }
                    _table.Rows.Add(row.ToArray());
                }

                return _table;
            }
            set { }
        }

        private DataTable _table;

        public static Matrix Addition(Matrix left, Matrix right)
        {
            if (left.Size != right.Size)
                throw new Exception("Матрицы разного размера");

            var resMatrix = new Matrix(left.Size);

            for (var x = 0; x < left.Size; x++)
                for (var y = 0; y < left.Size; y++)
                    resMatrix.Coefficients[x][y] = left.Coefficients[x][y] + right.Coefficients[x][y];

            return resMatrix;
        }

        public static Matrix Substraction(Matrix left, Matrix right)
        {
            if (left.Size != right.Size)
                throw new Exception("Матрицы разного размера");

            var resMatrix = new Matrix(left.Size);

            for (var x = 0; x < left.Size; x++)
                for (var y = 0; y < left.Size; y++)
                    resMatrix.Coefficients[x][y] = left.Coefficients[x][y] - right.Coefficients[x][y];

            return resMatrix;
        }

        public static Matrix Multiplication(Matrix left, Matrix right)
        {
            if (left.Size != right.Size)
                throw new Exception("Матрицы разного размера");

            var resMatrix = new Matrix(left.Size);

            for (var x = 0; x < left.Size; x++)
                for (var y = 0; y < left.Size; y++)
                    for (var t = 0; t < left.Size; t++)
                        resMatrix.Coefficients[x][y] += left.Coefficients[x][t] * right.Coefficients[t][y];

            return resMatrix;
        }

        public void Resize(int size)
        {
            Coefficients = new ObservableCollection<ObservableCollection<double>>();
            for (int i = 0; i <size; i++)
            {
                Coefficients.Add(new ObservableCollection<double>());
                for (int j = 0; j <size; j++)
                {
                    Coefficients[i].Add(0);
                }
            }

            Size = size;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            var matrix = new Matrix(Size);
            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    matrix.Coefficients[i][j] = Coefficients[i][j];
                }
            }
            return matrix;
        }

        public void NewValues(List<double> coef)
        {
            var index = 0;

            for (int i = 0; i < Size; i++)
            {
                var row = new List<object>();
                for (int j = 0; j < Size; j++)
                    Coefficients[i][j] = coef[index];
            }
            OnPropertyChanged("Data");   
        }
    }
}